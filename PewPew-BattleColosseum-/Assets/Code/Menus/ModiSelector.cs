using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class ModiSelector : MonoBehaviour {

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

            for (int i = 0; i < DataHolder.s_modis.Length; i++) {
                SelectItemRefHolder element = Instantiate(p_item, r_contentHolder).GetComponent<SelectItemRefHolder>();
                element.transform.localPosition = new Vector3(i * m_itemWidth, 0, 0);
                element.transform.localRotation = Quaternion.Euler(Vector3.zero);

                r_contentHolder.sizeDelta += new Vector2(m_itemWidth, 0);

                element.r_name.text = DataHolder.s_modis[i].m_name;
                element.r_image.sprite = DataHolder.s_modis[i].m_icon;
                element.r_text.text = DataHolder.s_modis[i].m_text;
            }
        }

        void Update() {
            if (m_finished) {
                return;
            }

            if (r_contentHolder.localPosition.x > -DataHolder.s_currentModi * m_itemWidth) {
                r_contentHolder.localPosition -= new Vector3(m_changeSpeed * Time.deltaTime, 0, 0);
                if (r_contentHolder.localPosition.x < -DataHolder.s_currentModi * m_itemWidth) {
                    m_finished = true;
                    r_contentHolder.localPosition = new Vector3(-DataHolder.s_currentModi * m_itemWidth, r_contentHolder.localPosition.y, r_contentHolder.localPosition.z);
                }
            } else {
                r_contentHolder.localPosition += new Vector3(m_changeSpeed * Time.deltaTime, 0, 0);
                if (r_contentHolder.localPosition.x > -DataHolder.s_currentModi * m_itemWidth) {
                    m_finished = true;
                    r_contentHolder.localPosition = new Vector3(-DataHolder.s_currentModi * m_itemWidth, r_contentHolder.localPosition.y, r_contentHolder.localPosition.z);
                }
            }
        }

        #endregion

        public void NextModi() {
            DataHolder.s_currentModi = DataHolder.FixedMod(DataHolder.s_currentModi + 1, DataHolder.s_modis.Length);
            m_finished = false;
        }

        public void PreviousModi() {
            DataHolder.s_currentModi = DataHolder.FixedMod(DataHolder.s_currentModi - 1, DataHolder.s_modis.Length);
            m_finished = false;
        }
    }
}