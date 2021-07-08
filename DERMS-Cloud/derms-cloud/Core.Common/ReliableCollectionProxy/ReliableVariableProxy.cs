using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Core.Common.ReliableCollectionProxy
{
    public static class ReliableVariableProxy
    {
        public static T GetVariable<T>(IReliableStateManager stateManager, string variableName)
        {
            var reliableInstance = stateManager.GetOrAddAsync<IReliableDictionary<string, T>>(variableName).GetAwaiter().GetResult();

            using (var tx = stateManager.CreateTransaction())
            {
                var transactionObject = reliableInstance.TryGetValueAsync(tx, variableName).GetAwaiter().GetResult();
                if (!transactionObject.HasValue)
                {                  
                    return default(T);
                }

                return transactionObject.Value;
            }
        }

        public static T GetVariable<T>(IReliableStateManager stateManager, string variableName, ITransaction tx)
        {
            var reliableInstance = stateManager.GetOrAddAsync<IReliableDictionary<string, T>>(variableName).GetAwaiter().GetResult();
            var transactionObject = reliableInstance.TryGetValueAsync(tx, variableName).GetAwaiter().GetResult();
            if (!transactionObject.HasValue)
            {
                return default(T);
            }

            return transactionObject.Value;
        }

        public static void SetVariable<T>(IReliableStateManager stateManager, T variable, string variableName)
        {
            var reliableCollection = stateManager.GetOrAddAsync<IReliableDictionary<string, T>>(variableName).GetAwaiter().GetResult();

            using (var tx = stateManager.CreateTransaction())
            {
                reliableCollection.SetAsync(tx, variableName, variable).GetAwaiter().GetResult();

                tx.CommitAsync().GetAwaiter().GetResult();
            }
        }

        public static void SetVariable<T>(IReliableStateManager stateManager, T variable, string variableName, ITransaction tx)
        {
            var reliableCollection = stateManager.GetOrAddAsync<IReliableDictionary<string, T>>(variableName).GetAwaiter().GetResult();

            reliableCollection.SetAsync(tx, variableName, variable).GetAwaiter().GetResult();
        }

        public static async void AddVariable<T>(IReliableStateManager stateManager, T variable, string variableName)
        {
            var reliableCollection = await stateManager.GetOrAddAsync<IReliableDictionary<string, T>>(variableName);

            using (var tx = stateManager.CreateTransaction())
            {
                if (! await reliableCollection.ContainsKeyAsync(tx, variableName))
                {
                    await reliableCollection.AddAsync(tx, variableName, variable);
                }

                await tx.CommitAsync();
            }
        }
    }
}
