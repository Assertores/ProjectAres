﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class FFA_Casual : MonoBehaviour, IGameMode {

        #region Variables

        [Header("Balancing")]
        [SerializeField] float m_gameTime = 120.0f;
        [SerializeField] float m_respawnTime = 2.0f;

        float m_startTime;

        #endregion
        #region MonoBehaviour

        void Start() {
            Stop();

            DataHolder.s_gameModes[e_gameMode.FFA_CASUAL] = this;
        }

        void Update() {
            if (m_gameTime <=  Time.timeSinceLevelLoad - m_startTime) {
                SceneManager.LoadScene(StringCollection.ENDSCREEN);
            }
        }

        #endregion
        #region IGameMode

        public void Init() {
            m_startTime = Time.timeSinceLevelLoad;
            foreach (var it in Player.s_references) {
                it.Respawn(PlayerStart.s_references[Random.Range(0, PlayerStart.s_references.Count-1)].transform.position);
            }
            gameObject.SetActive(true);
        }

        public void Stop() {
            gameObject.SetActive(false);
        }

        public void PlayerDied(Player player) {

            foreach (var it in Player.s_sortedRef) {
                it.m_stats.m_points = it.m_stats.m_kills;
            }
            //----- ----- sorting players ----- -----
            Player.s_sortedRef.Sort(delegate (Player lhs, Player rhs) {
                if (lhs.m_stats.m_kills != rhs.m_stats.m_kills) {
                    return rhs.m_stats.m_kills.CompareTo(lhs.m_stats.m_kills);
                }
                if (lhs.m_stats.m_assists != rhs.m_stats.m_assists) {
                    return rhs.m_stats.m_assists.CompareTo(lhs.m_stats.m_assists);
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
                if (lhs.GetHealth() != rhs.GetHealth()) {
                    return rhs.GetHealth().CompareTo(lhs.GetHealth());
                }
                return 0;
            });

            StartCoroutine(RespawnPlayer(player));
        }

        public void SetMenuSpecific(Transform specificRef) {
        }

        public bool ReadyToChange() {
            return true;
        }

        #endregion

        IEnumerator RespawnPlayer(Player player) {
            yield return new WaitForSeconds(m_respawnTime);

            player.Respawn(PlayerStart.s_references[Random.Range(0, PlayerStart.s_references.Count - 1)].transform.position);

        }
    }
}