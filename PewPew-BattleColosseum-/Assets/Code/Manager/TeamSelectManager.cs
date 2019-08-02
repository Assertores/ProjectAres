using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {

    public class TeamSelectManager : MonoBehaviour {

        [Header("Balancing")]
        [SerializeField] float m_delay;
        
        int m_playerCount = 0;
        float m_startTime;
        bool m_finished = false;

        private void Update() {
            if(!m_finished && m_playerCount >= Player.s_references.Count && (Player.s_references.Find(x => x.m_team == 0) && Player.s_references.Find(x => x.m_team == 1))) {
                if(Time.time - m_startTime < m_delay) {
                    //TODO: doCountDown
                } else {
                    Dictionary<Player, int> m_teams = new Dictionary<Player, int>();
                    foreach(var it in Player.s_references) {
                        m_teams[it] = it.m_team;
                    }
                    if (MatchManager.s_currentMatch.m_teamHolder != null && MatchManager.s_currentMatch.m_teamHolder.Count == 0) {
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
                    m_finished = true;
                }
            }
        }

        public void AddToFirstTeam() {
            TriggerButton.s_hoPlayer.m_team = 0;
            m_playerCount++;
            if(m_playerCount >= Player.s_references.Count) {
                m_startTime = Time.time;
            }
        }

        public void AddToSecondTeam() {
            TriggerButton.s_hoPlayer.m_team = 1;
            m_playerCount++;
            if (m_playerCount >= Player.s_references.Count) {
                m_startTime = Time.time;
            }
        }

        public void RemoveOfTeam() {
            TriggerButton.s_hoPlayer.m_team = -1;
            m_playerCount--;
            m_startTime = float.MaxValue;
        }
    }
}