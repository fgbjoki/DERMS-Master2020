using CIM.Model;
using FTN.ESI.SIMES.CIM.CIMAdapter;
using System.IO;
using System.Reflection;
using System;

namespace FieldSimulator.PowerSimulator.SchemaLoader
{
    public class SchemaCIMLoader : ISchemaLoader
    {
        private CIMAdapter cimAdapter;

        public SchemaCIMLoader()
        {
            cimAdapter = new CIMAdapter();
        }

        public ConcreteModel LoadSchema(string xmlFilePath)
        {
            ConcreteModel concreteModel = null;
            Assembly assembly = null;
            string log;

            using (FileStream fileStream = File.Open(xmlFilePath, FileMode.Open))
            {
                if (!cimAdapter.LoadModelFromExtractFile(fileStream, FTN.ESI.SIMES.CIM.CIMAdapter.Manager.SupportedProfiles.DERMS, ref concreteModel, ref assembly, out log))
                {
                    return null;
                }

            }

            return concreteModel;
        }
    }
}
