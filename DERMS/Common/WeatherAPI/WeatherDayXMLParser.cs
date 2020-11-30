using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Common.WeatherApiTester
{
    public class WeatherDayXMLParser
    {
        public static List<WeatherDayData> ParseXMLElements(List<XElement> xelements)
        {
            List<WeatherDayData> returnList = new List<WeatherDayData>(1);

            var serializer = new XmlSerializer(typeof(WeatherHourData));

            WeatherDayData day = null;
            for (int i = 0; i < xelements.Count(); i++)
            {
                WeatherHourData hourData = (WeatherHourData)serializer.Deserialize(xelements[i].CreateReader());

                if (i%24 == 0)
                {
                    day = new WeatherDayData(DateTime.ParseExact(hourData.TimeUsedToParse, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture));
                    returnList.Add(day);
                }

                day.AddHourData(hourData);
            }

            return returnList;
        }
    }
}
