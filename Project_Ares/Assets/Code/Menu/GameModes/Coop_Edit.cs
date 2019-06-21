using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class Coop_Edit : Sauerbraten, IGameMode {

        #region Variables
        [Header("References")]
        [SerializeField] Sprite m_icon_;
        [SerializeField] string m_text_;
        [SerializeField] GameObject m_EditingHUD;

        #endregion
        #region MonoBehaviour

        void Start() {
            if(!m_EditingHUD || !m_EditingHUD.GetComponent<EditorHUDAndPlayerLogic>()) {
                Destroy(this);
                return;
            }
            gameObject.SetActive(false);

            foreach(Transform it in transform) {
                it.gameObject.SetActive(false);
            }

            DataHolder.s_gameModes[e_gameMode.COOP_EDIT] = this;
        }

        #endregion
        //#region IGameMode

        //public void Init() {
        //    foreach(var it in Player.s_references) {
        //        it.EditAble(Instantiate(m_EditingHUD, it.transform).GetComponent<EditorHUDAndPlayerLogic>());
        //        it.Respawn(PlayerStart.s_references[Random.Range(0, PlayerStart.s_references.Count)].transform.position);
        //    }

        //    foreach (Transform it in transform) {
        //        it.gameObject.SetActive(true);
        //    }

        //    gameObject.SetActive(true);
        //}

        //public void Stop() {
        //    foreach (var it in Player.s_references) {
        //        it.EditAble(null);
        //    }
        //    gameObject?.SetActive(false);
        //}

        //public void PlayerDied(Player player) {
        //    StartCoroutine(player.Respawn(PlayerStart.s_references[Random.Range(0, PlayerStart.s_references.Count)].transform.position));

        //}

        //public void SetMenuSpecific(Transform specificRef) {
        //}

        //public bool ReadyToChange() {
        //    foreach (var it in Player.s_references) {
        //        it.DoReset();
        //    }
        //    return true;
        //}

        //#endregion
        #region IGameMode

        public Sprite m_icon { get => m_icon_; set { } }
        public string m_text { get => m_text_; set { } }

        public void Unselect() {
        }

        public void Select() {
        }

        public void SetMenuSpecific(Transform specificRef) {
        }

        public void StartGame() {
            foreach (var it in Player.s_references) {
                it.EditAble(Instantiate(m_EditingHUD, it.transform).GetComponent<EditorHUDAndPlayerLogic>());
                it.Respawn(PlayerStart.s_references[Random.Range(0, PlayerStart.s_references.Count)].transform.position);
            }
        }

        public void PlayerDied(Player player) {
            StartCoroutine(player.Respawn(PlayerStart.s_references[Random.Range(0, PlayerStart.s_references.Count)].transform.position));
        }

        public void EndGame() {

            SceneManager.LoadScene(StringCollection.MAINMENU);
        }

        public bool ReadyToChange() {
            return true;
        }

        #endregion

        public void SaveAndExit() {
            GameManager.s_singelton.m_mapHandler.SaveMap(System.DateTime.Now.ToString());
            SceneManager.LoadScene(StringCollection.MAINMENU);
        }
    }
}