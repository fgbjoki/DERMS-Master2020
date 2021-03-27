using Common.Communication;
using Common.DataTransferObjects.CalculationEngine;
using Common.Logger;
using Common.ServiceInterfaces.CalculationEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UIAdapter.Schema
{
    public class SchemaAligner : IDisposable
    {
        private AutoResetEvent alignSchemas;

        private CancellationTokenSource cancellationTokenSource;

        private GraphSchemaController schemaController;
        private WCFClient<ISchemaRepresentation> schemaRepresentationClient;

        public SchemaAligner(GraphSchemaController schemaController, AutoResetEvent alignSchemas)
        {
            this.schemaController = schemaController;
            this.alignSchemas = alignSchemas;

            schemaRepresentationClient = new WCFClient<ISchemaRepresentation>("ceSchema");

            cancellationTokenSource = new CancellationTokenSource();

            Thread alignWorker = new Thread(() => AlignSchemas(cancellationTokenSource.Token));
            alignWorker.Start();
        }

        private void AlignSchemas(CancellationToken cancellationToken)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                alignSchemas.WaitOne();

                if (cancellationTokenSource.IsCancellationRequested)
                {
                    continue;
                }

                Logger.Instance.Log($"[{GetType().Name}] Schema aligning process started.");

                try
                {
                    List<long> existingSchemaSources = schemaRepresentationClient.Proxy.GetSchemaSources().ToList();

                    List<long> schemaSources = schemaController.GetSchemaSources();

                    List<long> neededSchemaSources = existingSchemaSources.Except(schemaSources).ToList();

                    List<SchemaGraphChanged> changedSchemas = GetChangedSchemas(neededSchemaSources);

                    foreach (var changedSchema in changedSchemas)
                    {
                        schemaController.AddNewSchema(changedSchema);

                        Logger.Instance.Log($"[{GetType().Name}] Added new schema with source id: {changedSchema.EnergySourceGid:X16}.");
                    }

                }
                catch (Exception e)
                {
                    Logger.Instance.Log($"[{GetType().Name}] Couldn't align schema sources. More info: {e.Message}, stack trace: {e.StackTrace}");
                }
                finally
                {
                    Logger.Instance.Log($"[{GetType().Name}] Schema aligning process finished.");
                }
            }
        }

        private List<SchemaGraphChanged> GetChangedSchemas(List<long> neededSchemaSources)
        {
            List<SchemaGraphChanged> changedSchemas = new List<SchemaGraphChanged>();

            try
            {
                foreach (var neededSchemaSource in neededSchemaSources)
                {
                    SchemaGraphChanged changedSchema = schemaRepresentationClient.Proxy.GetSchema(neededSchemaSource);

                    changedSchemas.Add(changedSchema);
                }
            }
            catch
            {
                throw;
            }

            return changedSchemas;
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
