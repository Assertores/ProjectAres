using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class FFA : MonoBehaviour, IGameMode {

        #region Variables

        [Header("References")]
        [SerializeField] Sprite r_icon_;
        [SerializeField] string r_text_;

        [Header("Balancing")]
        [SerializeField] int m_killsToWin = 5;
        [SerializeField] float m_respawnDelay = 2;

        #endregion
        #region MonoBehaviour



        #endregion
        #region IGameMode

        public Sprite m_icon => r_icon_;

        public string m_name => StringCollection.M_FFA;

        public string m_text => r_text_;

        public bool m_isTeamMode => false;

        public System.Action<bool> EndGame { get; set; }

        public void StartTransition() {
            //Player transition
        }

        public void SetUpGame() {
            foreach (var it in Player.s_references) {
                it.Respawn(SpawnPoint.s_references[Random.Range(0, SpawnPoint.s_references.Count)].transform.position);
            }
        }

        public void StartGame() {
            
        }

        public void AbortGame() {
            EndGame?.Invoke(false);
        }

        public void DoEndGame() {
            EndGame?.Invoke(true);
        }

        public e_mileStones[] GetMileStones() {
            e_mileStones[] value = new e_mileStones[Player.s_references.Count];
            //impliment milestones
            return value;
        }

        public void PlayerDied(IHarmingObject killer, Player victim) {
            if (killer.m_owner)
                killer.m_owner.m_stats.m_points = killer.m_owner.m_stats.m_kills;

            Player.s_sortRef.Sort(delegate (Player lhs, Player rhs) {
                if (lhs.m_stats.m_kills != rhs.m_stats.m_kills) {
                    return rhs.m_stats.m_kills.CompareTo(lhs.m_stats.m_kills);
                }
                if (lhs.m_stats.m_deaths != rhs.m_stats.m_deaths) {
                    return rhs.m_stats.m_deaths.CompareTo(lhs.m_stats.m_deaths);
                }
                if (lhs.m_stats.m_damageDealt != rhs.m_stats.m_damageDealt) {
                    return rhs.m_stats.m_damageDealt.CompareTo(lhs.m_stats.m_damageDealt);
                }
                if (lhs.m_stats.m_damageTaken != rhs.m_stats.m_damageTaken) {
                    return rhs.m_stats.m_damageTaken.CompareTo(lhs.m_stats.m_damageTaken);
                }
                return 0;
            });

            if (Player.s_sortRef[0].m_stats.m_kills >= m_killsToWin) {
                DoEndGame();
                return;
            }

            victim.Respawn(SpawnPoint.s_references[Random.Range(0, SpawnPoint.s_references.Count)].transform.position, m_respawnDelay);
        }

        public void ScorePoint(Player scorer) {
        }

        #endregion
    }
}