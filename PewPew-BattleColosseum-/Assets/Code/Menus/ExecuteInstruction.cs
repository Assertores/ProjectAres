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
                foreach (var it in Player.s_references) {
                    TransitionHandler.ReadyToChange += it.DoColliderListClear;
                }

                TransitionHandler.StartOutTransition();
            }
        }

        void BackToMMImideatly() {
            TransitionHandler.ReadyToChange -= BackToMMImideatly;
            

            SceneManager.LoadScene(StringCollection.S_MAINMENU);
        }
    }
}