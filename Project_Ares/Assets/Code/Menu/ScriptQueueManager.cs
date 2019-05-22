using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PPBC {
    public class ScriptQueueManager {

        class d_SubItem {//https://stackoverflow.com/questions/16679033/cannot-modify-struct-in-a-list?rq=1
            public bool finished;
            public IScriptQueueItem item;

            public d_SubItem(IScriptQueueItem reference) {
                finished = false;
                item = reference;
            }
        }

        List<Tuple<int,List<d_SubItem>>> m_queue = new List<Tuple<int, List<d_SubItem>>>();

        #region Variables

        int m_currentIndex = 0;

        #endregion

        /// <summary>
        /// executes all ticks of all active IScriptQueueItems
        /// </summary>
        /// <returns>true if the queue has finished</returns>
        public bool Tick() {
            if(m_queue.Count == 0)
                return true;

            List<d_SubItem> activeQueue = null;
            activeQueue = m_queue[m_currentIndex].Item2.FindAll(e => e.finished == false);

            if(activeQueue == null) {
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
                Tuple<int, List<d_SubItem>> newItem = new Tuple<int, List<d_SubItem>>(sortingLayer, new List<d_SubItem>());
                newItem.Item2.Add(newSubItem);
                m_queue.Add(newItem);
                return;
            }

            for (int i = 0; i < m_queue.Count; i++) {
                if(m_queue[i].Item1 > sortingLayer) {
                    Tuple<int, List<d_SubItem>> newItem = new Tuple<int, List<d_SubItem>>(sortingLayer, new List<d_SubItem>());
                    newItem.Item2.Add(newSubItem);
                    m_queue.Insert(i, newItem);
                    return;
                }
                if(m_queue[i].Item1 == sortingLayer) {
                    m_queue[i].Item2.Add(newSubItem);
                    return;
                }
            }
        }
    }
}