using System;
using System.Net;
using System.Xml.Linq;

namespace Common.WeatherApiTester
{
    public class WeatherApiClient : IWeatherClient
    {
        private static readonly string API_URL = "http://api.weatherapi.com/v1/@QUERYTYPE@.xml?key=@KEY@&q=@CITY@&";
        private static readonly string QUERY_PARAMETER = "@QUERYTYPE@";
        private static readonly string KEY_PARAMETER = "@KEY@";
        private static readonly string CITY_PARAMETER = "@CITY@";

        private static readonly string FORECAST_PARAMETER = "forecast";

        private static readonly int FUTURE_DAYS = 2;

        private string url;

        public WeatherApiClient(string apiKey, string city)
        {
            url = API_URL.Replace(KEY_PARAMETER, apiKey).Replace(CITY_PARAMETER, city);
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

            string additionalParameter = $"days={FUTURE_DAYS}";
            string tempUrl = url.Replace(QUERY_PARAMETER, FORECAST_PARAMETER) + additionalParameter;
            string xmlContent = String.Empty;

            using (var client = new WebClient())
            {
                xmlContent = client.DownloadString(tempUrl);
            }

            XElement xml = XElement.Parse(xmlContent);

            returnData = WeatherDayXMLParser.ParseXMLElements(xml)[0];

            return returnData;
        }
    }
}
