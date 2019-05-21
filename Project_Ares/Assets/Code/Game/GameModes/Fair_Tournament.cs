using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class Fair_Tournament : MonoBehaviour, IGameMode {

        #region Variables

        [Header("References")]
        [SerializeField] Transform m_respawnParent;

        [Header("Balancing")]
        [SerializeField] int m_maxKills = 8;
        [SerializeField] float m_respawnTime = 2.0f;

        float m_startTime;

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
            m_startTime = Time.timeSinceLevelLoad;
            foreach (var it in Player.s_references) {
                it.Respawn(m_respawnParent.GetChild(Random.Range(0, m_respawnParent.childCount)).position);
            }
            gameObject.SetActive(true);
        }

        public void Stop() {
            gameObject.SetActive(false);
        }

        public void PlayerDied(Player player) {
            int max = 0;
            foreach(var it in Player.s_references) {
                if(max < it.m_stats.m_kills)
                    max = it.m_stats.m_kills;
            }
            if(max >= m_maxKills) {
                //disconecte die schlechtesten zwei/die besten zwei
                
                DataHolder.s_firstMatch = !DataHolder.s_firstMatch;

                SceneManager.LoadScene(StringCollection.FFACASUAL);
                return;
            }

            //----- ----- sorting players ----- -----
            Player.s_sortedRef.Sort(delegate (Player lhs, Player rhs) {
                if(lhs.m_stats.m_kills != rhs.m_stats.m_kills) {
                    return rhs.m_stats.m_kills.CompareTo(lhs.m_stats.m_kills);
                }
                if(lhs.m_stats.m_assists != rhs.m_stats.m_assists) {
                    return rhs.m_stats.m_assists.CompareTo(lhs.m_stats.m_assists);
                }
                if(lhs.m_stats.m_deaths != rhs.m_stats.m_deaths) {
                    return rhs.m_stats.m_deaths.CompareTo(lhs.m_stats.m_deaths);
                }
                if(lhs.m_stats.m_damageDealt != rhs.m_stats.m_damageDealt) {
                    return rhs.m_stats.m_damageDealt.CompareTo(lhs.m_stats.m_damageDealt);
                }
                if(lhs.m_stats.m_damageTaken != rhs.m_stats.m_damageTaken) {
                    return rhs.m_stats.m_damageTaken.CompareTo(lhs.m_stats.m_damageTaken);
                }
                if(lhs.GetHealth() != rhs.GetHealth()) {
                    return rhs.GetHealth().CompareTo(lhs.GetHealth());
                }
                return 0;
                });

            StartCoroutine(RespawnPlayer(player));
        }

        #endregion

        IEnumerator RespawnPlayer(Player player) {
            yield return new WaitForSeconds(m_respawnTime);

            player.Respawn(m_respawnParent.GetChild(Random.Range(0, m_respawnParent.childCount)).position);

        }
    }
}