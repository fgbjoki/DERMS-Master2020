using CIM.Model;
using Common.AbstractModel;

namespace FieldSimulator.PowerSimulator.Model.Creators
{
    public interface IEntityCreator
    {
        void CreateNewEntities(ConcreteModel concreteModel, EntityStorage entityStorage);

        DMSType DMSType { get; }
    }
}