using HtmlAgilityPack;
using CsvHelper;
using System.Globalization;
using System.Reflection.Metadata;
using System.Collections.Concurrent;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace WebScraperPagination{
    class Program{
        // main version        
        public static void Main(string[] args){       



            var web = new HtmlWeb();
            //global user agent header
            web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36";

            //instantiate variables
            var products = new List<Product>();
            var firstPageToScrape = "https://www.scrapingcourse.com/ecommerce/";
            var pagesDiscovered = new List<string> {firstPageToScrape};
            var pagesToScrape = new Queue<string>();

            //initialize the list w/ firstPageToScrape
            pagesToScrape.Enqueue(firstPageToScrape);
            int iteration = 1;
            int max = 12;

            while(pagesToScrape.Count != 0 && iteration < max){
                var currentPage = pagesToScrape.Dequeue();
                var currentDocument = web.Load(currentPage);
                var paginationHtmlElements = currentDocument.DocumentNode.QuerySelectorAll("a.page-numbers");

                //avoid visiting a page twice
                foreach(var element in paginationHtmlElements){
                    //extracting current pagination url
                    var newPaginationLink = element.Attributes["href"].Value;

                    //if the discovered page is new
                    if(!pagesDiscovered.Contains(newPaginationLink)){
                        //if the page discovered needs to be scraped
                        if(!pagesToScrape.Contains(newPaginationLink)){
                            pagesToScrape.Enqueue(newPaginationLink);                            
                        }
                        pagesDiscovered.Add(newPaginationLink);
                    }
                }

                var productHTMLElement  = currentDocument.DocumentNode.QuerySelectorAll("li.product");

                //iterate over the list of productHTMLElement 
                foreach(var prod in productHTMLElement ){
                    var url = HtmlEntity.DeEntitize(prod.QuerySelector("a").Attributes["href"].Value);
                    var image = HtmlEntity.DeEntitize(prod.QuerySelector("img").Attributes["src"].Value);
                    var name = HtmlEntity.DeEntitize(prod.QuerySelector("h2").InnerText.Trim());
                    var price = HtmlEntity.DeEntitize(prod.QuerySelector(".price").InnerText.Trim());


                    //only add products starting with letter A
                    if(name.ToLower().StartsWith("b") || name.ToLower().StartsWith("a")){
                        var product = new Product() {
                            Url = url,
                            Image = image,
                            Name = name,
                            Price = price
                        };
                        products.Add(product);
                    }


                }
                iteration++;
                WriteToCsv(products);

            }            
        }//end of main

        public static void WriteToCsv (List<Product> products) {
            string fileName = "product.csv";
            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
                csv.WriteRecords(products);
            }
        }


        int n = 0;
        //that int n = 0 is just to sepereate the two main methods
            //because if there isn't code here, the comment below will be lumped with the commented code block Main() above.

        //selenium webdriver version
        // public static void Main() {


        //     var products = new List<Product>();

        //     //open chrome in headless mode
        //     var chromeOptions = new ChromeOptions();
        //     chromeOptions.AddArguments("headless");

        //     //starting selenium instance
        //     using (var driver = new ChromeDriver(chromeOptions)){
        //         //navigate to the target page in browser

        //         driver.Navigate().GoToUrl("https://www.scrapingcourse.com/ecommerce/");

        //         //getting html product elements
        //         var productHTMLElements =  driver.FindElements(By.CssSelector("li.product"));

        //         //iterate over them to scrape relevant data
        //         foreach(var prod in productHTMLElements){
        //             var url = prod.FindElement(By.CssSelector("a")).GetAttribute("href"); 
		// 			var image = prod.FindElement(By.CssSelector("img")).GetAttribute("src"); 
		// 			var name = prod.FindElement(By.CssSelector("h2")).Text; 
		// 			var price = prod.FindElement(By.CssSelector(".price")).Text; 
 
		// 			var product = new Product() { Url = url, Image = image, Name = name, Price = price }; 
 
		// 			products.Add(product); 
        //         }
        //     }           

        //     //export to csv
        //     using (var writer = new StreamWriter("productSelenium.csv"))
        //     using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
        //         csv.WriteRecords(products);
        //     }

        // }
   
   
   
    }
}