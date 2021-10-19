using System;
using System.Collections.Generic;
using System.IO;
using CalculationEngine.Commanding.BalanceForecastCommanding.GeneticAlgorithm.InitialPopulationCreation.Model;
using CalculationEngine.Model.EnergyCalculations;
using CalculationEngine.Model.Forecast.ConsumptionForecast;
using CalculationEngine.TransactionProcessing.Storage.EnergyBalance;
using CalculationEngine.TransactionProcessing.Storage.Forecast;
using Common.ComponentStorage;
using Common.DataTransferObjects;
using Common.Logger;
using Common.ServiceInterfaces.CalculationEngine;
using Keras.Models;
using Numpy;

namespace CalculationEngine.Commanding.BalanceForecastCommanding.DataPreparation.Consumption
{
    public class ConsumptionPreparation : IConsumptionPreparation
    {
        private static BaseModel model_Home;
        private static BaseModel model_Building;
        private static BaseModel model_Block;

        private float[] home_predicted_consumption;
        private float[] building_predicted_consumption;
        private float[] block_predicted_consumption;

        private DateTime last_call_of_Predict;

        //private object locker;

        public ConsumptionPreparation()
        {
            //LoadTrainedModels();

            last_call_of_Predict = DateTime.MinValue;

            //locker = new object();
        }


        public float CalculateEnergyDemand(DateTime startingPoint, ulong forecastedMinutes, IWeatherForecastStorage weatherForecastStorage, ConsumptionForecastStorage consumptionForecastStorage)
        {
            Logger.Instance.Log($"Consumption Forecast STARTED ...");

            if (last_call_of_Predict.AddHours(1) < DateTime.Now)
            {
                LoadTrainedModels();


                string[] training_temperature = File.ReadAllLines(@"../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/Za_Dunju_Temp_2.csv");
                float[] temperature = new float[training_temperature.Length - 1];

                string line_temperature;
                string[] hourly_data = new string[2];

                for (int i = 1; i < training_temperature.Length; i++)
                {
                    line_temperature = training_temperature[i];
                    hourly_data = line_temperature.Split(',');
                    temperature[i - 1] = float.Parse(hourly_data[1]);
                }


                #region scaling into range [0,1]

                float temperature_min = temperature[0];
                float temperature_max = temperature[0];

                for (int i = 1; i < temperature.Length; i++)
                {
                    if (temperature[i] < temperature_min)
                    {
                        temperature_min = temperature[i];
                    }

                    if (temperature[i] > temperature_max)
                    {
                        temperature_max = temperature[i];
                    }
                }

                List<WeatherDataInfo> wdi = weatherForecastStorage.GetHourlyWeatherInfo(24);
                float[] forecast_temperatures = new float[24];
                for (int i = 0; i < 24; i++)
                {
                    forecast_temperatures[i] = wdi[i].TemperatureC;
                }

                float[] forecast_temperatures_Scaled = ScaleTemperature(forecast_temperatures, temperature_min, temperature_max);
                float dayOfWeek_Scaled = ScaleDayOfWeek(startingPoint.DayOfWeek);

                #endregion


                #region prepare data for PREDICT

                NDarray x_for_predict = np.array(new float[1, 25]);

                for (int i = 0; i < forecast_temperatures_Scaled.Length; i++)
                {
                    x_for_predict[0, i] = (NDarray)forecast_temperatures_Scaled[i];
                    //Logger.Instance.Log($"---------->  PrepareDataForPredict:  i = {i}");
                }

                x_for_predict[0, 24] = (NDarray)dayOfWeek_Scaled;

                #endregion


                home_predicted_consumption = PredictConsumption_Home(x_for_predict);
                building_predicted_consumption = PredictConsumption_Building(x_for_predict);
                block_predicted_consumption = PredictConsumption_Block(x_for_predict);

                last_call_of_Predict = DateTime.Now;
            }


            // getting Consumers for their 'pfixed'
            List<Consumer> all_consumers = consumptionForecastStorage.GetAllEntities();

            float consumption_sum = 0;

            int hours = (int)forecastedMinutes / 60;
            float remainder = forecastedMinutes % 60;
            float part_of_hour;

            for (int i = 0; i < hours; i++)
            {
                for (int j = 0; j < all_consumers.Capacity; j++)
                {
                    switch (all_consumers[j].Type)
                    {
                        case Common.AbstractModel.ConsumerType.Home:
                            consumption_sum += all_consumers[j].Pfixed * home_predicted_consumption[i];
                            Logger.Instance.Log($"---------->   {all_consumers[j].Type} :  {all_consumers[j].Pfixed * home_predicted_consumption[i]} W");
                            break;
                        case Common.AbstractModel.ConsumerType.Building:
                            consumption_sum += all_consumers[j].Pfixed * building_predicted_consumption[i];
                            Logger.Instance.Log($"---------->   {all_consumers[j].Type} :  {all_consumers[j].Pfixed * building_predicted_consumption[i]} W");
                            break;
                        case Common.AbstractModel.ConsumerType.Block:
                            consumption_sum += all_consumers[j].Pfixed * block_predicted_consumption[i];
                            Logger.Instance.Log($"---------->   {all_consumers[j].Type} :  {all_consumers[j].Pfixed * block_predicted_consumption[i]} W");
                            break;
                        default:
                            consumption_sum += 0;
                            break;
                    }
                }
            }

            if (remainder != 0)
            {
                part_of_hour = remainder / 60;

                for (int j = 0; j < all_consumers.Capacity; j++)
                {
                    switch (all_consumers[j].Type)
                    {
                        case Common.AbstractModel.ConsumerType.Home:
                            consumption_sum += all_consumers[j].Pfixed * home_predicted_consumption[hours] * part_of_hour;
                            Logger.Instance.Log($"---------->   {all_consumers[j].Type} :  {all_consumers[j].Pfixed * home_predicted_consumption[hours] * part_of_hour} W");
                            break;
                        case Common.AbstractModel.ConsumerType.Building:
                            consumption_sum += all_consumers[j].Pfixed * building_predicted_consumption[hours] * part_of_hour;
                            Logger.Instance.Log($"---------->   {all_consumers[j].Type} :  {all_consumers[j].Pfixed * building_predicted_consumption[hours] * part_of_hour} W");
                            break;
                        case Common.AbstractModel.ConsumerType.Block:
                            consumption_sum += all_consumers[j].Pfixed * block_predicted_consumption[hours] * part_of_hour;
                            Logger.Instance.Log($"---------->   {all_consumers[j].Type} :  {all_consumers[j].Pfixed * block_predicted_consumption[hours] * part_of_hour} W");
                            break;
                        default:
                            consumption_sum += 0;
                            break;
                    }
                }
            }


            Logger.Instance.Log($"Consumption Forecast ENDED.");

            // result: W -> kW
            return (consumption_sum / 1000);
        }



        private void LoadTrainedModels()
        {
            Logger.Instance.Log($"---> Loading model_Home");

            model_Home = Sequential.ModelFromJson(File.ReadAllText("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_Household.json"));
            model_Home.LoadWeight("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_weight_Household.h5");


            Logger.Instance.Log($"---> Loading model_Building");

            model_Building = Sequential.ModelFromJson(File.ReadAllText("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_Building.json"));
            model_Building.LoadWeight("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_weight_Building.h5");


            Logger.Instance.Log($"---> Loading model_Block");

            model_Block = Sequential.ModelFromJson(File.ReadAllText("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_Block.json"));
            model_Block.LoadWeight("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_weight_Block.h5");

        }




        private float[] ScaleTemperature(float[] forecast_temperatures, float temperature_min, float temperature_max)
        {
            float[] temperature_Scaled = new float[forecast_temperatures.Length];

            for (int i = 0; i < forecast_temperatures.Length; i++)
            {
                temperature_Scaled[i] = (forecast_temperatures[i] - temperature_min) / (temperature_max - temperature_min);
            }

            return temperature_Scaled;
        }

        private float ScaleDayOfWeek(DayOfWeek dayOfWeek)
        {
            float dayOfWeek_f, dayOfWeek_Scaled;

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    dayOfWeek_f = 0;
                    break;
                case DayOfWeek.Tuesday:
                    dayOfWeek_f = 1;
                    break;
                case DayOfWeek.Wednesday:
                    dayOfWeek_f = 2;
                    break;
                case DayOfWeek.Thursday:
                    dayOfWeek_f = 3;
                    break;
                case DayOfWeek.Friday:
                    dayOfWeek_f = 4;
                    break;
                case DayOfWeek.Saturday:
                    dayOfWeek_f = 5;
                    break;
                case DayOfWeek.Sunday:
                    dayOfWeek_f = 6;
                    break;
                default:
                    Console.WriteLine("ERROR: dayOfWeek !");
                    dayOfWeek_f = 0;
                    break;
            }

            dayOfWeek_Scaled = (dayOfWeek_f - 0) / (6 - 0);

            return dayOfWeek_Scaled;
        }




        private float[] PredictConsumption_Home(NDarray x_for_predict)
        {
            float[] consumption = GetConsumptionFromFile_Home();

            float consumption_min = consumption[0];
            float consumption_max = consumption[0];

            for (int i = 1; i < consumption.Length; i++)
            {
                if (consumption[i] < consumption_min)
                {
                    consumption_min = consumption[i];
                }

                if (consumption[i] > consumption_max)
                {
                    consumption_max = consumption[i];
                }
            }


            //Logger.Instance.Log($"---> Loading model_Home");

            //var model_Home = Sequential.ModelFromJson(File.ReadAllText("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_Household.json"));
            //model_Home.LoadWeight("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_weight_Household.h5");

            Logger.Instance.Log($"---> Home Predict started ...");

            var consumption_Home_Scaled = model_Home.Predict(x_for_predict);

            Logger.Instance.Log($"---> Home Predict ENDED.");


            //DEscaling
            float[] consumption_Home = new float[24];

            // y_scaled * (in_max - in_min) + in_min
            for (int i = 0; i < 24; i++)
            {
                consumption_Home[i] = (float)consumption_Home_Scaled[0][i] * (consumption_max - consumption_min) + consumption_min;
            }

            return consumption_Home;
        }

        private float[] PredictConsumption_Building(NDarray x_for_predict)
        {
            float[] consumption = GetConsumptionFromFile_Building();

            float consumption_min = consumption[0];
            float consumption_max = consumption[0];

            for (int i = 1; i < consumption.Length; i++)
            {
                if (consumption[i] < consumption_min)
                {
                    consumption_min = consumption[i];
                }

                if (consumption[i] > consumption_max)
                {
                    consumption_max = consumption[i];
                }
            }


            //Logger.Instance.Log($"---> Loading model_Building");

            //var model_Building = Sequential.ModelFromJson(File.ReadAllText("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_Building.json"));
            //model_Building.LoadWeight("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_weight_Building.h5");

            Logger.Instance.Log($"---> Building Predict started ...");

            var consumption_Building_Scaled = model_Building.Predict(x_for_predict);

            Logger.Instance.Log($"---> Building Predict ENDED.");


            //DEscaling
            float[] consumption_Building = new float[24];

            // y_scaled * (in_max - in_min) + in_min
            for (int i = 0; i < 24; i++)
            {
                consumption_Building[i] = (float)consumption_Building_Scaled[0][i] * (consumption_max - consumption_min) + consumption_min;
            }

            return consumption_Building;
        }

        private float[] PredictConsumption_Block(NDarray x_for_predict)
        {
            float[] consumption = GetConsumptionFromFile_Block();

            float consumption_min = consumption[0];
            float consumption_max = consumption[0];

            for (int i = 1; i < consumption.Length; i++)
            {
                if (consumption[i] < consumption_min)
                {
                    consumption_min = consumption[i];
                }

                if (consumption[i] > consumption_max)
                {
                    consumption_max = consumption[i];
                }
            }


            //Logger.Instance.Log($"---> Loading model_Block");

            //var model_Block = Sequential.ModelFromJson(File.ReadAllText("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_Block.json"));
            //model_Block.LoadWeight("../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/model_weight_Block.h5");


            Logger.Instance.Log($"---> Block Predict started ...");

            var consumption_Block_Scaled = model_Block.Predict(x_for_predict);

            Logger.Instance.Log($"---> Block Predict ENDED.");


            //DEscaling
            float[] consumption_Block = new float[24];

            // y_scaled * (in_max - in_min) + in_min
            for (int i = 0; i < 24; i++)
            {
                consumption_Block[i] = (float)consumption_Block_Scaled[0][i] * (consumption_max - consumption_min) + consumption_min;
            }

            return consumption_Block;
        }




        private float[] GetConsumptionFromFile_Home()
        {
            string[] training_consumption = System.IO.File.ReadAllLines(@"../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/household_load.csv");
            float[] consumption = new float[training_consumption.Length - 1];

            string line_consumption;
            string[] hourly_data = new string[2];

            for (int i = 1; i < training_consumption.Length; i++)
            {
                line_consumption = training_consumption[i];
                hourly_data = line_consumption.Split(',');
                consumption[i - 1] = float.Parse(hourly_data[1]);
            }

            return consumption;
        }

        private float[] GetConsumptionFromFile_Building()
        {
            string[] training_consumption = System.IO.File.ReadAllLines(@"../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/building_load.csv");
            float[] consumption = new float[training_consumption.Length - 1];

            string line_consumption;
            string[] hourly_data = new string[2];

            for (int i = 1; i < training_consumption.Length; i++)
            {
                line_consumption = training_consumption[i];
                hourly_data = line_consumption.Split(',');
                consumption[i - 1] = float.Parse(hourly_data[1]);
            }

            return consumption;
        }

        private float[] GetConsumptionFromFile_Block()
        {
            string[] training_consumption = System.IO.File.ReadAllLines(@"../../Commanding/BalanceForecastCommanding/DataPreparation/Consumption/block_load.csv");
            float[] consumption = new float[training_consumption.Length - 1];

            string line_consumption;
            string[] hourly_data = new string[2];

            for (int i = 1; i < training_consumption.Length; i++)
            {
                line_consumption = training_consumption[i];
                hourly_data = line_consumption.Split(',');
                consumption[i - 1] = float.Parse(hourly_data[1]);
            }

            return consumption;
        }




    }
}
