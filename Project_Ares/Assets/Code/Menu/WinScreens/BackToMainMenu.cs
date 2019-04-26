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

        int m_playerCount = 0;
        #endregion
        #region MonoBehaviour

        void Start() {
            
        }
        
        void Update() {
            if(m_playerCount >= Player.s_references.Count) {
                SceneManager.LoadScene(StringCollection.MAINMENU);
            }
            if (m_pillarRiseTime < Time.timeSinceLevelLoad) {
                m_restartTimeText.text = m_restartTime.ToString();
                m_winscreenRestartText.text = "Time till Restart";
                m_restartTimeText.text = Mathf.RoundToInt(((m_restartTime + m_pillarRiseTime) - Time.timeSinceLevelLoad)).ToString();

                if (Time.timeSinceLevelLoad >= (m_restartTime + m_pillarRiseTime)) {
                    SceneManager.LoadScene(StringCollection.MAINMENU);
                }
               
            }
        }

        #endregion
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