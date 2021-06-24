using Common.Communication;
using Common.ComponentStorage;
using Common.DataTransferObjects.CalculationEngine.EnergyBalanceForecast;
using Common.Logger;
using Common.ServiceInterfaces.CalculationEngine;
using Common.UIDataTransferObject.EnergyBalanceForecast;
using System;
using System.Collections.Generic;
using System.Threading;
using UIAdapter.Model.NetworkModel;

namespace UIAdapter.Forecast.EnergyBalanceForecast
{
    public class EnergyBalanceForecastProxy : Common.ServiceInterfaces.UIAdapter.IEnergyBalanceForecast
    {
        private int id = 0;
        private ReaderWriterLockSlim locker;
        private WCFClient<IEnergyBalanceForecast> energyBalanceForecast;
        private Dictionary<int, DERStatesSuggestionDTO> requests;

        private DataConverter dataConverter;

        private IStorage<NetworkModelItem> storage;

        public EnergyBalanceForecastProxy(IStorage<NetworkModelItem> storage)
        {
            locker = new ReaderWriterLockSlim();
            dataConverter = new DataConverter();
            energyBalanceForecast = new WCFClient<IEnergyBalanceForecast>("ceEnergyBalanceForecast");

            requests = new Dictionary<int, DERStatesSuggestionDTO>();

            this.storage = storage;
        }

        public int Compute(DomainParametersDTO domainParameters)
        {
            int clientId;
            locker.EnterWriteLock();
            clientId = id;
            locker.ExitWriteLock();

            Interlocked.Increment(ref id);

            try
            {
                object parameters = new object[2] { clientId, domainParameters };

                ThreadPool.QueueUserWorkItem(Computing, parameters);
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't start processing. More info:\n{e.Message}\nStack trace:\n{e.StackTrace}");
                return -1;
            }

            return clientId;
        }

        public DERStatesSuggestionDTO GetResults(int clientId)
        {
            locker.EnterReadLock();

            DERStatesSuggestionDTO result;
            requests.TryGetValue(clientId, out result);

            locker.ExitReadLock();

            if (result != null)
            {
                locker.EnterWriteLock();

                requests.Remove(clientId);

                locker.ExitWriteLock();
            }

            return result;
        }

        private void Computing(object parameter)
        {
            int clientId = (int)(((object[])parameter)[0]);
            DomainParametersDTO domainParameter = (DomainParametersDTO)(((object[])parameter)[1]);
            DERStatesSuggestionDTO result = new DERStatesSuggestionDTO();

            try
            {
                DERStateCommandingSequenceDTO request = energyBalanceForecast.Proxy.Compute(domainParameter);
                result = ConvertResult(request, domainParameter);
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Couldn't calculate energy balance forecast for client id: {clientId}. More info:\n{e.Message}\nStackTrace:\n{e.StackTrace}");
                result.Error = true;
            }

            locker.EnterWriteLock();

            requests.Add(clientId, result);

            locker.ExitWriteLock();
        }

        private DERStatesSuggestionDTO ConvertResult(DERStateCommandingSequenceDTO request, DomainParametersDTO domainParameters)
        {
            return dataConverter.ConvertData(request, domainParameters, storage);
        }
    }
}
