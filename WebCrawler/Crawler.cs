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
    /* A Web Crawler class that analyzes the html contents of websites, 
     extracts all absolute URL links, and then further analyzes those links. */
    public class Crawler
    {
        private Hashtable _urls;
        private int _numPages = 0;
        private int maxLinks; // maximum number of links to analyze

        public Crawler(string url, int numLinks)
        {
            // Add the new url to the initial hashtable
            _urls = new Hashtable();
            _urls[url] = false; // or _urls.Add(url, false);
            maxLinks = numLinks;
        }

        // Starts the crawling step
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
                }
                if (candidateURL == null || _numPages >= maxLinks) break;

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
            // Use error handling for invalid url paths
            try
            {
                // Use WebClient to download the page data
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(url);

                // Writes the html content to a new file
                FileStream fs = new FileStream(_numPages.ToString(), FileMode.OpenOrCreate);
                fs.Write(data, 0, data.Length);
                fs.Close();

                // Convert page data array to string
                string str = Encoding.UTF8.GetString(data);
                //Console.WriteLine(str);
                return str;
            }
            catch
            {
            }
            // simply return nothing in the case of an invalid url path
            // and no further links will get parsed from this link
            return "";

            /* Other ways to download the html contents from a web page */

            // Download the page data in string format
            // return client.DownloadString(url);

            // Use WebRequest/WebResponse and StreamReader
            // HttpWebRequest req = (HttpWebRequest) WebRequest.Create(url);
            // HttpWebResponse resp = (HttpWebResponse) req.GetResponse();
            // Stream stream = resp.GetResponseStream();
            // return new StreamReader(stream).ReadToEnd();
        }

        // Parse the given html of a page and find the url links 
        // referenced and sourced in this page
        private void Parse(string html)
        {
            // regex reference
            // example href="https://brand.rice.edu" 
            string refRegex = @"(href|HREF|src|SRC)[ ]*=[ ]*[""'][^""'#>]+[""']";

            MatchCollection matches = new Regex(refRegex).Matches(html);
            foreach(Match match in matches)
            {
                // process this matched string and extract the url
                string trimmedMatch = match.Value.Substring(match.Value.IndexOf("=")
                    + 1).Trim('"', '\'', '#', ' ', '>');
                if (trimmedMatch.Length == 0) continue;

                // add the new url to the hashtable
                if (_urls[trimmedMatch] == null) _urls[trimmedMatch] = false;
            }
        }

        
    }
}
