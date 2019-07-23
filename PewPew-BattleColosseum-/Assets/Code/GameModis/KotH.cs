using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class KotH : MonoBehaviour, IGameMode {

        #region Varialbes

        [Header("References")]
        [SerializeField] Sprite r_icon_;
        [SerializeField] string r_text_;
        [SerializeField] GameObject p_hill;
        [SerializeField] GameObject p_valley;

        [Header("Balancing")]
        [SerializeField] int m_PointsToWin = 50;
        [SerializeField] float m_respawnDelay = 2;
        [SerializeField] int m_valleyCount = 1;
        [SerializeField] float m_zoneMoveDelay = 20;
        [SerializeField] float m_zoneTimeInBetween = 2;
        [SerializeField] float m_pointGainPerSeconds = 2;
        [SerializeField] float m_pointLossPerSeconds = 1;
        [SerializeField] float m_hillDiameter = 5;
        [SerializeField] float m_valleyDiameter = 2;
        [SerializeField] float m_timeThreshold = 1;

        #endregion
        #region MonoBehaviour



        #endregion
        #region IGameMode

        public Sprite m_icon => r_icon_;

        public string m_name => StringCollection.M_KOTH;

        public string m_text => r_text_;

        public bool m_isTeamMode => false;

        public System.Action<bool> EndGame { get; set; }

        public void AbortGame() {
            EndGame(false);
        }

        public void DoEndGame() {
            EndGame(true);
        }

        public e_mileStones[] GetMileStones() {
            e_mileStones[] value = new e_mileStones[Player.s_references.Count];
            //impliment milestones
            return value;
        }

        public void PlayerDied(IHarmingObject killer, Player victim) {
            victim.Respawn(SpawnPoint.s_references[Random.Range(0, SpawnPoint.s_references.Count)].transform.position, m_respawnDelay);
        }

        public void ScorePoint(Player scorer, float amount) {
            scorer.m_stats.m_points += amount;

            if (scorer.m_stats.m_points < 0)
                scorer.m_stats.m_points = 0;
            
            if(scorer.m_stats.m_points >= m_PointsToWin) {
                EndGame?.Invoke(true);
            }

            Player.s_sortRef.Sort(delegate (Player lhs, Player rhs) {
                if (lhs.m_stats.m_points != rhs.m_stats.m_points) {
                    return rhs.m_stats.m_points.CompareTo(lhs.m_stats.m_points);
                }
                if(lhs.m_stats.m_kills != rhs.m_stats.m_kills) {
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
        }

        public void StartTransition() {
            //Player transition
        }

        public void SetUpGame() {
            foreach (var it in Player.s_references) {
                it.Respawn(SpawnPoint.s_references[Random.Range(0, SpawnPoint.s_references.Count)].transform.position);
            }
        }

        public void StartGame() {
            Instantiate(p_hill).GetComponent<Zone>().Init(this, m_zoneMoveDelay, m_zoneTimeInBetween, m_pointGainPerSeconds, m_hillDiameter, m_timeThreshold);
            for(int i = 0; i < m_valleyCount; i++) {
                Instantiate(p_valley).GetComponent<Zone>().Init(this, m_zoneMoveDelay, m_zoneTimeInBetween, -m_pointLossPerSeconds, m_valleyDiameter, m_timeThreshold);
            }
        }

        #endregion
    }
}