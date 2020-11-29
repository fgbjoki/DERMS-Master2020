using System;
using System.Xml.Serialization;

namespace Common.WeatherApiTester
{
    [Serializable]
    [XmlRoot(ElementName = "hour")]
    public class WeatherHourData
    {
        [XmlElement("temp_c")]
        public float Temperature { get; set; }

        [XmlElement("time")]
        public string TimeUsedToParse { get; set; }

        public ushort Hour { get; set; }

        [XmlElement("is_day")]
        public bool IsSunny { get; set; }

        [XmlElement("wind_kph")]
        public float WindKPH { get; set; }

        [XmlElement("cloud")]
        public float CloudCover { get; set; }

        public void InternalParseHour()
        {
            string temp = TimeUsedToParse.Substring(TimeUsedToParse.IndexOf(" ") + 1, 2);
            temp = temp[0] == '0' ? temp.Substring(1) : temp;
            Hour = ushort.Parse(temp);
        }
    }
}
