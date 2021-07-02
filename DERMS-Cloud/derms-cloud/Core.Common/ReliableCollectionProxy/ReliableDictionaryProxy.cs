using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace Core.Common.ReliableCollectionProxy
{
    public static class ReliableDictionaryProxy
    {
        public static void CreateDictionary<EntityType, KeyType>(IReliableStateManager stateManager, string dictionaryName)
            where KeyType : IComparable<KeyType>, IEquatable<KeyType>

        {
            var dictionary = stateManager.GetOrAddAsync<IReliableDictionary<KeyType, EntityType>>(dictionaryName).GetAwaiter().GetResult();

            using (var tx = stateManager.CreateTransaction())
            {
                dictionary.ClearAsync(TimeSpan.FromSeconds(60), CancellationToken.None).GetAwaiter().GetResult();

                tx.CommitAsync().GetAwaiter().GetResult();
            }
        }

        public static bool EntityExists<EntityType, KeyType>(IReliableStateManager stateManager, KeyType key, string dictionaryName)
            where KeyType : IComparable<KeyType>, IEquatable<KeyType>

        {
            var dictionary = stateManager.GetOrAddAsync<IReliableDictionary<KeyType, EntityType>>(dictionaryName).GetAwaiter().GetResult();

            using (var tx = stateManager.CreateTransaction())
            {
                return dictionary.ContainsKeyAsync(tx, key, TimeSpan.FromSeconds(60), CancellationToken.None).GetAwaiter().GetResult();
            }
        }

        public static bool AddOrUpdateEntity<EntityType, KeyType>(IReliableStateManager stateManager, EntityType entity, KeyType key, string dictionaryName)
            where KeyType : IComparable<KeyType>, IEquatable<KeyType>

        {
            var dictionary = stateManager.GetOrAddAsync<IReliableDictionary<KeyType, EntityType>>(dictionaryName).GetAwaiter().GetResult();

            using (var tx = stateManager.CreateTransaction())
            {
                dictionary.AddOrUpdateAsync(tx, key, entity, (entityKey, value) => value).GetAwaiter().GetResult();

                tx.CommitAsync().GetAwaiter().GetResult();

                return true;
            }
        }

        public static EntityType GetEntity<EntityType, KeyType>(IReliableStateManager stateManager, KeyType key, string dictionaryName)
            where KeyType : IComparable<KeyType>, IEquatable<KeyType>
        {
            var dictionary = stateManager.GetOrAddAsync<IReliableDictionary<KeyType, EntityType>>(dictionaryName).GetAwaiter().GetResult();

            using (var tx = stateManager.CreateTransaction())
            {
                var conditionVariable = dictionary.TryGetValueAsync(tx, key).GetAwaiter().GetResult();
                if (conditionVariable.HasValue)
                {
                    return conditionVariable.Value;
                }

                return default(EntityType);
            }
        }

        public static List<EntityType> GetAllEntities<EntityType, KeyType>(IReliableStateManager stateManager, string dictionaryName)
            where KeyType : IComparable<KeyType>, IEquatable<KeyType>
        {
            var dictionary = stateManager.GetOrAddAsync<IReliableDictionary<KeyType, EntityType>>(dictionaryName).GetAwaiter().GetResult();

            using (var tx = stateManager.CreateTransaction())
            {
                List<EntityType> entities = new List<EntityType>();

                foreach (var item in AsyncEnumeratorHandler.Enumerate(dictionary.CreateEnumerableAsync(tx).GetAwaiter().GetResult()))
                {
                    entities.Add(item.Value);
                }

                return entities;
            }
        }

        public static IEnumerable<EntityType> Iterate<EntityType, KeyType>(IReliableStateManager stateManager, string dictionaryName)
            where KeyType : IComparable<KeyType>, IEquatable<KeyType>
        {
            var dictionary = stateManager.GetOrAddAsync<IReliableDictionary<KeyType, EntityType>>(dictionaryName).GetAwaiter().GetResult();

            using (var tx = stateManager.CreateTransaction())
            {
                foreach (var item in AsyncEnumeratorHandler.Enumerate(dictionary.CreateEnumerableAsync(tx).GetAwaiter().GetResult()))
                {
                    yield return item.Value;
                }
            }
        }

        public static void CopyDictionary<EntityType, KeyType>(IReliableStateManager stateManager, string dictionarySource, string dictionaryDestination)
            where KeyType : IComparable<KeyType>, IEquatable<KeyType>
        {
            var source = stateManager.GetOrAddAsync<IReliableDictionary<KeyType, EntityType>>(dictionarySource).GetAwaiter().GetResult();
            CreateDictionary<EntityType, KeyType>(stateManager, dictionaryDestination);
            var destination = stateManager.GetOrAddAsync<IReliableDictionary<KeyType, EntityType>>(dictionaryDestination).GetAwaiter().GetResult();

            using (var tx = stateManager.CreateTransaction())
            {
                foreach (var entity in AsyncEnumeratorHandler.Enumerate(source.CreateEnumerableAsync(tx).GetAwaiter().GetResult()))
                {
                    destination.AddAsync(tx, entity.Key, entity.Value).GetAwaiter().GetResult();
                }

                tx.CommitAsync().GetAwaiter().GetResult();
            }
        }
    }
}
