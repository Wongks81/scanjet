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

        static int showMenu()
        {
            string menu_selection = "";
            int iSelection = 0;
            bool err = false;

            //shows the menu for selection
            Console.WriteLine("Please select the function to run");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("(1) Get Title");
            Console.WriteLine("(2) Handle Title");
            Console.WriteLine("(3) Get All URLs");
            Console.WriteLine("---------------------------------");

            //loop till correct input is entered
            do
            {
                err = false;
                menu_selection = Console.ReadLine();

                if (!Int32.TryParse(menu_selection, out int m_out))
                {
                    err = true;
                }
                else
                {
                    iSelection = Convert.ToInt32(menu_selection);
                }

                if (iSelection < min || iSelection > max)
                {
                    err = true;
                }
                if (err == true)
                {
                    Console.WriteLine("Not a Valid Input, Please try again");
                }
            } while (err == true);

            return iSelection;
        }

        static string getURL()
        {
            string url = "";

            Console.WriteLine("Please enter the URL....");
            url = Console.ReadLine();

            return url;
        }

        static string GetTitle(string url, int nbrRetries)
        {
            // Create a webrequest
            try
            {
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Unable to proccess URL string, please check";
            }
        }
    }
}
