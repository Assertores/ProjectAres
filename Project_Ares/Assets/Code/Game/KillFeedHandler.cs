using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class KillFeedHandler : MonoBehaviour {

        static KillFeedHandler s_reference = null;

        [Header("References")]
        [SerializeField] GameObject m_killFeedItemPrefab;

        [Header("Balancing")]
        [SerializeField] float m_scrollSpeed = 1;

        float m_itemHight;

        void Start() {
            if (!m_killFeedItemPrefab.GetComponent<KillFeedRefHolder>()) {
                print("kill feed Item has no Reference Holder");
                Destroy(this);
                return;
            }

            if(s_reference != null) {
                print("there is alreade a Kill Feed Handler in the szene");
                Destroy(this);
                return;
            }
            s_reference = this;

            ((RectTransform)transform).sizeDelta = new Vector2(((RectTransform)transform).sizeDelta.x, 0);

            m_itemHight = ((RectTransform)m_killFeedItemPrefab.transform).rect.height;
        }

        private void OnDestroy() {
            if(s_reference == this) {
                s_reference = null;
            }
        }

        // Update is called once per frame
        void Update() {
            if(transform.position.y < ((RectTransform)transform).rect.height) {
                transform.position += new Vector3(0, m_scrollSpeed * Time.deltaTime, 0);
            }
        }

        /// <summary>
        /// kümmert sich um den killfeed
        /// </summary>
        /// <param name="killerName">der name des spielers, der getöted hat</param>
        /// <param name="killerIcon">das icon des charakter, der getöted hat</param>
        /// <param name="weaponIcon">die waffe, die getöted hat</param>
        /// <param name="victimIcon">das icon des charakters, der gestorben ist</param>
        /// <param name="victimName">der name des spielers, der gestorben ist</param>
        public static void AddKill(string killerName, Sprite killerIcon, Sprite weaponIcon, Sprite victimIcon, string victimName) {
            if (!s_reference)
                return;

            GameObject item = Instantiate(s_reference.m_killFeedItemPrefab, s_reference.transform);
            KillFeedRefHolder holder = item.GetComponent<KillFeedRefHolder>();
            holder.m_killerName.text = killerName;
            holder.m_killerIcon.sprite = killerIcon;
            holder.m_weaponIcon.sprite = weaponIcon;
            holder.m_victimIcon.sprite = victimIcon;
            holder.m_victimName.text = victimName;

            item.transform.position -= new Vector3(0, ((RectTransform)s_reference.transform).rect.height, 0);
            ((RectTransform)s_reference.transform).sizeDelta = new Vector2(((RectTransform)s_reference.transform).sizeDelta.x, ((RectTransform)s_reference.transform).sizeDelta.y + s_reference.m_itemHight);
        }
    }
}