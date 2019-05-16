﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

namespace ProjectAres {
    public class Bracket_Fair_Tournament : MonoBehaviour {
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
        [SerializeField] GameObject m_backToMM;
        [SerializeField] GameObject m_spawnHandler;

        [Header("Balancing")]
        [SerializeField] float m_winScreenMaxTime = 4;

        float m_pillarSpeed = 1;
        float m_hightPerKill = 1;
        List<PillarRefHolder> m_pillar = new List<PillarRefHolder>();
        float m_startTime;

        int m_playerCount;

        bool m_endet = false;

        List<Player> m_sorted = new List<Player>();

        #endregion
        #region MonoBehaviour

        void Start() {
            if(DataHolder.s_gameMode != e_gameMode.FAIR_TOURNAMENT || DataHolder.s_firstMatch) {//first match wurde ende GameMode schon hoch gezählt
                Destroy(this);
                return;
            }
            if (!m_pillarRef.GetComponent<PillarRefHolder>()) {
                print("no pillar ref on the Prefab");
                return;
            }

            m_backToMM.SetActive(false);

            m_sorted.AddRange(Player.s_references);
            m_sorted.Sort(delegate (Player lhs, Player rhs) { return rhs.m_stats.m_kills - lhs.m_stats.m_kills; });

            int maxKills = 0;
            for (int i = 0; i < Player.s_references.Count; i++) {

                Player.s_references[i].Invincible(true);

                if (!Player.s_references[i].m_alive) {
                    Player.s_references[i].Respawn(transform.position);
                }
                Player.s_references[i].DoReset();

                if (Player.s_references[i].m_stats.m_kills > maxKills)
                    maxKills = Player.s_references[i].m_stats.m_kills;

                Player.s_references[i].transform.position = Vector3.Lerp(m_leftMostPlayer.position, m_rightMostPlayer.position, ((float)i + 1) / (Player.s_references.Count + 1));
                Player.s_references[i].InControle(false);
                m_pillar.Add(Instantiate(m_pillarRef, Player.s_references[i].transform.position, Player.s_references[i].transform.rotation).GetComponent<PillarRefHolder>());

                //----- ----- FeedBack ----- -----
                m_pillar[i].m_screen.text = "0";
                m_pillar[i].m_pillarGradient.gameObject.SetActive(false);
                m_pillar[i].m_pillarField.text = "";
                m_pillar[i].m_playerName.text = Player.s_references[i].m_stats.m_name;
            }

            m_pillarSpeed = (m_maxHeight.position.y - m_rightMostPlayer.position.y) / m_winScreenMaxTime;
            m_hightPerKill = (m_maxHeight.position.y - m_rightMostPlayer.position.y) / maxKills;
            m_startTime = Time.timeSinceLevelLoad;

            m_playerCount = Player.s_references.Count;
        }

        // Update is called once per frame
        void Update() {
            if(Time.timeSinceLevelLoad - m_startTime <= m_winScreenMaxTime) {
                for (int i = 0; i < Player.s_references.Count; i++) {
                    if (m_pillar[i].gameObject.transform.position.y < m_leftMostPlayer.position.y + (Player.s_references[i].m_stats.m_kills * m_hightPerKill)) {
                        m_pillar[i].m_screen.text = Mathf.RoundToInt((m_pillar[i].gameObject.transform.position.y - m_leftMostPlayer.position.y) / m_hightPerKill).ToString();
                        m_pillar[i].gameObject.transform.position += new Vector3(0, m_pillarSpeed * Time.deltaTime, 0);

                        Player.s_references[i].transform.position = m_pillar[i].gameObject.transform.position;
                    } else {
                        int index = m_sorted.IndexOf(Player.s_references[i]);
                        m_pillar[i].m_pillarField.text = ((index+1).ToString() + ". Platz");//TODO: Lokalisierung, Bracket anstadt finale
                        Color c = Color.green;
                        c.a = m_pillar[i].m_pillarGradient.color.a;
                        m_pillar[i].m_pillarGradient.color = c;
                        m_pillar[i].m_pillarGradient.gameObject.SetActive(true);
                        Player.s_references[i].InControle(true);
                        if ((DataHolder.s_winnerPC && index >= m_playerCount / 2) ||(!DataHolder.s_winnerPC && index < m_playerCount / 2)) {
                            m_pillar[i].m_changePcText.text = "Please Change the Pc";
                            c = Color.red;
                            c.a = m_pillar[i].m_pillarGradient.color.a;
                            m_pillar[i].m_pillarGradient.color = c;
                        }
                    }
                }
            } else if(!m_endet) {
                m_endet = true;
                List<Player> disconect = new List<Player>();
                if (DataHolder.s_winnerPC) {
                    for(int i = m_sorted.Count / 2; i < m_sorted.Count; i++) {
                        disconect.Add(m_sorted[i]);
                    }
                } else {
                    for(int i = 0; i < m_sorted.Count/2; i++) {
                        disconect.Add(m_sorted[i]);
                    }
                }
                foreach(var it in disconect) {
                    it.m_control.DoDisconect();
                }

                m_spawnHandler.SetActive(true);
                
            }
            if (m_endet && Player.s_references.Count >= m_playerCount) {
                m_backToMM.SetActive(true);
            }
        }

        #endregion
    }
}