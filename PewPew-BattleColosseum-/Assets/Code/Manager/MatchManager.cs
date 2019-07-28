using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace PPBC {
    public class MatchManager : MonoBehaviour {

        public static MatchManager s_currentMatch;

        #region Variables

        public int m_matchCount { get; private set; } = 1;
        public Dictionary<Player, int> m_teamHolder = new Dictionary<Player, int>();
        [SerializeField] float m_delay;

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
            print("starting a new match");

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
                m_teamHolder = null;
                StartMap();
            }
        }

        void TeamSelect() {
            print("selecting team");

            TeamSelectI();
            SceneManager.LoadScene(StringCollection.S_TEAMSELECT);
        }

        /// <summary>
        /// entrance to map
        /// </summary>
        void StartMap() {
            print("starting map: " + DataHolder.s_maps[DataHolder.s_currentMap].m_name);

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
            print("stopt map " + (normal ? "normal" : "ireguler"));

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
            print("started the winscreen");

            WinScreenI();

            m_matchCount--;

            SceneManager.LoadScene(StringCollection.S_WINSCREEN);

            foreach (var it in Player.s_references) {
                it.InControle(false);
                it.ResetVelocity();
            }
        }

        void StartWinScreen() {
            print("initialiced winscreen");

            WinScreenManager.s_singelton.Init();
        }

        void EndWinScreen() {
            print("stoped winscreen");

            ResetTeam();
            
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
            print("resumed back to Main menu");

            ResetTeam();

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
            print("MainMenuE everytime");

            if (h_singleMM)
                return;
            h_singleMM = true;

            print("MainMenuE once");

            TransitionHandler.ReadyToChange += MainMenuO;

            TransitionHandler.StartOutTransition();
        }

        void MainMenuO() {
            print("MainMenuO");

            TransitionHandler.ReadyToChange -= MainMenuO;

            StartMatch();

            h_singleMM = false;
        }

        #endregion
        #region TeamSelect

        bool h_singleTS = false;
        void TeamSelectI() {
            print("TeamSelectI everytime");

            if (h_singleTS)
                return;
            h_singleTS = true;

            print("TeamSelectI once");

            TransitionHandler.ReadyToChange += TeamSelectO;
            
        }

        void TeamSelectE() {
            print("TeamSelectE");

            TransitionHandler.StartOutTransition();
        }

        void TeamSelectO() {
            print("TeamSelectO");

            TransitionHandler.ReadyToChange -= TeamSelectO;

            StartMap();

            h_singleTS = false;
        }

        #endregion
        #region Map

        bool h_singleMap = false;
        void MapI() {
            print("MapI everytime");

            if (h_singleMap)
                return;
            h_singleMap = true;

            print("MapI once");

            DataHolder.s_modis[DataHolder.s_currentModi].EndGame += MapE;
            TransitionHandler.ReadyToStart += MapR;
        }

        void MapR() {
            print("MapR");

            TransitionHandler.ReadyToStart -= MapR;

            DataHolder.s_modis[DataHolder.s_currentModi].StartGame();
        }

        bool h_mapEnd;
        void MapE(bool normal) {
            print("MapE");

            DataHolder.s_modis[DataHolder.s_currentModi].EndGame -= MapE;
            TransitionHandler.ReadyToChange += MapO;

            h_mapEnd = normal;

            TransitionHandler.StartOutTransition();
        }

        void MapO() {
            print("MapO");

            TransitionHandler.ReadyToChange -= MapO;

            ExitMap(h_mapEnd);

            h_singleMap = false;
        }

        #endregion
        #region WinScreen

        bool h_singleWS = false;
        void WinScreenI() {
            print("WinScreenI everytime");

            if (h_singleWS)
                return;
            h_singleWS = true;

            print("WinScreenI once");

            TransitionHandler.ReadyToChange += WinScreenO;
            TransitionHandler.ReadyToStart += WinScreenR;
        }

        void WinScreenR() {
            print("WinScreenR");

            TransitionHandler.ReadyToStart -= WinScreenR;

            StartWinScreen();
        }

        void WinScreenO() {
            print("WinScreenO");

            TransitionHandler.ReadyToChange -= WinScreenO;

            EndWinScreen();

            h_singleWS = false;
        }

        #endregion
        #endregion

        Coroutine h_startGame;
        public void StartGame() {
            h_startGame = StartCoroutine(IEStartGame());
            Timer.StartTimer(m_delay);
        }

        public void AbortStartGame() {
            StopCoroutine(h_startGame);
            Timer.AbortTimer();
        }

        IEnumerator IEStartGame() {

            yield return new WaitForSeconds(m_delay);
            MainMenuE();

        }

        Coroutine h_cTM;
        public void ContinueToMap() {

            h_cTM = StartCoroutine(IEContinueToMap());
        }

        public void AbortContinueToMap() {
            StopCoroutine(h_cTM);
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