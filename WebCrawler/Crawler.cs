using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    public class Crawler
    {
        private Hashtable _urls;
        private int _numPages = 0;

        public Crawler(string url)
        {
            // Add the new url to the initial hashtable
            _urls = new Hashtable();
            _urls[url] = false; // or _urls.Add(url, false);
        }

        public void Crawl()
        {
            Console.WriteLine("Start crawling...");
            while (true)
            {
                string candidateURL = null;
                // Go through the url hashtable
                // Find an unprocessed url from HT and download its contents
                foreach (string url in _urls.Keys)
                {
                    if (!(bool)_urls[url])
                    {
                        candidateURL = url;
                        break;
                    }

                    //if ((bool)_urls[url]) continue; //??????????????????????
                    //candidateURL = url;
                }
                if (candidateURL == null || _numPages > 10) break;

                Console.WriteLine("Analyzing {0}...", candidateURL);

                // Download the content in string format
                string html = Download(candidateURL);

                _urls[candidateURL] = true;
                _numPages++;

                // Parse the downloaded html string and store further links
                // used in that html page
                Parse(html);
            }
            Console.WriteLine("Finished crawling...");
        }

        // Downloads the html page content from the given url
        private string Download(string url)
        {
            try
            {
                //Console.WriteLine("Download function using {0}", url);
                // Use WebClient to download the page data
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(url);
                // Writes the html content to a new file
                FileStream fs = new FileStream(_numPages.ToString(), FileMode.OpenOrCreate);
                fs.Write(data, 0, data.Length);
                fs.Close();
                //return client.DownloadString(url);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
            }
            return "";
            //Console.WriteLine("Download function using {0}", url);
            //// Use WebClient to download the page data
            //WebClient client = new WebClient();
            //byte[] data = client.DownloadData(url);
            //// Writes the html content to a new file
            //FileStream fs = new FileStream(_numPages.ToString(), FileMode.OpenOrCreate);
            //fs.Write(data, 0, data.Length);
            //fs.Close();
            ////return client.DownloadString(url);
            //return Encoding.UTF8.GetString(data);

            // Other ways to download the html contents from a web page

            // Download the page data array then convert to a string
            //byte[] data = client.DownloadData(url);
            //return Encoding.UTF8.GetString(data);

            // Use WebRequest/WebResponse
            //HttpWebRequest req = (HttpWebRequest) WebRequest.Create(url);
            //HttpWebResponse resp = (HttpWebResponse) req.GetResponse();
            //Stream stream = resp.GetResponseStream();
            //return new StreamReader(stream).ReadToEnd();
        }

        // Parse the given html of a page and find the url links 
        // referenced and sourced in this page
        private void Parse(string html)
        {
            string strRef = @"(href|HREF|src|SRC)[ ]*=[ ]*[""'][^""'#>]+[""']";
            MatchCollection matches = new Regex(strRef).Matches(html);
            foreach (Match match in matches)
            {
                strRef = match.Value.Substring(match.Value.IndexOf('=') + 1).Trim('"', '\'', '#', ' ', '>');
                if (strRef.Length == 0) continue;

                if (_urls[strRef] == null) _urls[strRef] = false;
            }
        }

        
    }
}
