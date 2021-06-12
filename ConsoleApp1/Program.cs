using System;
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
            int menu_select;
            string urlString = "asshttps://docs.microsoft.com/en-us/dotnet/api/system.net.decompressionmethods?view=net-5.0";
            string reply = "";
            int nbrRetries = 3;

            Console.Clear();
            reply = GetTitle(urlString, nbrRetries);

            Console.WriteLine(reply);      
            
        }
    
        static string GetTitle(string url, int nbrRetries)
        {

                bool isOk = checkWebRequest(nbrRetries);
                if(isOk == false)
                    return "Max Attempts reached, function terminated";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

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

        static bool checkWebRequest(int nbrRetries)
        {
            bool hasError = false;
            int retries = 0;
            do
            {
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("sasshttp://www.google.com");
                    req.Timeout = 10000;
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    return true;
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

            return false;
        }
    }
}
