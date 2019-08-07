# WebCrawler

A web crawler that iteratively collects all absolute links from a webpage and downloads the html content from these linked websites.

Specifically, the crawler starts with a website (e.g. https://rice.edu), downloads its html content, searches
links used in this page through parsing, and then further downloads the html content of those linked websites.
Urls are parsed and saved in a Hashtable, with the key being the url of the website, and the value being false before
the content of the website is downloaded and parsed, and true after downloading.

Written in C#