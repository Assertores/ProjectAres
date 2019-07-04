using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class MatchManager : MonoBehaviour {

        public static MatchManager s_currentMatch;

        #region MonoBehaviour

        private void Awake() {
            if (s_currentMatch)
                Destroy(s_currentMatch.gameObject);
            s_currentMatch = this;
        }

        #endregion
        #region NextMatch

        bool h_startGameOngoing = false;
        public void StartGame() {
            if (h_startGameOngoing)
                return;
            h_startGameOngoing = true;

            TransitionHandler.ReadyToChange += ContinueToNextMatch;
            TransitionHandler.StartOutTransition();
        }

        void ContinueToNextMatch() {
            TransitionHandler.ReadyToChange -= ContinueToNextMatch;
            TransitionHandler.ReadyToStart += FinishedContinueToNextMatch;

            if (DataHolder.s_modis[DataHolder.s_currentModi].m_isTeamMode) {
                SceneManager.LoadScene(StringCollection.S_TEAMSELECT);
            } else {
                SceneManager.LoadScene(StringCollection.S_MAP);
                TransitionHandler.ReadyToStart += StartMap;
            }
        }

        public void ContinueToMap() {
            if (h_startGameOngoing)
                return;
            h_startGameOngoing = true;

            TransitionHandler.ReadyToChange += ContinueToNextMap;
            TransitionHandler.StartOutTransition();
        }

        void ContinueToNextMap() {
            TransitionHandler.ReadyToChange -= ContinueToNextMap;
            TransitionHandler.ReadyToStart += FinishedContinueToNextMatch;
            TransitionHandler.ReadyToStart += StartMap;

            SceneManager.LoadScene(StringCollection.S_MAP);
        }

        void FinishedContinueToNextMatch() {
            h_startGameOngoing = false;
        }

        #endregion
        #region InGame

        bool h_OngoingGame = false;
        void StartMap() {
            if (h_OngoingGame)
                return;
            h_OngoingGame = true;

            TransitionHandler.ReadyToStart -= StartMap;
            DataHolder.s_modis[DataHolder.s_currentModi].EndGame += GMEnded;

            DataHolder.s_modis[DataHolder.s_currentModi].StartGame();
        }

        void GMEnded(bool normal) {
            DataHolder.s_modis[DataHolder.s_currentModi].EndGame -= GMEnded;

            if (normal) {
                TransitionHandler.ReadyToChange += BackToMainMenu;
            } else {
                TransitionHandler.ReadyToChange += ContinueToWinScreen;
            }
        }

        void BackToMainMenu() {
            TransitionHandler.ReadyToChange -= BackToMainMenu;

            SceneManager.LoadScene(StringCollection.S_MAINMENU);
        }

        void ContinueToWinScreen() {
            TransitionHandler.ReadyToChange -= ContinueToWinScreen;

            SceneManager.LoadScene(StringCollection.S_WINSCREEN);
        }

        #endregion
        #region WinScreen



        #endregion
    }
}