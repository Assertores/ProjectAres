using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PPBC {
    [Serializable]
    public class ScriptQueueManager {
        [Serializable]
        class d_SubItem {//https://stackoverflow.com/questions/16679033/cannot-modify-struct-in-a-list?rq=1
            public bool finished;
            public IScriptQueueItem item;

            public d_SubItem(IScriptQueueItem reference) {
                finished = false;
                item = reference;
            }
        }
        [Serializable]
        class d_Item {
            public int Item1;
            public List<d_SubItem> Item2 = new List<d_SubItem>();

            public d_Item(int i) {
                Item1 = i;
            }
        }

        [SerializeField] List<d_Item> m_queue = new List<d_Item>();

        #region Variables

        int m_currentIndex = 0;
        bool m_firstTick = false;

        #endregion

        /// <summary>
        /// executes all ticks of all active IScriptQueueItems
        /// </summary>
        /// <returns>true if the queue has finished</returns>
        public bool Tick() {
            if(m_queue.Count == 0)
                return true;

            if (!m_firstTick) {
                foreach (var it in m_queue[m_currentIndex].Item2) {
                    it.finished = it.item.FirstTick();
                }
                m_firstTick = true;
            }

            List<d_SubItem> activeQueue = null;
            activeQueue = m_queue[m_currentIndex].Item2.FindAll(e => e.finished == false);

            if(activeQueue.Count == 0) {
                m_currentIndex++;
                if (m_currentIndex >= m_queue.Count) {
                    m_queue.Clear();
                    m_currentIndex = 0;
                    return true;
                }

                foreach (var it in m_queue[m_currentIndex].Item2) {
                    it.finished = it.item.FirstTick();
                }

                return false;
            }

            foreach(var it in activeQueue) {
                it.finished = it.item.DoTick();
            }

            return false;
        }

        /// <summary>
        /// adds items to the queue
        /// </summary>
        /// <param name="item">the reference to the item that should be added</param>
        /// <param name="sortingLayer">the execute order value. bigger meanse laiter</param>
        public void AddItemToQueue(IScriptQueueItem item, int sortingLayer) {
            d_SubItem newSubItem = new d_SubItem(item);

            if(m_queue.Count <= 0 || sortingLayer > m_queue[m_queue.Count - 1].Item1) {
                d_Item newItem = new d_Item(sortingLayer);
                newItem.Item2.Add(newSubItem);
                m_queue.Add(newItem);
                return;
            }

            for (int i = 0; i < m_queue.Count; i++) {
                if(m_queue[i].Item1 > sortingLayer) {
                    d_Item newItem = new d_Item(sortingLayer);
                    newItem.Item2.Add(newSubItem);
                    m_queue.Insert(i, newItem);
                    return;
                }
                if(m_queue[i].Item1 == sortingLayer) {
                    m_queue[i].Item2.Add(newSubItem);
                    return;
                }
            }

            Debug.Log("something whent wrong");
        }
    }
}