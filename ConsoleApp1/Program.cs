using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        public static int min = 1;
        public static int max = 3;


        static void Main(string[] args)
        {
            
            string urlString = "https://docs.microsoft.com/en-us/dotnet/api/system.net.decompressionmethods?view=net-5.0";
            string reply = "";
            int nbrRetries = 3;
            int j = 0;
            List<string> testString = new List<string>();

            Console.Clear();
            reply = GetTitle(urlString, nbrRetries);
            Console.WriteLine("Title of URL");
            Console.WriteLine(reply);
            Console.WriteLine("----------------------------\n");

            testString = HandleTitle(reply);

            // Reverse the string index
            testString.Reverse();
            testString.ForEach(Console.WriteLine);

            Console.WriteLine("----------------------------\n");

            // Test faulty URL
            //urlString = "asdasdhttps://docs.microsoft.com/en-us/dotnet/api/system.net.decompressionmethods?view=net-5.0";
            reply = GetAllUrls(urlString, nbrRetries);

            Console.WriteLine(reply);
        }

        static string GetTitle(string url,int nbrRetries)
        {
            bool hasError = false;
            int retries = 0;

            do
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Timeout = 10000;

                    // Decompress the data recrived from HttpWebRequest
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    // Get the body of the stream response
                    using (Stream stream = response.GetResponseStream())

                    // Reads char from a byte stream
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        // read from start to end of the response
                        var str = reader.ReadToEnd();

                        // regex to find <title> header
                        // (.*) is to match all characters between <title></title> except line breaks 
                        Regex reg = new Regex("<title>(.*)</title>");

                        // will populate if match is found
                        MatchCollection m = reg.Matches(str);

                        if (m.Count > 0)
                        {
                            return m[0].Value.Replace("<title>", "").Replace("</title>", "");
                        }
                        else
                            return "Title not found, please check URL!";
                    }
                }
                catch (TimeoutException e)
                {
                    Console.WriteLine($"Attempt {retries +1} " + e.Message);
                    hasError = true;
                    ++retries;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Attempt {retries + 1} " + ex.Message);
                    hasError = true;
                    ++retries;
                }
                
            } while (hasError && retries < nbrRetries);
            return "Max Attempt reached";
        }

        static List<string> HandleTitle(string title)
        {
            // decalre a variable to store the list of strings
            List<string> lStr = new List<string>();

            // declare an array to temp store the words
            string[] tmpStr = title.Split(' ');
            
            // loop thru the temp array and add the word to the list of strings
            foreach(string word in tmpStr)
            {
                lStr.Add(word);
            }

            // return the list
            return lStr;
        }

        static string GetAllUrls(string url, int nbrRetries)
        {
            bool hasError = false;
            int retries = 0;
            string tmpStr = "";

            do
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Timeout = 10000;

                    // Decompress the data recrived from HttpWebRequest
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    // Get the body of the stream response
                    using (Stream stream = response.GetResponseStream())

                    // Reads char from a byte stream
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        // read from start to end of the response
                        var str = reader.ReadToEnd();

                        // regex to find https:// url 
                        // (.*) is to match all characters between https:// and ending '"' except line breaks 
                        Regex reg = new Regex("https://(.*)\"");

                        // will populate if match is found
                        MatchCollection m = reg.Matches(str);

                        if (m.Count > 0)
                        {
                            foreach (var i in m)
                            {
                                tmpStr += i + "\n";
                            }
                            return tmpStr;
                        }
                        else
                            return "No URL found!";
                    }
                }
                catch (TimeoutException e)
                {
                    Console.WriteLine($"Attempt {retries + 1} " + e.Message);
                    hasError = true;
                    ++retries;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempt {retries + 1} " + ex.Message);
                    hasError = true;
                    ++retries;
                }

            } while (hasError && retries < nbrRetries);
            return "Max Attempt reached";
        }
    }
}
