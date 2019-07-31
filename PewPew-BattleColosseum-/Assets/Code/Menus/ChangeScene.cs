using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class ChangeScene : MonoBehaviour {

        #region Variables

        [SerializeField] string m_nextScene ;

        #endregion

        public void DoChangeScene() {
            TransitionHandler.ReadyToChange += ChangeSceneImmediately;

            TransitionHandler.StartOutTransition();
        }

        public void ChangeSceneImmediately() {
            TransitionHandler.ReadyToChange -= ChangeSceneImmediately;

            SceneManager.LoadScene(m_nextScene);
        }
    }
}