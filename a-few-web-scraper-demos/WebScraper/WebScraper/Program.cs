using HtmlAgilityPack;
using System;
using System.Net.Http;


namespace WebScraper
{
    class Program
    {
        static void Main(String[] args)
        {


            //Send get request to weather.com
            String url = "https://weather.com/en-CA/weather/today/l/584018bec07ce9573837c14fa59da031fa6fcdeb1c3c9e3b2b27cb79ce254b5a";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;
            var htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(html);


            //Get the temperature
            var temperatureElement = htmlDocument.DocumentNode.SelectSingleNode("//span[@class='CurrentConditions--tempValue--MHmYY']");
            var temperature = temperatureElement.InnerText.Trim();
            Console.WriteLine("Temperature: " + temperature);


            //get the conditions
            var conditionElement = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='CurrentConditions--phraseValue--mZC_p']");
            var condition = conditionElement.InnerText.Trim();
            Console.WriteLine("Condition: " + condition);


            //Get the location
            var locationElement = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='CurrentConditions--location--1YWj_']");
            var location = locationElement.InnerText.Trim();
            Console.WriteLine("Location: " + location);


        }
    }
}