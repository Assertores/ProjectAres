using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class ChangeScene : MonoBehaviour {

        #region Variables

        [SerializeField] string m_nextScene = StringCollection.S_MAINMENU;

        #endregion

        public void DoChangeScene() {
            TransitionHandler.ReadyToChange += ChangeSceneImmediately;

            TransitionHandler.StartOutTransition();
        }

        public void ChangeSceneImmediately() {
            SceneManager.LoadScene(m_nextScene);
        }
    }
}