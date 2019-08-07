using System;
using System.Threading;

namespace WebCrawler
{
    public class Program
    {
        static void Main(string[] args)
        {
            // starts with a user-inputted url or Rice University's url by default
            string defaultURL = "https://www.rice.edu/";
            string url = args.Length >= 1 ? args[0] : defaultURL;
            Crawler crawler = new Crawler(url);

            // start running the crawler executing on a new thread
            new Thread(new ThreadStart(crawler.Crawl)).Start();

            Console.ReadLine();
        }
    }
}
