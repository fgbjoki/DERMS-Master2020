using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Common.WeatherApiTester
{
    public class WeatherApiClient : IWeatherClient
    {
        private static readonly string apiUrl = "http://api.weatherapi.com/v1/@QUERYTYPE@.xml?key=@KEY@&q=@CITY@&";
        private static readonly string queryParameter = "@QUERYTYPE@";
        private static readonly string keyParameter = "@KEY@";
        private static readonly string cityParameter = "@CITY@";

        private static readonly string forecastParameter = "forecast";
        private static readonly string historyParameter = "history";

        private static readonly int futureDays = 3;

        private string url;

        public WeatherApiClient(string apiKey, string city)
        {
            url = apiUrl.Replace(keyParameter, apiKey).Replace(cityParameter, city);
        }

        public List<WeatherDayData> GetWeatherDayData(int numberOfDays)
        {
            List<WeatherDayData> daysData = new List<WeatherDayData>();

            using (WebClient client = new WebClient())
            {
                daysData = GetFutureDays(client);
                daysData.AddRange(GetHistoryDays(client, numberOfDays - 3));
            }

            return daysData;
        }

        private List<WeatherDayData> GetFutureDays(WebClient client)
        {
            List<WeatherDayData> returnData = null;

            string additionalParameter = $"days={futureDays}";
            string tempUrl = url.Replace(queryParameter, forecastParameter) + additionalParameter;

            string xmlContent = client.DownloadString(tempUrl);

            XElement xml = XElement.Parse(xmlContent);

            // parse XML
            IEnumerable<XElement> xelements =
            from elements in xml.Descendants("hour")
            select elements;

            returnData = WeatherDayXMLParser.ParseXMLElements(xelements.ToList());
           
            return returnData;
        }

        private List<WeatherDayData> GetHistoryDays(WebClient client, int numberofDays)
        {
            List<WeatherDayData> returnData = new List<WeatherDayData>(0);
            DateTime dateTime = DateTime.Now;
            TimeSpan daySubtraction = new TimeSpan(1, 0, 0, 0);

            for (int i = 0; i < numberofDays; i++)
            {
                string additionalParameter = $"dt={dateTime.Year}-{dateTime.Month}-{dateTime.Day}";
                string tempUrl = url.Replace(queryParameter, historyParameter) + additionalParameter;

                string xmlContent = client.DownloadString(tempUrl);

                XElement xml = XElement.Parse(xmlContent);

                // parse XML
                IEnumerable<XElement> xelements =
                from elements in xml.Descendants("hour")
                select elements;

                returnData.AddRange(WeatherDayXMLParser.ParseXMLElements(xelements.ToList()));

                dateTime = dateTime.Subtract(daySubtraction);
            }

            return returnData;
        }
    }
}
