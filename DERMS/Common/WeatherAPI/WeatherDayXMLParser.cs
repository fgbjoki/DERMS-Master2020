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
        public static List<WeatherDayData> ParseXMLElements(XElement xmlContent)
        {
            IEnumerable<XElement> xElements =
                from elements in xmlContent.Descendants("hour")
                select elements;

            xElements = xElements.Skip(24);

            var returnList = new List<WeatherDayData>(1);

            var serializer = new XmlSerializer(typeof(WeatherHourData));

            WeatherDayData day = null;
            int i = 0;
            foreach (XElement xElement in xElements)
            {
                var hourData = (WeatherHourData)serializer.Deserialize(xElement.CreateReader());

                if (i % 24 == 0)
                {
                    day = new WeatherDayData(DateTime.ParseExact(hourData.TimeUsedToParse, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture));
                    returnList.Add(day);
                }

                day.AddHourData(hourData);

                ++i;
            }

            return returnList;
        }
    }
}
