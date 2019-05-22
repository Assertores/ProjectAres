using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace PPBC {
    [RequireComponent(typeof(Collider2D))]
    public class BackToMainMenu : MonoBehaviour, IScriptQueueItem {

        #region Variables

        [Header("References")]
        [SerializeField] TextMeshProUGUI m_winscreenRestartText;
        [SerializeField] TextMeshProUGUI m_restartTimeText;

        [Header("Balancing")]
        [SerializeField] int m_restartTime;

        float m_startTime;
        int m_playerCount = 0;
        bool m_active = false;

        #endregion
        #region MonoBehaviour

        private void Start() {
            //if(DataHolder.s_gameMode == e_gameMode.FFA_CASUAL ||(DataHolder.s_gameMode == e_gameMode.FAIR_TOURNAMENT && DataHolder.s_firstMatch)) {
                EndScreenManager.s_ref?.AddItem(this, 10);
            //}
        }

        void Update() {
            if (!m_active)
                return;

            if(m_playerCount >= Player.s_references.Count) {
                ChangeSzene();
            }

            m_restartTimeText.text = m_restartTime.ToString();
            m_winscreenRestartText.text = "Time till Restart";
            m_restartTimeText.text = Mathf.RoundToInt((m_restartTime - (Time.timeSinceLevelLoad - m_startTime))).ToString();

            if (Time.timeSinceLevelLoad - m_startTime >= m_restartTime) {
                ChangeSzene();
            }
        }

        #endregion

        void ChangeSzene() {
            if (DataHolder.s_gameMode == e_gameMode.FAIR_TOURNAMENT && !DataHolder.s_firstMatch) {
                print("restar level: " + DataHolder.s_level);
                SceneManager.LoadScene(DataHolder.s_level);
                return;
            }

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
        #region IScriptQueueItem

        public bool FirstTick() {
            m_startTime = Time.timeSinceLevelLoad;
            m_active = true;
            return true;
        }

        public bool DoTick() {
            return true;
        }

        #endregion
    }
}