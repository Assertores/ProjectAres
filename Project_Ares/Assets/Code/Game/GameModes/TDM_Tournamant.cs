using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class TDM_Tournamant : MonoBehaviour, IGameMode {

        #region Variables

        [Header("Balancing")]
        [SerializeField] int m_teamCount = 2;
        [SerializeField] int m_teamLives = 5;
        [SerializeField] float m_respawnTime = 2.0f;

        List<int> m_lives = new List<int>();

        #endregion
        #region MonoBehaviour

        void Start() {
            Stop();
        }
        
        void Update() {

        }

        #endregion
        #region IGameMode

        public void Init() {
            for (int i = 0; i < Player.s_references.Count; i++) {
                Player.s_references[i].m_team = i % m_teamCount;
                Player.s_references[i].m_stats.m_points = m_teamLives;
            }
            m_lives.Clear();
            for(int i = 0; i < m_teamCount; i++) {
                m_lives.Add(m_teamLives);
            }
        }

        public void Stop() {
            gameObject.SetActive(false);
        }

        public void PlayerDied(Player player) {
            if(player.m_team >= 0 && player.m_team < m_lives.Count) {
                m_lives[player.m_team]--;
                foreach(var it in Player.s_references.FindAll(x => x.m_team == player.m_team)) {
                    it.m_stats.m_points = m_lives[player.m_team];
                }
            }

            StartCoroutine(RespawnPlayer(player));
        }

        #endregion

        IEnumerator RespawnPlayer(Player player) {
            yield return new WaitForSeconds(m_respawnTime);

            List<PlayerStart> availableSpawns = PlayerStart.s_references.FindAll(x => x.m_team == player.m_team);
            if (availableSpawns.Count == 0) {
                player.Respawn(PlayerStart.s_references[Random.Range(0, PlayerStart.s_references.Count - 1)].transform.position);
            } else {
                player.Respawn(availableSpawns[Random.Range(0, availableSpawns.Count - 1)].transform.position);
            }
        }
    }
}