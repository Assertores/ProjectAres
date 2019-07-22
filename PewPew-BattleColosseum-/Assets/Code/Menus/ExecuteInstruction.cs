using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class ExecuteInstruction : MonoBehaviour {

        public void BackToMM() {
            if (MatchManager.s_currentMatch) {
                MatchManager.s_currentMatch.StopGame();
            } else {
                TransitionHandler.ReadyToChange = BackToMMImideatly;
                TransitionHandler.StartOutTransition();
            }
        }

        void BackToMMImideatly() {
            SceneManager.LoadScene(StringCollection.S_MAINMENU);
        }
    }
}