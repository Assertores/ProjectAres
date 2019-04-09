using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class GameManager : MonoBehaviour {

        [Header("References")]
        [SerializeField] GameObject m_gmObject;
        [SerializeField] GameObject m_playerRef;

        IGameMode m_gameMode;

        #region Singelton

        static GameManager s_singelton_ = null;
        public static GameManager _singelton  {
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

        #endregion

        void Start() {
            m_gameMode = m_gmObject.GetComponent<IGameMode>();//Interface werden nicht im inspector angezeigt
            m_gameMode?.Init();//TODO: wie bekommt er den richtigen GameMode aus dem Menü

            if(Player.s_references.Count == 0) {
                GameObject tmp = Instantiate(m_playerRef);
                if (tmp) {
                    GameObject tmpControle = new GameObject("Controler");
                    tmpControle.transform.parent = tmp.transform;

                    IControl reference = tmpControle.AddComponent<KeyboardControl>();//null reference checks
                    tmp.GetComponent<Player>().Init(reference);//null reference checks
                }
            }
        }

        public void Init(IGameMode mode) {
            m_gameMode?.Stop();
            m_gameMode = mode;
            mode.Init();
            foreach(var it in Player.s_references) {
                it.m_stats.m_assists = 0;
                it.m_stats.m_damageDealt = 0;
                it.m_stats.m_damageTaken = 0;
                it.m_stats.m_deaths = 0;
                it.m_stats.m_kills = 0;
            }
        }

        void Update() {

        }

        public void PlayerDied(Player player) {
            m_gameMode.PlayerDied(player);
        }
    }
}