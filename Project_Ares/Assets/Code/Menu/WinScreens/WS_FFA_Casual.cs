using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

namespace ProjectAres {
    public class WS_FFA_Casual : WinScreen {

        #region Variables

        struct d_pillar {
            public GameObject pillar;
            public TextMeshProUGUI text;
        }

        [Header("References")]
        [SerializeField] GameObject m_pillarRef;
        [SerializeField] Transform m_rightMostPlayer;
        [SerializeField] Transform m_leftMostPlayer;
        [SerializeField] Transform m_maxHeight;

        [Header("Balancing")]
        [SerializeField] float m_winScreenMaxTime = 4;

        float m_pillarSpeed = 1;
        float m_hightPerKill = 1;
        List<PillarRefHolder> m_pillar = new List<PillarRefHolder>();
        
        float m_startTime;

        #endregion
        #region WinScreen
        #region MonoBehaviour

        void Start() {
            foreach (var it in Player.s_references) {
                it.SetStatsAble(true);
            }
            if (DataHolder.s_gameMode != e_gameMode.FFA_CASUAL) {
            Destroy(this);
            return;
            }
            if (!m_pillarRef.GetComponent<PillarRefHolder>()) {
                print("no pillar ref on the Prefab");
                return;
            }
            
            
            
            int maxKills = 0;
            for (int i = 0; i < Player.s_references.Count; i++) {

                Player.s_references[i].Invincible(true);

                if (!Player.s_references[i].m_alive) {
                    Player.s_references[i].Respawn(transform.position);
                }
                Player.s_references[i].DoReset();

                if (Player.s_references[i].m_stats.m_kills > maxKills)
                    maxKills = Player.s_references[i].m_stats.m_kills;

                Player.s_references[i].transform.position = Vector3.Lerp(m_leftMostPlayer.position, m_rightMostPlayer.position, ((float)i+1) /(Player.s_references.Count+1));
                Player.s_references[i].InControle(false);
                m_pillar.Add(Instantiate(m_pillarRef, Player.s_references[i].transform.position, Player.s_references[i].transform.rotation).GetComponent<PillarRefHolder>());
                //m_pStats.Add(m_statsRef, )

                //----- ----- FeedBack ----- -----
                
                m_pillar[i].m_screen.text = "0";
                m_pillar[i].m_playerName.text = Player.s_references[i].m_stats.m_name;
            }

            m_pillarSpeed = (m_maxHeight.position.y - m_rightMostPlayer.position.y) / m_winScreenMaxTime;
            m_hightPerKill = (m_maxHeight.position.y - m_rightMostPlayer.position.y) / maxKills;
            m_startTime = Time.timeSinceLevelLoad;
        }

        // Update is called once per frame
        void Update() {

            for(int i = 0; i < Player.s_references.Count; i++) {
                if(m_pillar[i].gameObject.transform.position.y < m_leftMostPlayer.position.y + (Player.s_references[i].m_stats.m_kills * m_hightPerKill)) {
                    m_pillar[i].m_screen.text = Mathf.RoundToInt((m_pillar[i].gameObject.transform.position.y - m_leftMostPlayer.position.y) / m_hightPerKill).ToString();
                    m_pillar[i].gameObject.transform.position += new Vector3(0,m_pillarSpeed * Time.deltaTime,0);

                    Player.s_references[i].transform.position = m_pillar[i].gameObject.transform.position;
                } else {
                    Player.s_references[i].InControle(true);
                }
            }
        }

        private void OnDestroy() {
            foreach(var it in Player.s_references){
                it.SetStatsAble(false);
            }
        }

        #endregion
        #endregion
    }
}