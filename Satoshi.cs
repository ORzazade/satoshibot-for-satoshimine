// Decompiled with JetBrains decompiler
// Type: SatoshiBot.Satoshi
// Assembly: SatoshiBot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 63885AB5-41F9-4B2D-AC53-92693431213D
// Assembly location: C:\Users\root\Desktop\SatoshiBot.exe

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SatoshiBot
{
    internal class Satoshi
    {
        public string message = "";
        private DepositData _lastDepositData = (DepositData)null;
        private HttpWebRequest _httpRequests;

        public GameData Data { get; private set; }

        public string PlayerHash { get; private set; }

        public Satoshi(string hash)
        {
            this.PlayerHash = hash;
        }

        public void NewGame(int mines, Decimal betSatoshi)
        {
            try
            {
                this.PrepRequest("https://satoshimines.com/action/newgame.php");
                
                string postResponce = this.getPostResponce(Encoding.UTF8.GetBytes(string.Format("player_hash={0}&bet={1}&num_mines={2}&bd={3}", (object)this.PlayerHash, (object)(betSatoshi / new Decimal(1000000)).ToString("0.000000", (IFormatProvider)new CultureInfo("en-US")), (object)mines, (object)"12")));
                this.Data = Satoshi.Deserialize<GameData>(WebUtility.HtmlDecode(postResponce));
                this.message = postResponce;
            }
            catch (Exception ex)
            {
                this.Data = (GameData)null;
            }
        }

        public BetData Bet(int squaer)
        {
            try
            {
                this.PrepRequest("https://satoshimines.com/action/checkboard.php");
                return Satoshi.Deserialize<BetData>(this.getPostResponce(Encoding.UTF8.GetBytes(string.Format("game_hash={0}&guess={1}&v04=1", (object)this.Data.game_hash, (object)squaer))));
            }
            catch
            {
                return (BetData)null;
            }
        }
        public static void Log(string logMessage, TextWriter w)
        {
            //w.Write("\r\nLog Entry : ");
          //  w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
          //      DateTime.Now.ToLongDateString());          
            w.WriteLine("{0}", logMessage);
           // w.WriteLine("-------------------------------");
        }
        public CashOutData CashOut()
        {
            try
            {
                this.PrepRequest("https://satoshimines.com/action/cashout.php");
                var data = this.getPostResponce(Encoding.UTF8.GetBytes(string.Format("game_hash={0}", (object)this.Data.game_hash)));              

                var rData =  Satoshi.Deserialize<CashOutData>(data);
                using (StreamWriter w = File.AppendText("log.html"))
                {
                    Log("<a target='_blank' href='https://satoshimines.com/s/"+rData.game_id+"/"+rData.random_string+ "/'>https://satoshimines.com/s/" + rData.game_id + "/" + rData.random_string + "/</a><br/>", w);
                }
                return rData;
            }
            catch
            {
                return (CashOutData)null;
            }
        }

        public DepositData GetDepositData()
        {
            if (this._lastDepositData != null && this._lastDepositData.status == "success")
                return this._lastDepositData;
            try
            {
                this.PrepRequest("https://satoshimines.com/action/getaddr.php");
                return Satoshi.Deserialize<DepositData>(this.getPostResponce(Encoding.UTF8.GetBytes(string.Format("secret={0}", (object)this.PlayerHash))));
            }
            catch
            {
                return (DepositData)null;
            }
        }

        public void TryWithdraw(string BTCAddr, int AmmountSatoshi)
        {
            try
            {
                this.PrepRequest("https://satoshimines.com/action/full_cashout.php");
                this.getPostResponce(Encoding.UTF8.GetBytes(string.Format("secret={0}&payto_address={1}&amount={2}", (object)this.PlayerHash, (object)BTCAddr, (object)(AmmountSatoshi / 1000000).ToString("0.000000", (IFormatProvider)new CultureInfo("en-US")))));
            }
            catch
            {
            }
        }

        private void PrepRequest(string url)
        {
            this._httpRequests = (HttpWebRequest)WebRequest.Create(url);
            this._httpRequests.Method = "POST";
            this._httpRequests.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
        }

        private string getPostResponce(byte[] post)
        {
            this._httpRequests.ContentLength = (long)post.Length;
            using (Stream requestStream = this._httpRequests.GetRequestStream())
            {
                requestStream.Write(post, 0, post.Length);
                requestStream.Flush();
            }
            return new StreamReader(this._httpRequests.GetResponse().GetResponseStream()).ReadToEnd();
        }

        private static T Deserialize<T>(string json)
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject((Stream)memoryStream);
        }
    }
}
