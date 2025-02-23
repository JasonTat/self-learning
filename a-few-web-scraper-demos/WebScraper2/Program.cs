﻿using System.Globalization;
using CsvHelper;
using HtmlAgilityPack;

namespace WebScraper2 {
    
    class Book{
        public string Title {get;set;}
        public string Price {get;set;}

    }



    class Program{ 

        static void Main (string[] args){
            var bookLinks = GetBookLinks("https://books.toscrape.com/catalogue/category/books/mystery_3/index.html");
            Console.WriteLine("Found {0} links", bookLinks.Count);
            var books = GetBookDetails(bookLinks);
            exportToCSV(books);
        }

        static void exportToCSV (List<Book> books){
            using (var writer = new StreamWriter("./books.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
                csv.WriteRecords(books);
            }
        }


        static List<Book> GetBookDetails(List<string> urls) {
            var books = new List<Book>();
            foreach(var url in urls){
                HtmlDocument document = GetDocument(url);
                var titleXPath = "//h1";
                var priceXPath = "//*[@id=\"content_inner\"]/article/div[1]/div[2]/p[1]";
                var book = new Book();
                book.Title = document.DocumentNode.SelectSingleNode(titleXPath).InnerText;
                book.Price = document.DocumentNode.SelectSingleNode(priceXPath).InnerText;
                books.Add(book);
            }
            return books;
        }
        

        //download the html webpage
        static HtmlDocument GetDocument(string url) {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc;
        }

        static List<string> GetBookLinks(string url){

            var bookLinks = new List<string>();

            HtmlDocument doc = GetDocument(url);
            HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//h3/a");
            var baseUri = new Uri(url);

            foreach(var link in linkNodes){
                string href = link.Attributes["href"].Value;
                bookLinks.Add(new Uri(baseUri, href).AbsoluteUri);
            }

            return bookLinks;

        }







    }





}