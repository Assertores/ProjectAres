﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace ProjectAres {
    [RequireComponent(typeof(Collider2D))]
    public class BackToMainMenu : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] TextMeshProUGUI m_winscreenRestartText;
        [SerializeField] TextMeshProUGUI m_restartTimeText;

        [Header("Balancing")]
        [SerializeField] int m_restartTime;
        [SerializeField] int m_pillarRiseTime;

        float m_startTime;

        int m_playerCount = 0;
        #endregion
        #region MonoBehaviour

        private void OnEnable() {
            m_startTime = Time.timeSinceLevelLoad;
        }

        void Update() {
            if(m_playerCount >= Player.s_references.Count) {
                ChangeSzene();
            }
            if (m_pillarRiseTime < Time.timeSinceLevelLoad - m_startTime) {
                m_restartTimeText.text = m_restartTime.ToString();
                m_winscreenRestartText.text = "Time till Restart";
                m_restartTimeText.text = Mathf.RoundToInt(((m_restartTime + m_pillarRiseTime) - (Time.timeSinceLevelLoad - m_startTime))).ToString();

                if (Time.timeSinceLevelLoad - m_startTime >= (m_restartTime + m_pillarRiseTime)) {
                    ChangeSzene();
                }
               
            }
        }

        #endregion

        void ChangeSzene() {
            if (DataHolder.s_gameMode == e_gameMode.FAIR_TOURNAMENT && !DataHolder.s_firstMatch) {
                print("restar level: " + DataHolder.s_level);
                SceneManager.LoadScene(DataHolder.s_level);
                return;
            }
            print("i'm here");

            SceneManager.LoadScene(StringCollection.MAINMENU);
        }

        #region Physics

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.tag == StringCollection.PLAYER)
                m_playerCount++;
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.gameObject.tag == StringCollection.PLAYER)
                m_playerCount--;
        }

        #endregion
    }
}