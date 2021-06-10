using CalculationEngine.Model.EnergyImporter;
using Common.Communication;
using Common.ComponentStorage;
using Common.Logger;
using Common.ServiceInterfaces.NetworkDynamicsService.Commands;
using System;

namespace CalculationEngine.EnergyCalculators
{
    public class EnergyImporterProcessor : IEnergyImporterProcessor
    {
        private IStorage<EnergySource> storage;
        private WCFClient<INDSCommanding> ndsCommanding;

        public EnergyImporterProcessor(IStorage<EnergySource> storage)
        {
            this.storage = storage;

            ndsCommanding = new WCFClient<INDSCommanding>("ndsCommanding");
        }

        public void ChangeSourceImportPower(long sourceGid, float activePower)
        {
            EnergySource energySource = storage.GetEntity(sourceGid);
            if (energySource == null)
            {
                Logger.Instance.Log($"[{GetType().Name}] Cannot find energy source with gid: 0x{sourceGid:X16}. Skipping commanding.");
                return;
            }

            try
            {
                Logger.Instance.Log($"[{GetType().Name}] Sending command for energy source(0x{energySource.GlobalId:X16}).");
                Logger.Instance.Log($"[{GetType().Name}] Analog gid: 0x{energySource.ActivePowerMeasurementGid:X16}, Value: {activePower:N2}");
                ndsCommanding.Proxy.SendCommand(new ChangeAnalogRemotePointValue(energySource.ActivePowerMeasurementGid, activePower));
                Logger.Instance.Log($"[{GetType().Name}] Command successfuly sent.");
            }
            catch (Exception e)
            {
                Logger.Instance.Log($"[{GetType().Name}] Command failed.\n{e.Message}\nStack trace:\n{e.StackTrace}");
            }
        }
    }
}
