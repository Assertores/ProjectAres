using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class MatchManager : MonoBehaviour {

        public static MatchManager s_currentMatch;

        #region Variables

        public int m_matchCount { get; private set; } = 1;
        public Dictionary<Player, int> m_teamHolder = new Dictionary<Player, int>();

        #endregion
        #region MonoBehaviour

        private void Awake() {
            if (s_currentMatch)
                Destroy(s_currentMatch.gameObject);
            s_currentMatch = this;

            foreach(var it in Player.s_references) {
                it.ResetStatsFull();
                it.InControle(true);
                it.CanChangeCharacter(true);
                it.Invincable(true);
            }
        }

        #endregion
        #region NextMatch

        bool h_startGameOngoing = false;
        public void StartGame() {//TODO: move to IEnumerator to make countDown
            if (h_startGameOngoing)
                return;
            h_startGameOngoing = true;

            foreach(var it in Player.s_references) {
                it.ResetMatchStats();
                it.CanChangeCharacter(false);
            }

            TransitionHandler.ReadyToChange += ContinueToNextMatch;
            TransitionHandler.StartOutTransition();
        }

        void ContinueToNextMatch() {
            TransitionHandler.ReadyToChange -= ContinueToNextMatch;
            TransitionHandler.ReadyToStart += FinishedContinueToNextMatch;

            if (DataHolder.s_modis[DataHolder.s_currentModi].m_isTeamMode) {
                SceneManager.LoadScene(StringCollection.S_TEAMSELECT);
            } else {

                foreach (var it in Player.s_references) {
                    it.ResetGameStats();
                    it.Invincable(false);
                    it.InControle(true);
                }

                TransitionHandler.ReadyToStart += StartMap;
                
                SceneManager.LoadScene(StringCollection.S_MAP);
            }
        }

        public void ContinueToMap() {
            if (h_startGameOngoing)
                return;
            h_startGameOngoing = true;

            print("starting modi: " + DataHolder.s_modis[DataHolder.s_currentModi].m_name);

            DataHolder.s_modis[DataHolder.s_currentModi].StartTransition();

            TransitionHandler.ReadyToChange += ContinueToNextMap;
            TransitionHandler.StartOutTransition();
        }

        void ContinueToNextMap() {
            TransitionHandler.ReadyToChange -= ContinueToNextMap;
            TransitionHandler.ReadyToStart += FinishedContinueToNextMatch;
            TransitionHandler.ReadyToStart += StartMap;

            foreach (var it in Player.s_references) {
                it.ResetGameStats();
                it.Invincable(false);
                it.InControle(true);
            }

            SceneManager.LoadScene(StringCollection.S_MAP);
        }

        void FinishedContinueToNextMatch() {
            TransitionHandler.ReadyToStart -= FinishedContinueToNextMatch;
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
            h_OngoingGame = false;

            DataHolder.s_modis[DataHolder.s_currentModi].EndGame -= GMEnded;

            if (normal) {
                TransitionHandler.ReadyToChange += ContinueToWinScreen;
            } else {
                TransitionHandler.ReadyToChange += BackToMainMenu;
            }

            TransitionHandler.StartOutTransition();
        }

        void BackToMainMenu() {
            TransitionHandler.ReadyToChange -= BackToMainMenu;

            foreach(var it in Player.s_references) {
                it.Invincable(true);
                it.Respawn(transform.position);
            }

            SceneManager.LoadScene(StringCollection.S_MAINMENU);
        }

        void ContinueToWinScreen() {
            TransitionHandler.ReadyToChange -= ContinueToWinScreen;
            TransitionHandler.ReadyToStart += InWinScreen;

            foreach (var it in Player.s_references) {
                it.Invincable(true);
                it.Respawn(transform.position);
            }

            SceneManager.LoadScene(StringCollection.S_WINSCREEN);
        }

        #endregion
        #region WinScreen

        void InWinScreen() {
            TransitionHandler.ReadyToStart -= InWinScreen;
            TransitionHandler.ReadyToChange += ResetTeam;

            m_matchCount--;
            if(m_matchCount > 0) {
                TransitionHandler.ReadyToChange += ContinueToNextMatch;
            } else {
                TransitionHandler.ReadyToChange += BackToMainMenu;
            }

            WinScreenManager.s_singelton.Init();
        }

        void ResetTeam() {
            foreach(var it in Player.s_references) {
                it.ResetTeam();
            }
        }

        #endregion
    }
}