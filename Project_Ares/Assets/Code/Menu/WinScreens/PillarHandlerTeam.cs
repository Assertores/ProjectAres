using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class PillarHandlerTeam : MonoBehaviour, IScriptQueueItem {

        #region Variables

        [SerializeField] float m_pillarWith = 2.29f;
        [SerializeField] float m_duration = 2f;
        [SerializeField] PillarRefHolder m_p1;
        [SerializeField] PillarRefHolder m_p2;

        float m_startTime;

        List<PillarRefHolder> m_pillars = new List<PillarRefHolder>();

        #endregion
        #region MonoBehaviour

        void Start() {

            if (DataHolder.s_gameMode == e_gameMode.TDM_TOURNAMENT)
                EndScreenManager.s_ref?.AddItem(this, 0);
            else {
                Destroy(this.gameObject);
            }
        }

        #endregion

        #region IScriptQueueItem

        public bool FirstTick() {
            int TeamCount = 0;
            foreach(var it in Player.s_references) {
                if (TeamCount < it.m_team)
                    TeamCount = it.m_team;
            }

            m_pillars.Add(m_p1);
            m_pillars.Add(m_p2);

            List<Player> players = new List<Player>();

            players = Player.s_references.FindAll(x => x.m_team == 0);
            Vector3 lmp = new Vector3(m_pillars[0].transform.position.x - m_pillarWith / 2, m_pillars[0].transform.position.y, m_pillars[0].transform.position.z);
            Vector3 rmp = new Vector3(m_pillars[0].transform.position.x + m_pillarWith / 2, m_pillars[0].transform.position.y, m_pillars[0].transform.position.z);
            for (int i = 0; i < players.Count; i++) {
                players[i].transform.position = Vector3.Lerp(lmp, rmp, ((float)i + 1) / (Player.s_references.Count + 1));
            }

            players = Player.s_references.FindAll(x => x.m_team == 1);
            lmp = new Vector3(m_pillars[1].transform.position.x - m_pillarWith / 2, m_pillars[1].transform.position.y, m_pillars[1].transform.position.z);
            rmp = new Vector3(m_pillars[1].transform.position.x + m_pillarWith / 2, m_pillars[1].transform.position.y, m_pillars[1].transform.position.z);
            for (int i = 0; i < players.Count; i++) {
                players[i].transform.position = Vector3.Lerp(lmp, rmp, ((float)i + 1) / (Player.s_references.Count + 1));
            }


            m_startTime = Time.time;
            return false;
        }

        public bool DoTick() {
            if (Time.time > m_startTime + m_duration)
                return true;
            //cooles feedback einfügen
            return false;
        }

        #endregion
    }
}