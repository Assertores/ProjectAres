using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PPBC {
    public class BadPunJokes : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] TextMeshProUGUI r_speachBubble;
        [SerializeField] TextMeshProUGUI[] r_replySpeachBubble;

        [Header("Balancing")]
        [TextArea]
        [SerializeField] string[] m_puns;
        [TextArea]
        [SerializeField] string m_reply = "-_-";
        [SerializeField] float m_punDelay = 15;
        [SerializeField] float m_punDuration = 5;

        #endregion
        #region MonoBehaviour

        private void Start() {
            if (!r_speachBubble) {
                print("FATAL: No Speech buble");
                Destroy(this);
                return;
            }
            if(r_replySpeachBubble.Length == 0) {
                print("No Replys");
            }

            foreach(var it in r_replySpeachBubble) {
                it.text = m_reply;
            }
        }

        bool h_doOnce = false;
        private void Update() {
            if(Time.time % m_punDelay < m_punDuration) {
                if (!h_doOnce) {
                    h_doOnce = true;
                    ShowPun();
                }
            }else if (h_doOnce) {
                h_doOnce = false;
                HidePun();
            }
        }

        #endregion

        void ShowPun() {
            r_speachBubble.text = m_puns[Random.Range(0, m_puns.Length)];

            r_speachBubble.gameObject.SetActive(true);
            foreach(var it in r_replySpeachBubble) {
                it.gameObject.SetActive(true);
            }
        }

        void HidePun() {
            r_speachBubble.gameObject.SetActive(false);
            foreach (var it in r_replySpeachBubble) {
                it.gameObject.SetActive(false);
            }
        }
    }
}