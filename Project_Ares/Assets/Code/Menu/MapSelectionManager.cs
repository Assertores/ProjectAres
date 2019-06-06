using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class MapSelectionManager : MonoBehaviour {

        [Header("References")]
        [SerializeField] GameObject m_itemPrefab;
        [SerializeField] RectTransform m_contentHolder;
        [Header("Balancing")]
        [SerializeField] float m_changeSpeed = 1;

        List<string> m_indexToMapName = new List<string>();
        int m_index = 0;
        bool m_finished = true;
        float m_itemWidth = -1;

        #region MonoBehaviour

        void Awake() {
            if (!m_contentHolder) {
                print("reference to content holder not set");
                Destroy(this);
                return;
            }
            if (!m_itemPrefab) {
                print("no item prefab");
                Destroy(this);
                return;
            }
            if (!m_itemPrefab.GetComponent<MapItemRefHolder>()) {
                print("item has no MapItemRefHolder");
                Destroy(this);
                return;
            }
        }

        void Start() {
            m_itemWidth = ((RectTransform)(m_itemPrefab.transform)).rect.width;

            MapItemRefHolder tmp;
            foreach (var it in DataHolder.s_maps) {
                tmp = Instantiate(m_itemPrefab, m_contentHolder).GetComponent<MapItemRefHolder>();
                tmp.transform.localPosition = new Vector3(m_indexToMapName.Count * m_itemWidth, 0, 0);
                tmp.transform.localRotation = Quaternion.Euler(Vector3.zero);
                m_contentHolder.sizeDelta += new Vector2(m_itemWidth, 0);

                tmp.m_mapName.text = it.Key;
                if (it.Value.m_size >= 0) {
                    tmp.m_mapSize.text = DataHolder.s_commonSize[it.Value.m_size].ToString();
                } else {
                    tmp.m_mapSize.text = it.Value.p_size[it.Value.m_size * -1 - 1].ToString();
                }
                //tmp.m_mapIcon = it.Value.m_icon;//TODO integrate preview pictures to maps
                m_indexToMapName.Add(it.Key);
            }
        }

        void Update() {
            

            if (m_finished) {
                return;
            }
            
            if (m_contentHolder.localPosition.x > -m_index * m_itemWidth) {
                m_contentHolder.localPosition -= new Vector3(m_changeSpeed, 0, 0);
                if (m_contentHolder.localPosition.x < -m_index * m_itemWidth) {
                    m_finished = true;
                    m_contentHolder.localPosition = new Vector3(-m_index * m_itemWidth, m_contentHolder.localPosition.y, m_contentHolder.localPosition.z);
                }
            } else {
                m_contentHolder.localPosition += new Vector3(m_changeSpeed, 0, 0);
                if (m_contentHolder.localPosition.x > -m_index * m_itemWidth) {
                    m_finished = true;
                    m_contentHolder.localPosition = new Vector3(-m_index * m_itemWidth, m_contentHolder.localPosition.y, m_contentHolder.localPosition.z);
                }
            }

            
        }

        #endregion

        public void NextMap() {
            m_index++;
            m_index %= m_indexToMapName.Count;

            DataHolder.s_map = m_indexToMapName[m_index];
            m_finished = false;
        }

        public void PreviousMap() {
            m_index--;
            if (m_index < 0)
                m_index += m_indexToMapName.Count;

            DataHolder.s_map = m_indexToMapName[m_index];
            m_finished = false;
        }
    }
}