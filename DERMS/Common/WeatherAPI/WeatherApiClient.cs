using System;
using System.Net;
using System.Xml.Linq;

namespace Common.WeatherApiTester
{
    public class WeatherApiClient : IWeatherClient
    {
        private static readonly string apiUrl = "http://api.weatherapi.com/v1/@QUERYTYPE@.xml?key=@KEY@&q=@CITY@&";
        private static readonly string queryParameter = "@QUERYTYPE@";
        private static readonly string keyParameter = "@KEY@";
        private static readonly string cityParameter = "@CITY@";

        private static readonly string forecastParameter = "forecast";

        private static readonly int futureDays = 2;

        private string url;

        public WeatherApiClient(string apiKey, string city)
        {
            url = apiUrl.Replace(keyParameter, apiKey).Replace(cityParameter, city);
        }

        public WeatherDayData GetNextDayWeatherData()
        {
            WeatherDayData dayData = null;

            try
            {
                dayData = DownloadNextDayWeatherData();
            }
            catch (Exception e)
            {
                // log exception
            }

            return dayData;
        }

        private WeatherDayData DownloadNextDayWeatherData()
        {
            WeatherDayData returnData = null;

            string additionalParameter = $"days={futureDays}";
            string tempUrl = url.Replace(queryParameter, forecastParameter) + additionalParameter;
            string xmlContent = String.Empty;

            using (WebClient client = new WebClient())
            {
                xmlContent = client.DownloadString(tempUrl);
            }

            XElement xml = XElement.Parse(xmlContent);

            returnData = WeatherDayXMLParser.ParseXMLElements(xml)[0];

            return returnData;
        }
    }
}
