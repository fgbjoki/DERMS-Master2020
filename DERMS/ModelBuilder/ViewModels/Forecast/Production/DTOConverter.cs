using ClientUI.ViewModels.Forecast.Production.ProductionSeriesManager;
using Common.AbstractModel;
using Common.UIDataTransferObject.Forecast.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.ViewModels.Forecast.Production
{
    public class DTOConverter
    {
        public List<Tuple<PowerType, List<float>>> Convert(ProductionForecastDTO productionForecastDTO)
        {
            List<Tuple<PowerType, List<float>>> convertedData = new List<Tuple<PowerType, List<float>>>(3);

            List<float> totalConvertedData = ConvertTotalProductionForecast(productionForecastDTO.TimedForecast);
            convertedData.Add(new Tuple<PowerType, List<float>>(PowerType.Total, totalConvertedData));

            convertedData.AddRange(ConvertIndividualTypeProductionForecast(productionForecastDTO.GeneratorProductionForecasts));

            return convertedData;
        }

        public List<float> CreateValues(List<GeneratorProductionForecast> generatorProduction)
        {
            return new List<float>(generatorProduction.Select(x => x.ActivePower));
        }

        private List<float> ConvertTotalProductionForecast(TotalProductionForecast totalProductionForecastDTO)
        {
            List<float> convertedData = new List<float>(totalProductionForecastDTO.TotalForecast.Count);

            foreach (var sampleValue in totalProductionForecastDTO.TotalForecast)
            {
                convertedData.Add(sampleValue.Item2);
            }

            return convertedData;
        }

        private List<Tuple<PowerType, List<float>>> ConvertIndividualTypeProductionForecast(Dictionary<long, List<GeneratorProductionForecast>> productionForecast)
        {
            Dictionary<DMSType, List<float>> convertedData = new Dictionary<DMSType, List<float>>(2)
            {
                { DMSType.SOLARGENERATOR, new List<float>(25) },
                { DMSType.WINDGENERATOR, new List<float>(25) },
            };

            foreach (var values in convertedData.Values)
            {
                for (int i = 0; i < 25; i++)
                {
                    values.Add(0);
                }
            }

            foreach (var generatorProductionSample in productionForecast)
            {
                DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(generatorProductionSample.Key);
                List<float> totalValues = convertedData[dmsType];

                for (int i = 0; i < generatorProductionSample.Value.Count; i++)
                {
                    totalValues[i] += generatorProductionSample.Value[i].ActivePower;
                }
            }

            List<Tuple<PowerType, List<float>>> returnValues = new List<Tuple<PowerType, List<float>>>(2)
            {
                new Tuple<PowerType, List<float>>(PowerType.Solar, convertedData[DMSType.SOLARGENERATOR]),
                new Tuple<PowerType, List<float>>(PowerType.Wind, convertedData[DMSType.WINDGENERATOR])
            };

            return returnValues;
        }
    }
}
