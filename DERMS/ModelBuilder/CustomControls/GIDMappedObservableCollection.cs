﻿using ClientUI.Models;
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
        private Dictionary<int, long> indexToGidMap;

        public GIDMappedObservableCollection() : base()
        {
            gidToIndexMap = new Dictionary<long, int>();
            indexToGidMap = new Dictionary<int, long>();
        }

        public GIDMappedObservableCollection(IEnumerable<T> collection) : base(collection)
        {
            gidToIndexMap = new Dictionary<long, int>();
            indexToGidMap = new Dictionary<int, long>();
        }

        protected override void InsertItem(int index, T item)
        {
            if (gidToIndexMap.ContainsKey(item.GlobalId) || indexToGidMap.ContainsKey(index))
            {
                return;
            }

            base.InsertItem(index, item);

            gidToIndexMap.Add(item.GlobalId, index);
            indexToGidMap.Add(index, item.GlobalId);
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
                Items[index].Update(item);
            }
        }

        public bool Contains(long globalId)
        {
            return gidToIndexMap.ContainsKey(globalId);
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            gidToIndexMap.Clear();
            indexToGidMap.Clear();
        }
    }
}
