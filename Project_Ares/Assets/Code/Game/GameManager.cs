using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {

    [System.Serializable]
    struct d_gmObjectItem {
        public e_gameMode m_type;
        public GameObject m_value;
    }

    public class GameManager : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] BoxCollider2D m_cameraSize;
        [SerializeField] AudioSource m_backgroundAudio;
        [SerializeField] SpriteRenderer m_background;
        [SerializeField] Light m_directionalLight;
        [SerializeField] Transform m_levelHolder;
        [SerializeField] d_gmObjectItem[] m_gmObject;

        [HideInInspector]
        public List<Transform> m_borders { get; private set; } = new List<Transform>();

        Dictionary<e_gameMode, IGameMode> m_gameModes = new Dictionary<e_gameMode, IGameMode>();

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

            if (m_gmObject != null) {
                foreach (var it in m_gmObject) {
                    IGameMode tmp = it.m_value.GetComponent<IGameMode>();
                    if (tmp != null) {
                        m_gameModes[it.m_type] = tmp;
                        tmp.Stop();
                    }
                }
            }

            LoadMap();

            m_gameModes[DataHolder.s_gameMode].Init();
            foreach (var it in Player.s_references) {
                it.m_stats.m_assists = 0;
                it.m_stats.m_damageDealt = 0;
                it.m_stats.m_damageTaken = 0;
                it.m_stats.m_deaths = 0;
                it.m_stats.m_kills = 0;

                it.DoReset();
                it.Invincible(false);
            }

            foreach (var it in Player.s_references) {
                it.m_stats.m_timeInLobby = Time.time - it.m_joinTime;
            }
        }

        #endregion

        void LoadMap() {
            foreach(Transform it in m_levelHolder) {
                Destroy(it.gameObject);
            }

            MapDATA currentMap = DataHolder.s_maps[DataHolder.s_map];
            //GameObject a = Instantiate(currentMap.p_background[currentMap.m_background],Vector3.zero, Quaternion.Euler(Vector3.zero));
            m_background.sprite = currentMap.p_background[currentMap.m_background];
            m_directionalLight.color = currentMap.m_globalLight;
            foreach(var it in currentMap.m_props) {
                GameObject tmp = Instantiate(currentMap.p_props[it.index], it.position, Quaternion.Euler(new Vector3(0, 0, it.rotation)));
                tmp.transform.localScale = new Vector3(it.scale.x,it.scale.y,1);
                tmp.transform.parent = m_levelHolder;
            }
            for(int i = 0; i < currentMap.m_stage.Length; i++) {
                GameObject tmp = new GameObject("Stage " + i);
                tmp.transform.position = currentMap.m_stage[i].position;
                tmp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentMap.m_stage[i].rotation));
                tmp.transform.localScale = new Vector3(currentMap.m_stage[i].scale.x, currentMap.m_stage[i].scale.y, 1);
                SpriteRenderer ren = tmp.AddComponent<SpriteRenderer>();
                ren.sprite = currentMap.p_stage[currentMap.m_stage[i].index];
                ren.sortingLayerName = StringCollection.STAGE;

                tmp.transform.parent = m_levelHolder;
            }
            for(int i = 0; i < currentMap.m_forground.Length; i++) {
                GameObject tmp = new GameObject("Forground " + i);
                tmp.transform.position = currentMap.m_forground[i].position;
                tmp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentMap.m_forground[i].rotation));
                tmp.transform.localScale = new Vector3(currentMap.m_forground[i].scale.x, currentMap.m_forground[i].scale.y,1);
                SpriteRenderer ren = tmp.AddComponent<SpriteRenderer>();
                ren.sprite = currentMap.p_stage[currentMap.m_forground[i].index];
                ren.sortingLayerName = StringCollection.FORGROUND;

                tmp.transform.parent = m_levelHolder;
            }
            if (currentMap.p_light.GetComponentInChildren<Light>()) {
                foreach (var it in currentMap.m_lights) {
                    GameObject tmp = Instantiate(currentMap.p_light, it.position, Quaternion.Euler(Vector3.zero));
                    tmp.GetComponentInChildren<Light>().color = it.color;
                    tmp.transform.parent = m_levelHolder;
                }
            }
            for (int i = 0; i < currentMap.m_playerStarts.Length; i++) {
                GameObject tmp = new GameObject("PlayerSpawn " + i);
                tmp.transform.position = currentMap.m_playerStarts[i].position;
                tmp.AddComponent<PlayerStart>().team = currentMap.m_playerStarts[i].team;
                tmp.transform.parent = m_levelHolder;
            }
            for (int i = 0; i < currentMap.m_border.Length; i++) {
                GameObject tmp = Instantiate(currentMap.p_props[currentMap.m_border[i].index], currentMap.m_border[i].position, Quaternion.Euler(new Vector3(0, 0, currentMap.m_border[i].rotation)));
                tmp.transform.localScale = new Vector3(currentMap.m_border[i].scale.x, currentMap.m_border[i].scale.y,1);
                tmp.transform.parent = m_levelHolder;
                m_borders.Add(tmp.transform);
            }
            GameObject a = Instantiate(currentMap.p_laserBariar);
            a.transform.parent = m_levelHolder;

            if (m_backgroundAudio) {
                m_backgroundAudio.clip = currentMap.m_backgroundMusic;
                m_backgroundAudio.Play();
            }

            if (m_cameraSize) {
                m_cameraSize.size = currentMap.m_size;
            }
        }

        public void PlayerDied(Player player) {
            m_gameModes[DataHolder.s_gameMode].PlayerDied(player);
        }
    }
}