using CalculationEngine.Forecast.ProductionForecast.Formulas;
using CalculationEngine.Model.Forecast.ProductionForecast;
using Common.AbstractModel;
using Common.ComponentStorage;
using Common.DataTransferObjects;
using Common.Logger;
using Common.ServiceInterfaces.CalculationEngine;
using System.Collections.Generic;
using System.Linq;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Production
{
    public class GeneratorDataPreparator
    {
        private Dictionary<DMSType, IProductionFormula> productionFormulas;
        private IStorage<Generator> storage;

        public GeneratorDataPreparator(IStorage<Generator> storage)
        {
            this.storage = storage;

            productionFormulas = new Dictionary<DMSType, IProductionFormula>()
            {
                { DMSType.SOLARGENERATOR, new SolarPanelProductionFormula() },
                { DMSType.WINDGENERATOR, new WindGeneratorProductionFormula() }
            };
        }

        public List<GeneratorProduction> GenerateData(WeatherDataInfo weatherData)
        {
            var generators = storage.GetAllEntities();

            List<GeneratorProduction> generatorProduction = new List<GeneratorProduction>(generators.Count);

            foreach (var generator in generators)
            {
                IProductionFormula formula;
                if (!productionFormulas.TryGetValue(generator.DMSType, out formula))
                {
                    Logger.Instance.Log($"[{GetType().Name}] Cannot find formula for entity with gid: 0x{generator.GlobalId:X16}");
                    continue;
                }

                float activePower = formula.CalculateProduction(generator, weatherData);

                GeneratorProduction forecast = new GeneratorProduction(generator.GlobalId, activePower);
                generatorProduction.Add(forecast);
            }

            return generatorProduction;
        }
    }
}
