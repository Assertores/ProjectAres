using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectAres {
    [RequireComponent(typeof(Collider2D))]
    public class BackToMainMenu : MonoBehaviour {

        #region Variables

        int m_playerCount = 0;

        #endregion
        #region MonoBehaviour

        void Start() {

        }
        
        void Update() {
            if(m_playerCount >= Player.s_references.Count) {
                SceneManager.LoadScene(StringCollection.MAINMENU);
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