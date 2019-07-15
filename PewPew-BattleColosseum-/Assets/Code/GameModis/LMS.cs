using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class LMS : MonoBehaviour, IGameMode {

        #region Variables

        [Header("References")]
        [SerializeField] Sprite r_icon_;
        [SerializeField] string r_text_;

        [Header("Balancing")]
        [SerializeField] int m_lifes = 5;
        [SerializeField] float m_respawnDelay = 2;

        #endregion
        #region IGameMode

        public Sprite m_icon => r_icon_;

        public string m_name => StringCollection.M_LMS;

        public string m_text => r_text_;

        public bool m_isTeamMode => false;

        public System.Action<bool> EndGame { get; set; }

        public void StartTransition() {
            //Player transition
        }

        public void SetUpGame() {
            foreach (var it in Player.s_references) {
                it.Respawn(SpawnPoint.s_references[Random.Range(0, SpawnPoint.s_references.Count)].transform.position);
                it.m_stats.m_points = m_lifes;
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
            victim.m_stats.m_points--;

            Player.s_sortRef.Sort(delegate (Player lhs, Player rhs) {
                if (lhs.m_stats.m_points != rhs.m_stats.m_points) {
                    return rhs.m_stats.m_points.CompareTo(lhs.m_stats.m_points);
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

            if (victim.m_stats.m_points >= 0) {
                victim.Respawn(SpawnPoint.s_references[Random.Range(0, SpawnPoint.s_references.Count)].transform.position, m_respawnDelay);
                return;
            }
            if(Player.s_references.FindAll(x => x.m_alive).Count <= 1) {
                DoEndGame();
                return;
            }

            
        }

        public void ScorePoint(Player scorer) {
        }

        #endregion
    }
}