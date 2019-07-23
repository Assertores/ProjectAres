using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class KillFeed : MonoBehaviour {

        static KillFeed s_reference = null;

        [Header("References")]
        [SerializeField] GameObject p_killFeedItem;
        [SerializeField] GameObject p_dieFeedItem;
        [SerializeField] RectTransform r_content;

        [Header("Balancing")]
        [SerializeField] float m_scrollSpeed = 1;

        float m_itemHight;
        float m_lastFeedTime = float.MaxValue;

        void Start() {
            if (!p_killFeedItem.GetComponent<KillFeedRefHolder>()) {
                print("kill feed Item has no Reference Holder");
                Destroy(this);
                return;
            }

            if (s_reference != null) {
                print("there is alreade a Kill Feed Handler in the szene");
                Destroy(this);
                return;
            }
            s_reference = this;

            if (!r_content) {
                r_content = (RectTransform)transform;
            }
            ((RectTransform)transform).sizeDelta = new Vector2(((RectTransform)transform).sizeDelta.x, 0);

            m_itemHight = ((RectTransform)p_killFeedItem.transform).rect.height;
        }

        private void OnDestroy() {
            if (s_reference == this) {
                s_reference = null;
            }
        }

        // Update is called once per frame
        void Update() {
            if (r_content.anchoredPosition.y < r_content.sizeDelta.y) {
                r_content.anchoredPosition += new Vector2(0, m_scrollSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// kümmert sich um den killfeed
        /// </summary>
        /// <param name="killerIcon">das icon des charakter, der getöted hat</param>
        /// <param name="weaponIcon">die waffe, die getöted hat</param>
        /// <param name="victimIcon">das icon des charakters, der gestorben ist</param>
        public static void AddKill(Sprite killerIcon, Sprite weaponIcon, Sprite victimIcon) {
            if (!s_reference)
                return;

            GameObject item;
            if (killerIcon) {
                item = Instantiate(s_reference.p_killFeedItem, s_reference.r_content);
                KillFeedRefHolder holder = item.GetComponent<KillFeedRefHolder>();
                holder.m_killer.sprite = killerIcon;
                holder.m_weapon.sprite = weaponIcon;
                holder.m_victim.sprite = victimIcon;
            } else {
                item = Instantiate(s_reference.p_dieFeedItem, s_reference.r_content);
                KillFeedRefHolder holder = item.GetComponent<KillFeedRefHolder>();
                holder.m_weapon.sprite = weaponIcon;
                holder.m_victim.sprite = victimIcon;
            }
            

            ((RectTransform)(item.transform)).anchoredPosition -= new Vector2(0, s_reference.r_content.rect.height);
            s_reference.r_content.sizeDelta = new Vector2(s_reference.r_content.sizeDelta.x, s_reference.r_content.sizeDelta.y + s_reference.m_itemHight);
            
            s_reference.m_lastFeedTime = Time.timeSinceLevelLoad;
        }
    }
}