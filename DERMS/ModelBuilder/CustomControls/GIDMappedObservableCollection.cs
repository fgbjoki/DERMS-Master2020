using ClientUI.Models;
using Common.UIDataTransferObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientUI.CustomControls
{
    public class GIDMappedObservableCollection<T> : ObservableCollection<T>
        where T : IdentifiedObject
    {
        private Dictionary<long, int> gidToIndexMap;

        public GIDMappedObservableCollection() : base()
        {
            gidToIndexMap = new Dictionary<long, int>();
        }

        public GIDMappedObservableCollection(IEnumerable<T> collection) : base(collection)
        {
            gidToIndexMap = new Dictionary<long, int>();
        }

        protected override void InsertItem(int index, T item)
        {
            if (gidToIndexMap.ContainsKey(item.GlobalId))
            {
                throw new ArgumentException($"Entity with gid {item.GlobalId:8X} already exists!");
            }

            base.InsertItem(index, item);

            gidToIndexMap.Add(item.GlobalId, index);
        }

        public void AddOrUpdateEntity(T item)
        {
            int index;
            if (!gidToIndexMap.TryGetValue(item.GlobalId, out index))
            {
                base.Add(item);
            }
            else
            {
                Items[index] = item;
            }
        }
    }
}
