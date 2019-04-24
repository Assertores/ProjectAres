using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace ProjectAres {
    [RequireComponent(typeof(Collider2D))]
    public class BackToMainMenu : MonoBehaviour {

        #region Variables

        int m_playerCount = 0;
        [SerializeField] int m_restartTime;
        [SerializeField] TextMeshProUGUI m_restartTimeText;
        

        #endregion
        #region MonoBehaviour

        void Start() {
            m_restartTimeText.text = m_restartTime.ToString();
        }
        
        void Update() {
            if(m_playerCount >= Player.s_references.Count) {
                SceneManager.LoadScene(StringCollection.MAINMENU);
            }
            if (Time.timeSinceLevelLoad >= m_restartTime) {
                SceneManager.LoadScene(StringCollection.MAINMENU);
            }
            m_restartTimeText.text = Mathf.RoundToInt((m_restartTime - Time.timeSinceLevelLoad)).ToString();

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