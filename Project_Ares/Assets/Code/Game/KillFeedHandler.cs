using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class KillFeedHandler : Sauerbraten {

        static KillFeedHandler s_reference = null;

        [Header("References")]
        [SerializeField] GameObject m_killFeedItemPrefab;
        [SerializeField] CanvasGroup m_fade;
        [SerializeField] RectTransform m_content;

        [Header("Balancing")]
        [SerializeField] float m_scrollSpeed = 1;
        [SerializeField] float m_fadeTime = 1;
        [SerializeField] float m_fadeDuration = 1;
        [SerializeField] bool m_fadeOut = true;

        float m_itemHight;
        float m_lastFeedTime = float.MaxValue;

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

            if (!m_content) {
                m_content = (RectTransform)transform;
            }
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
            if(((RectTransform)transform).anchoredPosition.y < ((RectTransform)transform).rect.height) {
                transform.position += new Vector3(0, m_scrollSpeed * Time.deltaTime, 0);
            }
            if(m_fadeOut && Time.timeSinceLevelLoad - m_lastFeedTime > m_fadeTime) {
                m_fade.alpha = 1 - (Time.timeSinceLevelLoad - m_lastFeedTime - m_fadeTime) / m_fadeDuration;
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

            GameObject item = Instantiate(s_reference.m_killFeedItemPrefab, s_reference.m_content);
            KillFeedRefHolder holder = item.GetComponent<KillFeedRefHolder>();
            holder.m_killerName.text = killerName;
            holder.m_killerIcon.sprite = killerIcon;
            holder.m_weaponIcon.sprite = weaponIcon;
            holder.m_victimIcon.sprite = victimIcon;
            holder.m_victimName.text = victimName;

            ((RectTransform)(item.transform)).anchoredPosition -= new Vector2(0, s_reference.m_content.rect.height);
            s_reference.m_content.sizeDelta = new Vector2(s_reference.m_content.sizeDelta.x, s_reference.m_content.sizeDelta.y + s_reference.m_itemHight);

            s_reference.m_fade.alpha = 1;
            s_reference.m_lastFeedTime = Time.timeSinceLevelLoad;
        }
    }
}