using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Common.WeatherAPI
{
    public class WeatherDayXMLParser
    {
        public static List<WeatherDayData> ParseXMLElements(XElement xmlContent)
        {
            IEnumerable<XElement> hours =
                from elements in xmlContent.Descendants("hour")
                select elements;

            IEnumerable<XElement> astroData =
                from elements in xmlContent.Descendants("astro")
                select elements;

            var returnList = new List<WeatherDayData>(1);

            var serializer = new XmlSerializer(typeof(WeatherHourData));
            var astroDeserializer = new XmlSerializer(typeof(WeatherAstroData));

            var astroEnumerator = astroData.GetEnumerator();

            WeatherDayData day = null;
            int i = 0;
            foreach (XElement hour in hours)
            {
                var hourData = (WeatherHourData)serializer.Deserialize(hour.CreateReader());

                if (i % 24 == 0)
                {
                    day = new WeatherDayData(DateTime.ParseExact(hourData.TimeUsedToParse, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture));
                    if (astroEnumerator.MoveNext())
                    {
                        day.LoadAstroDetails((WeatherAstroData)astroDeserializer.Deserialize(astroEnumerator.Current.CreateReader()));
                    }

                    returnList.Add(day);
                }

                day.AddHourData(hourData);

                ++i;
            }

            return returnList;
        }
    }
}
