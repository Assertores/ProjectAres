using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {

    public class TeamSelectManager : MonoBehaviour {

        [Header("Balancing")]
        [SerializeField] float m_delay;

        Dictionary<Player, int> m_teams = new Dictionary<Player, int>();
        int m_playerCount = 0;
        float m_startTime;

        private void Update() {
            if(m_playerCount >= Player.s_references.Count) {
                if(Time.time - m_startTime < m_delay) {
                    //TODO: doCountDown
                } else {
                    if(MatchManager.s_currentMatch.m_teamHolder != null && MatchManager.s_currentMatch.m_teamHolder.Count == 0) {
                        MatchManager.s_currentMatch.m_teamHolder = m_teams;
                    } else {
                        bool sameTeams = true;

                        if (MatchManager.s_currentMatch.m_teamHolder.Count != m_teams.Count)
                            sameTeams = false;
                        else {
                            foreach (var it in MatchManager.s_currentMatch.m_teamHolder) {
                                if (it.Value != m_teams[it.Key])
                                    sameTeams = false;
                            }
                        }

                        if (!sameTeams)
                            MatchManager.s_currentMatch.m_teamHolder = null;
                    }
                    
                    MatchManager.s_currentMatch.ContinueToMap();
                }
            }
        }

        public void AddToFirstTeam() {
            m_teams[TriggerButton.s_hoPlayer] = 0;
            m_playerCount++;
            if(m_playerCount >= Player.s_references.Count) {
                m_startTime = Time.time;
            }
        }

        public void AddToSecondTeam() {
            m_teams[TriggerButton.s_hoPlayer] = 1;
            m_playerCount++;
            if (m_playerCount >= Player.s_references.Count) {
                m_startTime = Time.time;
            }
        }

        public void RemoveOfTeam() {
            m_teams[TriggerButton.s_hoPlayer] = -1;
            m_playerCount--;
            m_startTime = float.MaxValue;
        }
    }
}