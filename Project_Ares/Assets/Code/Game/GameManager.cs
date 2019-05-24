using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class GameManager : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] BoxCollider2D m_cameraSize;
        [SerializeField] AudioSource m_backgroundAudio;

        [HideInInspector]
        public List<Transform> m_borders { get; private set; } = new List<Transform>();

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
            }

            LoadMap();

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

        void LoadMap() {
            MapDATA currentMap = DataHolder.s_maps[DataHolder.s_map];
            Instantiate(currentMap.p_background[currentMap.m_background],Vector3.zero, Quaternion.Euler(Vector3.zero));
            foreach(var it in currentMap.m_props) {
                GameObject tmp = Instantiate(currentMap.p_props[it.index], it.position, Quaternion.Euler(new Vector3(0, 0, it.rotation)));
                tmp.transform.localScale = it.scale;
            }
            foreach (var it in currentMap.m_stage) {
                GameObject tmp = Instantiate(currentMap.p_stage[it.index], it.position, Quaternion.Euler(new Vector3(0, 0, it.rotation)));
                tmp.transform.localScale = it.scale;
            }
            foreach (var it in currentMap.m_forground) {
                GameObject tmp = Instantiate(currentMap.p_forground[it.index], it.position, Quaternion.Euler(new Vector3(0, 0, it.rotation)));
                tmp.transform.localScale = it.scale;
            }
            if (currentMap.p_light.GetComponent<Light>()) {
                foreach (var it in currentMap.m_lights) {
                    GameObject tmp = Instantiate(currentMap.p_light, it.position, Quaternion.Euler(Vector3.zero));
                    tmp.GetComponent<Light>().color = it.color;
                }
            }
            for (int i = 0; i < currentMap.m_playerStarts.Length; i++) {
                GameObject tmp = new GameObject("PlayerSpawn " + i);
                tmp.transform.position = currentMap.m_playerStarts[i].position;
                tmp.AddComponent<PlayerStart>().team = currentMap.m_playerStarts[i].team;
            }
            for (int i = 0; i < currentMap.m_border.Length; i++) {
                GameObject tmp = Instantiate(currentMap.p_props[currentMap.m_border[i].index], currentMap.m_border[i].position, Quaternion.Euler(new Vector3(0, 0, currentMap.m_border[i].rotation)));
                tmp.transform.localScale = currentMap.m_border[i].scale;
                m_borders.Add(tmp.transform);
            }
            Instantiate(currentMap.p_laserBariar);

            if (m_backgroundAudio) {
                m_backgroundAudio.clip = currentMap.m_backgroundMusic;
                m_backgroundAudio.Play();
            }

            if (m_cameraSize) {
                m_cameraSize.size = currentMap.m_size;
            }
        }

        public void PlayerDied(Player player) {
            DataHolder.s_gameModes[DataHolder.s_gameMode].PlayerDied(player);
        }
    }
}