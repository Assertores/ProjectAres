using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class TDM_Tournamant : MonoBehaviour, IGameMode {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_specifics;
        [Header("Balancing")]
        [SerializeField] int m_teamCount = 2;
        [SerializeField] int m_teamLives = 5;
        [SerializeField] float m_respawnTime = 2.0f;

        List<int> m_lives = new List<int>();

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

            Stop();

            DataHolder.s_gameModes[e_gameMode.TDM_TOURNAMENT] = this;
        }

        #endregion
        #region IGameMode

        public void Init() {
            m_lives.Clear();
            for (int i = 0; i < m_teamCount; i++) {
                m_lives.Add(m_teamLives);
            }

            for (int i = 0; i < Player.s_references.Count; i++) {
                Player.s_references[i].m_team = i % m_teamCount;
                Player.s_references[i].m_stats.m_points = m_teamLives;
                DoRespawn(Player.s_references[i]);
            }
            

            gameObject.SetActive(true);
        }

        public void Stop() {
            gameObject.SetActive(false);
        }

        public void PlayerDied(Player player) {
            if(player.m_team >= 0 && player.m_team < m_lives.Count) {
                m_lives[player.m_team]--;

                List<Player> tmp = Player.s_references.FindAll(x => x.m_team == player.m_team);
                foreach (var it in tmp) {
                    it.m_stats.m_points = m_lives[player.m_team];
                }

                if (m_lives[player.m_team] <= 0) {
                    SceneManager.LoadScene(StringCollection.ENDSCREEN);
                    return;
                }
                
            }

            StartCoroutine(RespawnPlayer(player));
        }

        public void SetMenuSpecific(Transform specificRef) {
            m_tdmhrh = Instantiate(m_specifics, specificRef).GetComponent<TDMHudRefHolder>();
        }

        public bool ReadyToChange() {
            return m_tdmhrh.m_teams[0].Count + m_tdmhrh.m_teams[1].Count >= Player.s_references.Count;
        }

        #endregion

        IEnumerator RespawnPlayer(Player player) {
            yield return new WaitForSeconds(m_respawnTime);

            DoRespawn(player);
        }

        void DoRespawn(Player player) {
            List<PlayerStart> availableSpawns = PlayerStart.s_references.FindAll(x => x.m_team == player.m_team);

            if (availableSpawns.Count == 0) {
                player.Respawn(PlayerStart.s_references[Random.Range(0, PlayerStart.s_references.Count - 1)].transform.position);
            } else {
                player.Respawn(availableSpawns[Random.Range(0, availableSpawns.Count - 1)].transform.position);
            }
        }
    }
}