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
        }

        #endregion
        #region Logic

        /// <summary>
        /// entrance to Match
        /// </summary>
        public void StartMatch() {

            foreach (var it in Player.s_references) {
                it.ResetMatchStats();
                it.CanChangeCharacter(false);
            }

            StartNextGame();
        }

        /// <summary>
        /// entrance to next game
        /// </summary>
        void StartNextGame() {
            print("starting modi: " + DataHolder.s_modis[DataHolder.s_currentModi].m_name);

            if (DataHolder.s_modis[DataHolder.s_currentModi].m_isTeamMode) {
                TeamSelect();
            } else {
                StartMap();
            }
        }

        void TeamSelect() {
            TeamSelectI();
            SceneManager.LoadScene(StringCollection.S_TEAMSELECT);
        }

        /// <summary>
        /// entrance to map
        /// </summary>
        void StartMap() {
            StartCoroutine(IEStartMap(DataHolder.s_modis[DataHolder.s_currentModi].StartTransition()));
        }

        
        IEnumerator IEStartMap(float delay) {
            yield return new WaitForSeconds(delay);

            MapI();

            SceneManager.LoadScene(StringCollection.S_MAP);

            foreach (var it in Player.s_references) {
                it.ResetGameStats();
                it.Invincable(false);
                it.InControle(true);
            }
        }

        void ExitMap(bool normal) {

            //reset global stuff
            foreach (var it in Player.s_references) {
                it.Invincable(true);
                it.Respawn(transform.position);
            }

            if (normal) {
                WinScreen();
            } else {
                BackToMM();
            }
        }

        void WinScreen() {
            WinScreenI();

            SceneManager.LoadScene(StringCollection.S_WINSCREEN);

            foreach (var it in Player.s_references) {
                it.InControle(false);
                it.ResetVelocity();
            }
        }

        void StartWinScreen() {
            WinScreenManager.s_singelton.Init();
        }

        void EndWinScreen() {
            m_matchCount--;
            if (m_matchCount > 0) {
                StartNextGame();
            } else {
                BackToMM();
            }
        }

        /// <summary>
        /// exit of Match
        /// </summary>
        public void BackToMM() {
            SceneManager.LoadScene(StringCollection.S_MAINMENU);

            foreach (var it in Player.s_references) {
                it.InControle(true);
                it.Invincable(true);
                it.Respawn(transform.position);
                it.CanChangeCharacter(true);
            }
        }

        #endregion
        #region Comunication
        //void SceneI(); //IN:    will be loaded next
        //void SceneR(); //READY: Transition has endet
        //void SceneE(); //EXIT:  start out Transition
        //void SceneO(); //OUT:   Ready to Change to next scene

        #region MainMenu

        bool h_singleMM = false;
        void MainMenuE() {
            if (h_singleMM)
                return;
            h_singleMM = true;

            TransitionHandler.ReadyToChange += MainMenuO;

            TransitionHandler.StartOutTransition();
        }

        void MainMenuO() {
            TransitionHandler.ReadyToChange -= MainMenuO;

            StartMatch();

            h_singleMM = false;
        }

        #endregion
        #region TeamSelect

        bool h_singleTS = false;
        void TeamSelectI() {
            if (h_singleTS)
                return;
            h_singleTS = true;

            TransitionHandler.ReadyToChange += TeamSelectO;
            
        }

        void TeamSelectE() {
            TransitionHandler.StartOutTransition();
        }

        void TeamSelectO() {
            TransitionHandler.ReadyToChange -= TeamSelectO;

            StartMap();

            h_singleTS = false;
        }

        #endregion
        #region Map

        bool h_singleMap = false;
        void MapI() {
            if (h_singleMap)
                return;
            h_singleMap = true;

            DataHolder.s_modis[DataHolder.s_currentModi].EndGame += MapE;
            TransitionHandler.ReadyToStart += MapR;
        }

        void MapR() {
            TransitionHandler.ReadyToStart -= MapR;

            DataHolder.s_modis[DataHolder.s_currentModi].StartGame();
        }

        bool h_mapEnd;
        void MapE(bool normal) {
            DataHolder.s_modis[DataHolder.s_currentModi].EndGame -= MapE;
            TransitionHandler.ReadyToChange += MapO;

            h_mapEnd = normal;

            TransitionHandler.StartOutTransition();
        }

        void MapO() {
            TransitionHandler.ReadyToChange -= MapO;

            ExitMap(h_mapEnd);

            h_singleMap = false;
        }

        #endregion
        #region WinScreen

        bool h_singleWS = false;
        void WinScreenI() {
            if (h_singleWS)
                return;
            h_singleWS = true;

            TransitionHandler.ReadyToChange += WinScreenO;
            TransitionHandler.ReadyToStart += WinScreenR;
        }

        void WinScreenR() {
            StartWinScreen();
        }

        void WinScreenO() {
            TransitionHandler.ReadyToChange -= WinScreenO;

            WinScreen();

            h_singleWS = false;
        }

        #endregion
        #endregion

        public void StartGame() {
            StartCoroutine(IEStartGame());
        }

        public void AbortStartGame() {
            StopCoroutine(IEStartGame());
        }

        IEnumerator IEStartGame() {
            yield return null;//TODO: add countdown
            MainMenuE();
        }

        public void ContinueToMap() {
            StartCoroutine(IEContinueToMap());
        }

        public void AbortContinueToMap() {
            StopCoroutine(IEContinueToMap());
        }

        IEnumerator IEContinueToMap() {
            yield return null;//TODO: add countdown
            TeamSelectE();
        }


        public void StopGame() {
            if (h_singleMap) {
                DataHolder.s_modis[DataHolder.s_currentModi].AbortGame();
            } else {
                BackToMM();
            }
        }

        void ResetTeam() {
            foreach (var it in Player.s_references) {
                it.ResetTeam();
            }
        }
    }
}