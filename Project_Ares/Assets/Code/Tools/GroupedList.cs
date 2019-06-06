using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class GroupedList<T> {

        List<T> m_list = new List<T>();
        List<int> m_groupingIndex = new List<int>();
        
        /// <summary>
        /// how many Objects are registered
        /// </summary>
        public int Count {
            get {
                return m_list.Count;
            }
        }

        /// <summary>
        /// how many teams do exist
        /// </summary>
        public int Groups {
            get {
                return m_groupingIndex.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">the Group</param>
        /// <returns>a list of all objects in this team</returns>
        public List<T> this[int index] {
            get {
                if(index >= m_groupingIndex.Count) {
                    return null;
                }
                if (index < m_groupingIndex.Count - 1) {
                    return m_list.GetRange(m_groupingIndex[index], m_groupingIndex[index + 1] - m_groupingIndex[index]);
                } else {
                    return m_list.GetRange(m_groupingIndex[index], m_list.Count - m_groupingIndex[index]);
                }
            }
            set {
                if(index >= m_groupingIndex.Count) {
                    m_groupingIndex.Add(m_list.Count);
                } else {
                    for (int i = index + 1; i < m_groupingIndex.Count; i++) {
                        m_groupingIndex[i] += value.Count;
                    }
                }
                m_list.InsertRange(m_groupingIndex[index], value);
            }
        }

        /// <summary>
        /// try to use intiger index
        /// </summary>
        /// <param name="index">the indexed item</param>
        /// <returns>the Group</returns>
        public int this[T index] {
            get {
                int listIndex = m_list.FindIndex(x => x.Equals(index));
                if(listIndex < 0) {
                    return -1;
                }
                return m_groupingIndex.FindIndex(x => x > listIndex)-1;//error by last index
            }
            set {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">the index of a groupe</param>
        /// <returns>the size of the Group</returns>
        public int GetGroupSize(int index) {
            if(index < m_groupingIndex.Count - 1) {
                return m_groupingIndex[index + 1] - m_groupingIndex[index];
            } else {
                return m_list.Count - m_groupingIndex[index];
            }
        }
    }
}