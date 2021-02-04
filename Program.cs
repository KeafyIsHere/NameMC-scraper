using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
namespace MinecraftUsernameScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> usernames = new List<string>();
            WebClient wc = new WebClient();
            string url = "https://namemc.com/minecraft-names";

            Console.WriteLine("What is the highest amount of characters should the username have?");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int numofchar)) 
            {
                Console.WriteLine(input + " is not a number!");
                Thread.Sleep(-1);
            }
            Console.WriteLine("Going to Scrape from 50 pages...");
            try
            {
                for (int i = 0; i < 50; i++)
                {
                    wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36 OPR/71.0.3770.287");
                    Console.WriteLine(url);
                    string data = wc.DownloadString(url);
                    string[] splits = data.Split(new string[] { "/name/" }, StringSplitOptions.None);
                    usernames.AddRange(from string split2 in splits
                                       let username = split2.Split('"')[0].Trim()
                                       where username.Length <= 11
                                       select username);
                    foreach (string a in data.Split(new string[] { "/minecraft-names?time=" }, StringSplitOptions.None).Where(a => a.Contains("next")))
                    {
                        url = "https://namemc.com/minecraft-names?time=" + a.Split('"')[0];
                    }
                    wc.Dispose();
                }
            }
            catch { }
            File.AppendAllLines("usernames.txt", usernames.Distinct());
        }
    }
}
