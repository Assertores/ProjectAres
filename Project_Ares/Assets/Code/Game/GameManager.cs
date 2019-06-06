using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {

    

    public class GameManager : MonoBehaviour {

        #region Variables

        [Header("References")]
        public MapHandler m_mapHandler;

        #endregion
        #region MonoBehaviour
        #region Singelton

        static GameManager s_singelton_ = null;
        public static GameManager s_singelton  {
            get {
                if (!s_singelton_)
                    s_singelton_ = new GameObject {
                        name = "GameManager"
                    }.AddComponent<GameManager>();
                return s_singelton_;
                }
            }

        void Awake() {
            if(s_singelton_ == null) {
                s_singelton_ = this;
            }else if (s_singelton_ != this) {
                Destroy(gameObject);
                return;
            }
        }

        void OnDestroy() {
            if (s_singelton_ == this)
                s_singelton_ = null;
        }

        #endregion

        void Start() {

            if(Player.s_references.Count == 0) {
                SceneManager.LoadScene(StringCollection.MAINMENU);
                return;
            }

            if (m_mapHandler)
                m_mapHandler.LoadCurrentMap();
            
            foreach (var it in Player.s_references) {
                it.m_stats.m_assists = 0;
                it.m_stats.m_damageDealt = 0;
                it.m_stats.m_damageTaken = 0;
                it.m_stats.m_deaths = 0;
                it.m_stats.m_kills = 0;

                it.DoReset();
                it.Invincible(false);
            }

            DataHolder.s_gameModes[DataHolder.s_gameMode].Init();

            foreach (var it in Player.s_references) {
                it.m_stats.m_timeInLobby = Time.time - it.m_joinTime;
            }
        }

        #endregion

        public void PlayerDied(Player player) {
            DataHolder.s_gameModes[DataHolder.s_gameMode].PlayerDied(player);
        }
    }
}