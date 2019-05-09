﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ProjectAres {
    public class MenuManager : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_SpawnPoint;

        #endregion
        #region MonoBehaviour
        #region Singelton

        public static MenuManager _singelton = null;

        private void Awake() {
            if (_singelton)
                Destroy(this);

            _singelton = this;
        }
        private void OnDestroy() {
            if (_singelton == this)
                _singelton = null;
        }

        #endregion

        private void Start() {
            foreach (var it in Player.s_references) {
                it.DoReset();
                it.Invincible(true);
                it.transform.position = m_SpawnPoint.transform.position;
                it.SetChangeCharAble(true);
            }
        }

        

        #endregion

        public void StartGame() {

            foreach(var it in Player.s_references) {
                it.SetChangeCharAble(false);
            }

            SceneManager.LoadScene(DataHolder.s_level);
            //SceneManager.LoadScene(StringCollection.EXAMPLESZENE);
            //lade ausgewählte Szene im hintergrund
            //spiel animation für szenenwechsel ab
            //überblende die musik
            //unload Menuszene
        }

        public void Exit() {
            print("Quitting the Game");
            Application.Quit();
        }
    }
}