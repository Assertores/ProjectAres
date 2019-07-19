﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PPBC {
    public class BadPunJokes : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject r_speachBubble;
        [SerializeField] TextMeshProUGUI t_speachBubble;
        [SerializeField] GameObject[] r_replySpeachBubble;
        [SerializeField] TextMeshProUGUI[] t_replySpeachBubble;

        [Header("Balancing")]
        [TextArea]
        [SerializeField] string[] m_puns;
        [TextArea]
        [SerializeField] string m_reply = "-_-";
        [SerializeField] float m_punDelay = 15;
        [SerializeField] float m_punDuration = 5;
        [SerializeField] float m_replayDelay = 0.5f;

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

            foreach(var it in t_replySpeachBubble) {
                it.text = m_reply;
            }

            r_speachBubble.SetActive(false);
            foreach(var it in r_replySpeachBubble) {
                it.SetActive(false);
            }
        }

        bool h_doOnce = false;
        private void Update() {
            if(Time.time % m_punDelay < m_punDuration) {
                if (!h_doOnce) {
                    h_doOnce = true;
                    StartCoroutine(ShowPun());
                }
            }else if (h_doOnce) {
                h_doOnce = false;
                HidePun();
            }
        }

        #endregion

        IEnumerator ShowPun() {
            t_speachBubble.text = m_puns[Random.Range(0, m_puns.Length)];

            r_speachBubble.gameObject.SetActive(true);

            yield return new WaitForSeconds(m_replayDelay);

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