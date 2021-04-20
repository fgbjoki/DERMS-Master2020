using System;
using System.Xml.Serialization;

namespace Common.WeatherAPI
{
    [Serializable]
    [XmlRoot(ElementName = "hour")]
    public class WeatherHourData
    {
        private string timeUsedForParsing;

        [XmlElement("temp_c")]
        public float Temperature { get; set; }

        [XmlElement("time")]
        public string TimeUsedToParse
        {
            get { return timeUsedForParsing; }
            set
            {
                timeUsedForParsing = value;
                InternalParseHour();
            }
        }

        public ushort Hour { get; set; }

        [XmlElement("is_day")]
        public bool IsSunny { get; set; }

        [XmlElement("wind_kph")]
        public float WindKPH { get; set; }

        [XmlElement("cloud")]
        public float CloudCover { get; set; }

        private void InternalParseHour()
        {
            string temp = TimeUsedToParse.Substring(TimeUsedToParse.IndexOf(" ") + 1, 2);
            temp = temp[0] == '0' ? temp.Substring(1) : temp;
            Hour = ushort.Parse(temp);
        }
    }
}
