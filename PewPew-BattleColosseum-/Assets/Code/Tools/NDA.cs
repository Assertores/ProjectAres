using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class NDA : MonoBehaviour {

        [SerializeField] float m_delay = 2;

        private void Start() {
            StartCoroutine(Wait());
        }

        IEnumerator Wait() {
            yield return new WaitForSeconds(m_delay);
            TransitionHandler.ReadyToChange += ReturnImideatly;

            TransitionHandler.StartOutTransition();
        }

        void ReturnImideatly() {
            TransitionHandler.ReadyToChange -= ReturnImideatly;

            SceneManager.LoadScene(StringCollection.S_MAINMENU);
        }
    }
}