using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class TDM_Tournamant : Sauerbraten, IGameMode {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_specifics;
        [SerializeField] Sprite m_icon_;
        [SerializeField] string m_text_;
        [Header("Balancing")]
        [SerializeField] int m_teamCount = 2;
        [SerializeField] int m_teamLives = 5;
        [SerializeField] float m_respawnTime = 2.0f;
        [SerializeField] float m_minDelayToGame = 1;
        [SerializeField] float m_laserSwitchTime = 25f;

        List<int> m_lives = new List<int>();

        float m_specificStartTime;
        TDMHudRefHolder m_tdmhrh = null;

        #endregion
        #region MonoBehaviour

        void Awake() {
            if (!m_specifics) {
                print("no specific prefab");
                Destroy(this);
                return;
            }
            if (!m_specifics.GetComponent<TDMHudRefHolder>()) {
                print("specifics has no TDMHudRefHolder");
                Destroy(this);
                return;
            }
        }

        void Start() {

            DataHolder.s_gameModes[e_gameMode.TDM_TOURNAMENT] = this;
        }

        #endregion
        //#region IGameMode

        //public void Init() {
        //    m_lives.Clear();
        //    for (int i = 0; i < m_teamCount; i++) {
        //        m_lives.Add(m_teamLives);
        //    }

        //    for (int i = 0; i < Player.s_references.Count; i++) {
        //        Player.s_references[i].m_team = i % m_teamCount;
        //        Player.s_references[i].m_stats.m_points = m_teamLives;
        //        DoRespawn(Player.s_references[i]);
        //    }


        //    gameObject.SetActive(true);
        //}

        //public void Stop() {
        //    gameObject?.SetActive(false);
        //}

        //public void PlayerDied(Player player) {
        //    if(player.m_team >= 0 && player.m_team < m_lives.Count) {
        //        m_lives[player.m_team]--;

        //        List<Player> tmp = Player.s_references.FindAll(x => x.m_team == player.m_team);
        //        foreach (var it in tmp) {
        //            it.m_stats.m_points = m_lives[player.m_team];
        //        }

        //        if (m_lives[player.m_team] <= 0) {
        //            SceneManager.LoadScene(StringCollection.ENDSCREEN);
        //            return;
        //        }

        //    }

        //    Player.s_sortedRef.Sort(delegate (Player lhs, Player rhs) { return rhs.m_stats.m_points.CompareTo(lhs.m_stats.m_points); });

        //    DoRespawn(player);
        //}

        //public void SetMenuSpecific(Transform specificRef) {
        //    m_tdmhrh = Instantiate(m_specifics, specificRef).GetComponent<TDMHudRefHolder>();
        //    m_specificStartTime = Time.time;
        //}

        //public bool ReadyToChange() {
        //    if (m_specificStartTime + m_minDelayToGame > Time.time) {
        //        return false;
        //    }
        //    if (m_tdmhrh.m_teams[0].Count + m_tdmhrh.m_teams[1].Count < Player.s_references.Count) {
        //        return false;
        //    }
        //    if (Mathf.Abs(m_tdmhrh.m_teams[0].Count - m_tdmhrh.m_teams[1].Count) > 1) {
        //        return false;
        //    }

        //    foreach (var it in Player.s_references) {
        //        it.DoReset();
        //    }
        //    return true;
        //    //return m_specificStartTime + m_minDelayToGame > Time.time && m_tdmhrh.m_teams[0].Count + m_tdmhrh.m_teams[1].Count >= Player.s_references.Count && Mathf.Abs(m_tdmhrh.m_teams[0].Count - m_tdmhrh.m_teams[1].Count) <= 1;
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
            StartCoroutine(LaserHandler());
        }

        public void PlayerDied(Player player) {
            RespawnPlayer(player);
        }

        public void EndGame() {
            StopAllCoroutines();
        }

        public bool ReadyToChange() {
            return true;
        }

        #endregion

        void RespawnPlayer(Player player) {
            List<PlayerStart> availableSpawns = PlayerStart.s_references.FindAll(x => x.m_team == player.m_team);

            if (availableSpawns.Count == 0) {
                StartCoroutine(player.Respawn(PlayerStart.s_references[Random.Range(0, PlayerStart.s_references.Count)].transform.position, m_respawnTime));
            } else {
                StartCoroutine(player.Respawn(availableSpawns[Random.Range(0, availableSpawns.Count)].transform.position, m_respawnTime));
            }
        }

        IEnumerator LaserHandler() {
            while (true) {
                yield return new WaitForSeconds(m_laserSwitchTime);
                StartCoroutine(LaserBehaviour.s_singelton.ChangePosition());
            }
        }
    }
}