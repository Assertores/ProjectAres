using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class Coop_Edit : MonoBehaviour, IGameMode {

        #region Variables

        [SerializeField] GameObject m_EditingHUD;

        #endregion
        #region MonoBehaviour

        void Start() {
            if(!m_EditingHUD || !m_EditingHUD.GetComponent<EditorHUDAndPlayerLogic>()) {
                Destroy(this);
                return;
            }
            gameObject.SetActive(false);
        }

        #endregion
        #region IGameMode

        public void Init() {
            foreach(var it in Player.s_references) {
                it.EditAble(Instantiate(m_EditingHUD, it.transform).GetComponent<EditorHUDAndPlayerLogic>());
                it.Respawn(PlayerStart.s_references[Random.Range(0, PlayerStart.s_references.Count - 1)].transform.position);
            }

            gameObject.SetActive(true);
        }

        public void Stop() {
            foreach (var it in Player.s_references) {
                it.EditAble(null);
            }
            gameObject.SetActive(false);
        }

        public void PlayerDied(Player player) {
            player.Respawn(PlayerStart.s_references[Random.Range(0, PlayerStart.s_references.Count - 1)].transform.position);
            
        }

        #endregion

        public void SaveAndExit() {

            //maps abspeichern
            //map unter anderem namen kopieren DataHolder.s_maps[DataHolder.s_map];
            //props index listen lehren
            //props aus aktueller szene rein schreiben
            Stop();
            SceneManager.LoadScene(StringCollection.MAINMENU);
        }
    }
}