using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class d_data {
        public string m_realName;
        public string m_alias;
    }
    public class d_matchdata {
        public d_data m_player;
        public int m_placement;
    }
    public class d_match {
        public BracketRefHolder reference;
        public d_matchdata m_player1;
        public d_matchdata m_player2;
        public d_matchdata m_player3;
        public d_matchdata m_player4;

        public void Set(d_data data, int index) {
            switch (index) {
            case 0:
                m_player1.m_player = data;
                m_player1.m_placement = -1;
                break;
            case 1:
                m_player2.m_player = data;
                m_player2.m_placement = -1;
                break;
            case 2:
                m_player3.m_player = data;
                m_player3.m_placement = -1;
                break;
            case 3:
                m_player4.m_player = data;
                m_player4.m_placement = -1;
                break;
            default:
                break;
            }
        }
    }
    public class d_final {
        public BracketRefHolder reference;
        public d_matchdata m_player1;
        public d_matchdata m_player2;
    }

    public class Manager : MonoBehaviour {

        public static Manager s_reference = null;

        [SerializeField] GameObject m_BracketPrefab;
        [SerializeField] GameObject m_FinalePrefab;


        List<d_data> m_players = new List<d_data>();
        List<d_match> m_matches = new List<d_match>();
        d_final m_final = new d_final();

        void Awake() {
            if(s_reference != null && s_reference != this) {
                Destroy(this);
                return;
            }
            if (!m_BracketPrefab.GetComponent<BracketRefHolder>()) {
                Destroy(this);
                return;
            }
            if (!m_FinalePrefab.GetComponent<BracketRefHolder>()) {
                Destroy(this);
                return;
            }

            s_reference = this;
        }

        private void OnDestroy() {
            if(s_reference == this) {
                s_reference = null;
            }
        }

        // Update is called once per frame
        void Update() {

        }

        public void AddPlayer(string name, string alias) {
            d_data tmp = new d_data();
            tmp.m_realName = name;
            tmp.m_alias = alias;
            m_players.Add(tmp);
        }

        public void StartGame() {
            //----- shuffle -----
            d_data tmp;
            int index;
            for (int i = 0; i < m_players.Count; i++) {
                index = Random.Range(0, m_players.Count - 1);

                tmp = m_players[i];
                m_players[i] = m_players[index];
                m_players[index] = tmp;
            }

            //----- create Brackets -----
            int RHS;
            int LHS;
            RHS = Mathf.FloorToInt(m_players.Count / 2.0f);
            LHS = (int)Mathf.Ceil(m_players.Count / 2.0f);
            int startMatches = 0;

            if(RHS%4 == 0) {
                startMatches += RHS / 4;
            } else {
                startMatches += RHS / 3;
            }

            if (LHS % 4 == 0) {
                startMatches += LHS / 4;
            } else {
                startMatches += LHS / 3;
            }

            m_matches = new List<d_match>(2 * startMatches - 1);

            foreach(var it in m_matches) {
                it.reference = Instantiate(m_BracketPrefab).GetComponent<BracketRefHolder>();
            }

            //----- assign players -----

            int startMatchIndex = m_matches.Count - startMatches;

            for (int i = 0; i < m_players.Count; i++) {
                m_matches[startMatchIndex + i % startMatches].Set(m_players[i], i / startMatches);
            }
        }

        public void FinishGame(int index, int p1place, int p2place, int p3place, int p4place) {
            m_matches[index].m_player1.m_placement = p1place;
            m_matches[index].m_player2.m_placement = p2place;
            m_matches[index].m_player3.m_placement = p3place;
            m_matches[index].m_player4.m_placement = p4place;

            int newMatchIndex = (index + 1) / 2;
            if(index == 1) {
                if (p1place == 1) {
                    m_final.m_player1.m_player = m_matches[index].m_player1.m_player;
                } else if (p2place == 1) {
                    m_final.m_player1.m_player = m_matches[index].m_player2.m_player;
                } else if (p3place == 1) {
                    m_final.m_player1.m_player = m_matches[index].m_player3.m_player;
                } else {
                    m_final.m_player1.m_player = m_matches[index].m_player4.m_player;
                }
                m_final.m_player1.m_placement = -1;

                if (p1place == 2) {
                    m_matches[1].m_player1.m_player = m_matches[index].m_player1.m_player;
                } else if (p2place == 2) {
                    m_matches[1].m_player1.m_player = m_matches[index].m_player2.m_player;
                } else if (p3place == 2) {
                    m_matches[1].m_player1.m_player = m_matches[index].m_player3.m_player;
                } else {
                    m_matches[1].m_player1.m_player = m_matches[index].m_player4.m_player;
                }
                if (p1place == 3) {
                    m_matches[1].m_player2.m_player = m_matches[index].m_player1.m_player;
                } else if (p2place == 3) {
                    m_matches[1].m_player2.m_player = m_matches[index].m_player2.m_player;
                } else if (p3place == 3) {
                    m_matches[1].m_player2.m_player = m_matches[index].m_player3.m_player;
                } else {
                    m_matches[1].m_player2.m_player = m_matches[index].m_player4.m_player;
                }
                return;
            }else if(index == 2) {
                if (p1place == 1) {
                    m_final.m_player2.m_player = m_matches[index].m_player1.m_player;
                } else if (p2place == 1) {
                    m_final.m_player2.m_player = m_matches[index].m_player2.m_player;
                } else if (p3place == 1) {
                    m_final.m_player2.m_player = m_matches[index].m_player3.m_player;
                } else {
                    m_final.m_player2.m_player = m_matches[index].m_player4.m_player;
                }
                m_final.m_player2.m_placement = -1;

                if (p1place == 2) {
                    m_matches[1].m_player3.m_player = m_matches[index].m_player1.m_player;
                } else if (p2place == 2) {
                    m_matches[1].m_player3.m_player = m_matches[index].m_player2.m_player;
                } else if (p3place == 2) {
                    m_matches[1].m_player3.m_player = m_matches[index].m_player3.m_player;
                } else {
                    m_matches[1].m_player3.m_player = m_matches[index].m_player4.m_player;
                }
                if (p1place == 3) {
                    m_matches[1].m_player4.m_player = m_matches[index].m_player1.m_player;
                } else if (p2place == 3) {
                    m_matches[1].m_player4.m_player = m_matches[index].m_player2.m_player;
                } else if (p3place == 3) {
                    m_matches[1].m_player4.m_player = m_matches[index].m_player3.m_player;
                } else {
                    m_matches[1].m_player4.m_player = m_matches[index].m_player4.m_player;
                }
                return;
            }else if (index % 2 == 0) {
                if(p1place == 1) {
                    m_matches[newMatchIndex].m_player1.m_player = m_matches[index].m_player1.m_player;
                }else if(p2place == 1) {
                    m_matches[newMatchIndex].m_player1.m_player = m_matches[index].m_player2.m_player;
                } else if(p3place == 1) {
                    m_matches[newMatchIndex].m_player1.m_player = m_matches[index].m_player3.m_player;
                } else {
                    m_matches[newMatchIndex].m_player1.m_player = m_matches[index].m_player4.m_player;
                }
                if (p1place == 2) {
                    m_matches[newMatchIndex].m_player2.m_player = m_matches[index].m_player1.m_player;
                } else if (p2place == 2) {
                    m_matches[newMatchIndex].m_player2.m_player = m_matches[index].m_player2.m_player;
                } else if (p3place == 2) {
                    m_matches[newMatchIndex].m_player2.m_player = m_matches[index].m_player3.m_player;
                } else {
                    m_matches[newMatchIndex].m_player2.m_player = m_matches[index].m_player4.m_player;
                }
            } else {
                if (p1place == 1) {
                    m_matches[newMatchIndex].m_player3.m_player = m_matches[index].m_player1.m_player;
                } else if (p2place == 1) {
                    m_matches[newMatchIndex].m_player3.m_player = m_matches[index].m_player2.m_player;
                } else if (p3place == 1) {
                    m_matches[newMatchIndex].m_player3.m_player = m_matches[index].m_player3.m_player;
                } else {
                    m_matches[newMatchIndex].m_player3.m_player = m_matches[index].m_player4.m_player;
                }
                if (p1place == 2) {
                    m_matches[newMatchIndex].m_player4.m_player = m_matches[index].m_player1.m_player;
                } else if (p2place == 2) {
                    m_matches[newMatchIndex].m_player4.m_player = m_matches[index].m_player2.m_player;
                } else if (p3place == 2) {
                    m_matches[newMatchIndex].m_player4.m_player = m_matches[index].m_player3.m_player;
                } else {
                    m_matches[newMatchIndex].m_player4.m_player = m_matches[index].m_player4.m_player;
                }
            }
        }
    }
}