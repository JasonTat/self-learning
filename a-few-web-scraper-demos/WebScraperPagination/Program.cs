using HtmlAgilityPack;
using CsvHelper;
using System.Globalization;
using System.Reflection.Metadata;
using System.Collections.Concurrent;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace WebScraperPagination{
    class Program{
        

        //main version        
        // static void Main(string[] args){           
            
        //     //initialize HtmlAgilityPack
        //     var web = new HtmlWeb();

        //     //global user agent header
        //     web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36";

        //     //instantiate new list of products to store the scraped data
        //     var products = new List<Product>();

        //     //first page to scrape
        //     var firstPageToScrape = "https://www.scrapingcourse.com/ecommerce/";

        //     //list of pages discovered
        //     var pagesDiscovered = new List<string> {firstPageToScrape};

        //     //list of pages that remain to be scraped
        //     var pagesToScrape = new Queue<string>();

        //     //initialize the list w/ firstPageToScrape
        //     pagesToScrape.Enqueue(firstPageToScrape);

        //     //current scrape iteration
        //     int iteration = 1;

        //     //max number of pages to scrape before stopping
        //     int max = 12;

        //     //until there are no pages to scrape or limit is hit

        //     while(pagesToScrape.Count != 0 && iteration < max){
        //         //extract current page from the queue to scrape
        //         var currentPage = pagesToScrape.Dequeue();

        //         //loading the page 
        //         var currentDocument = web.Load(currentPage);

        //         //select list of pagination from html elements
        //         var paginationHtmlElements = currentDocument.DocumentNode.QuerySelectorAll("a.page-numbers");

        //         //avoid visiting a page twice
        //         foreach(var element in paginationHtmlElements){
        //             //extracting current pagination url
        //             var newPaginationLink = element.Attributes["href"].Value;

        //             //if the discovered page is new
        //             if(!pagesDiscovered.Contains(newPaginationLink)){
        //                 //if the page discovered needs to be scraped
        //                 if(!pagesToScrape.Contains(newPaginationLink)){
        //                     pagesToScrape.Enqueue(newPaginationLink);                            
        //                 }
        //                 pagesDiscovered.Add(newPaginationLink);
        //             }
        //         }

        //         //get all <li> elements with the class 'li.product'
        //         var productHTMLElement  = currentDocument.DocumentNode.QuerySelectorAll("li.product");

        //         //iterate over the list of productHTMLElement 
        //         foreach(var prod in productHTMLElement ){
        //             //scrape relevant data from html element
        //             var url = HtmlEntity.DeEntitize(prod.QuerySelector("a").Attributes["href"].Value);
        //             var image = HtmlEntity.DeEntitize(prod.QuerySelector("img").Attributes["src"].Value);
        //             var name = HtmlEntity.DeEntitize(prod.QuerySelector("h2").InnerText.Trim());
        //             var price = HtmlEntity.DeEntitize(prod.QuerySelector(".price").InnerText.Trim());


        //             var product = new Product() {
        //                 Url = url,
        //                 Image = image,
        //                 Name = name,
        //                 Price = price
        //             };
        //             products.Add(product);
        //         }

        //         //increment iteration counter
        //         iteration++;

        //         //create csv file with streamreader and csvhelper
        //         using (var writer = new StreamWriter("products.csv"))
        //         using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
        //             //populate csv file w/ the data
        //             csv.WriteRecords(products);
        //         }
        //     }            
        // }
   
        //selenium webdriver version
        public static void Main() {


            var products = new List<Product>();

            //open chrome in headless mode
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");

            //starting selenium instance
            using (var driver = new ChromeDriver(chromeOptions)){
                //navigate to the target page in browser

                driver.Navigate().GoToUrl("https://www.scrapingcourse.com/ecommerce/");

                //getting html product elements
                var productHTMLElements =  driver.FindElements(By.CssSelector("li.product"));

                //iterate over them to scrape relevant data
                foreach(var prod in productHTMLElements){
                    var url = prod.FindElement(By.CssSelector("a")).GetAttribute("href"); 
					var image = prod.FindElement(By.CssSelector("img")).GetAttribute("src"); 
					var name = prod.FindElement(By.CssSelector("h2")).Text; 
					var price = prod.FindElement(By.CssSelector(".price")).Text; 
 
					var product = new Product() { Url = url, Image = image, Name = name, Price = price }; 
 
					products.Add(product); 
                }
            }           

            //export to csv
            using (var writer = new StreamWriter("productSelenium.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
                csv.WriteRecords(products);
            }

        }
   
   
   
    }
}