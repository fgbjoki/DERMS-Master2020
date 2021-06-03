using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Common.WeatherAPI
{
    [Serializable]
    [XmlRoot(ElementName = "astro")]
    public class WeatherAstroData
    {
        private string sunriseTime;
        private string sunsetTime;

        [XmlElement("sunrise")]
        public string SunriseTime
        {
            get { return sunriseTime; }
            set
            {
                sunriseTime = value;
                Sunrise = InternalParseHour(sunriseTime);
            }
        }

        [XmlElement("sunset")]
        public string SunsetTime
        {
            get { return sunsetTime; }
            set
            {
                sunsetTime = value;
                Sunset = InternalParseHour(sunsetTime);
            }
        }

        [XmlIgnore]
        public TimeSpan Sunrise
        {
            get;
            private set;
        }

        [XmlIgnore]
        public TimeSpan Sunset
        {
            get;
            private set;
        }

        private TimeSpan InternalParseHour(string time)
        {
            int hourAddition = time.Contains("AM") ? 0 : 12;
            TimeSpan convertedTime = TimeSpan.ParseExact(time.Substring(0, time.Length - 3), "h\\:mm", CultureInfo.InvariantCulture) + new TimeSpan(hourAddition, 0, 0);
            return convertedTime;
        }
    }
}
