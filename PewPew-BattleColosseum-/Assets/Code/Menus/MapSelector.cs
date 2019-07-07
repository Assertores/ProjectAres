using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class MapSelector : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject p_item;
        [SerializeField] RectTransform r_contentHolder;

        [Header("Balancing")]
        [SerializeField] float m_changeSpeed = 1;
        
        bool m_finished = true;
        float m_itemWidth = -1;

        #endregion
        #region MonoBehaviour

        void Awake() {
            if (!r_contentHolder) {
                print("reference to content holder not set");
                Destroy(this);
                return;
            }
            if (!p_item) {
                print("no item prefab");
                Destroy(this);
                return;
            }
            if (!p_item.GetComponent<SelectItemRefHolder>()) {
                print("item has no SelectItemRefHolder");
                Destroy(this);
                return;
            }
        }

        void Start() {
            m_itemWidth = ((RectTransform)(p_item.transform)).rect.width;

            for (int i = 0; i < DataHolder.s_maps.Count; i++) {
                SelectItemRefHolder element = Instantiate(p_item, r_contentHolder).GetComponent<SelectItemRefHolder>();
                element.transform.localPosition = new Vector3(i * m_itemWidth, 0, 0);
                element.transform.localRotation = Quaternion.Euler(Vector3.zero);

                r_contentHolder.sizeDelta += new Vector2(m_itemWidth, 0);

                element.r_name.text = DataHolder.s_maps[i].m_name;
                element.r_image.sprite = DataHolder.s_maps[i].m_icon;
                if(DataHolder.s_maps[i].m_size >= 0) {
                    element.r_text.text = DataHolder.s_commonSizes[DataHolder.s_maps[i].m_size].ToString();
                } else {
                    element.r_text.text = DataHolder.s_maps[i].p_sizes[DataHolder.s_maps[i].m_size * -1 - 1].ToString();
                }
            }
        }

        void Update() {
            if (m_finished) {
                return;
            }

            if (r_contentHolder.localPosition.x > -DataHolder.s_currentMap * m_itemWidth) {
                r_contentHolder.localPosition -= new Vector3(m_changeSpeed * Time.deltaTime, 0, 0);
                if (r_contentHolder.localPosition.x < -DataHolder.s_currentMap * m_itemWidth) {
                    m_finished = true;
                    r_contentHolder.localPosition = new Vector3(-DataHolder.s_currentMap * m_itemWidth, r_contentHolder.localPosition.y, r_contentHolder.localPosition.z);
                }
            } else {
                r_contentHolder.localPosition += new Vector3(m_changeSpeed * Time.deltaTime, 0, 0);
                if (r_contentHolder.localPosition.x > -DataHolder.s_currentMap * m_itemWidth) {
                    m_finished = true;
                    r_contentHolder.localPosition = new Vector3(-DataHolder.s_currentMap * m_itemWidth, r_contentHolder.localPosition.y, r_contentHolder.localPosition.z);
                }
            }
        }

        #endregion

        public void NextMap() {
            DataHolder.s_currentMap = DataHolder.FixedMod(DataHolder.s_currentMap + 1, DataHolder.s_maps.Count);
            m_finished = false;
        }

        public void PreviousMap() {
            DataHolder.s_currentMap = DataHolder.FixedMod(DataHolder.s_currentMap - 1, DataHolder.s_maps.Count);
            m_finished = false;
        }
    }
}