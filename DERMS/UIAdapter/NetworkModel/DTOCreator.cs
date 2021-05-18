using Common.AbstractModel;
using Common.GDA;
using Common.UIDataTransferObject.NetworkModel;
using System.Collections.Generic;

namespace UIAdapter.NetworkModel
{
    public abstract class DTOCreator<T> : IDTOCreator
        where T : NetworkModelEntityDTO
    {
        private List<ModelCode> neededModelCodes;

        public DTOCreator()
        {
            neededModelCodes = GetModelCodes();
        }

        public NetworkModelEntityDTO CreateEntityDTO(ResourceDescription rd)
        {
            T entity = InstantiateEntity();
            PopulateProperties(entity, rd);

            return entity;
        }
        
        public List<ModelCode> NeededModelCodes { get { return neededModelCodes; } }

        public void ConnectDependentDTO(NetworkModelEntityDTO entity, NetworkModelEntityDTO depedencyDTO)
        {
            ConnectDependentDTO(entity as T, depedencyDTO);
        }

        protected virtual void ConnectDependentDTO(T entity, NetworkModelEntityDTO depedencyDTO)
        {

        }

        protected abstract T InstantiateEntity();

        protected virtual void PopulateProperties(T dto, ResourceDescription rd)
        {
            dto.GlobalId = rd.Id;
            dto.Name = rd.GetProperty(ModelCode.IDOBJ_NAME).AsString();
            dto.Description = rd.GetProperty(ModelCode.IDOBJ_DESCRIPTION).AsString();
        }

        protected virtual List<ModelCode> GetModelCodes()
        {
            return new List<ModelCode>()
            {
                ModelCode.IDOBJ_NAME,
                ModelCode.IDOBJ_DESCRIPTION
            };
        }

        public virtual List<long> GetDependentEntities(ResourceDescription rd)
        {
            return new List<long>();
        }
    }
}
