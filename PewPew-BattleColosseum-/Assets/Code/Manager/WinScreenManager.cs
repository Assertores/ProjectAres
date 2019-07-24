using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class WinScreenManager : MonoBehaviour {

        enum e_winScreenState { STARTUP, PILLARRISING, ANIMATIONS, LERPTOMATCH, MATCHPILLAR, SELECT, CONTINUE };

        struct d_pair<T> {
            public T first;
            public T second;
        }

        public static WinScreenManager s_singelton { get; private set; }

        #region Variables

        [Header("References")]
        [SerializeField] GameObject p_pillar;
        [SerializeField] Transform r_rightMostPlayer;
        [SerializeField] Transform r_leftMostPlayer;
        [SerializeField] Transform r_maxHeight;
        [SerializeField] GameObject r_selectParent;
        [SerializeField] GameObject p_fireworkParent;

        [Header("Balancing")]
        [SerializeField] float m_pillarRiseTime;
        [SerializeField] float m_GameMatchPillarLerpTime;
        [SerializeField] int m_upperMatchPoints = 200;
        [SerializeField] Color m_winColor = Color.green;
        [SerializeField] Color m_loseColor = Color.red;

        [SerializeField] int[] m_matchPoints;

        d_pair<int>[] m_nextGames = new d_pair<int>[3];

        float m_pillarSpeed = 1;
        float m_hightPerKill = 1;
        float m_startTime;

        float m_startLerpTime;

        e_winScreenState m_state = e_winScreenState.STARTUP;

        #endregion
        #region MonoBehaviour

        private void Awake() {
            if(s_singelton != null && s_singelton != this) {
                Destroy(this);
                return;
            }
            s_singelton = this;
        }

        private void OnDestroy() {
            if(s_singelton == this) {
                s_singelton = null;
            }
        }

        private void Start() {
            if (!p_pillar) {
                print("no pillar prefab set");
                Destroy(this);
                return;
            }
            if (!p_pillar.GetComponent<PillarRefHolder>()) {
                print("pillar has no ref holder");
                Destroy(this);
                return;
            }
            if (!r_selectParent) {
                print("selecter reference not set");
                Destroy(this);
                return;
            }

            r_selectParent.SetActive(false);

            Init();
        }

        private void Update() {
            switch (m_state) {
            case e_winScreenState.PILLARRISING:
                if (Time.time <= m_startTime + m_pillarRiseTime)
                    RisePillar();
                else {
                    m_state = e_winScreenState.ANIMATIONS;
                    foreach(var it in Player.s_references) {
                        it.r_pillar.m_holderPos = it.transform.position;
                    }
                    StartCoroutine(IEStartAnim());
                }
                break;
            case e_winScreenState.LERPTOMATCH:
                if (m_GameMatchPillarLerpTime < Time.time - m_startLerpTime) {
                    foreach (var it in Player.s_references) {
                        PositionPlayer(it, Vector3.Lerp(it.r_pillar.m_holderPos, it.r_pillar.m_middlePos, m_GameMatchPillarLerpTime / (Time.time - m_startLerpTime)), it.r_pillar.m_previousMatchPoints);
                    }
                } else {
                    foreach(var it in Player.s_references) {
                        it.r_pillar.r_points.text = it.r_pillar.m_previousMatchPoints.ToString();
                    }
                    m_state = e_winScreenState.MATCHPILLAR;
                    m_startLerpTime = Time.time;
                }
                break;
            case e_winScreenState.MATCHPILLAR://eventuell anders cooler
                if (m_GameMatchPillarLerpTime < Time.time - m_startLerpTime) {
                    foreach (var it in Player.s_references) {
                        PositionPlayer(it, Vector3.Lerp(it.r_pillar.m_holderPos, it.r_pillar.m_targetPos, m_GameMatchPillarLerpTime / (Time.time - m_startLerpTime)), Mathf.FloorToInt(Mathf.Lerp(it.r_pillar.m_previousMatchPoints, it.m_stats.m_matchPoints, m_GameMatchPillarLerpTime / (Time.time - m_startLerpTime))));
                    }
                } else {
                    foreach(var it in Player.s_references) {
                        it.InControle(true);
                    }
                    if(MatchManager.s_currentMatch.m_matchCount == 0) {
                        m_state = e_winScreenState.CONTINUE;
                        Continue();
                    } else {
                        m_state = e_winScreenState.SELECT;
                        InitSelect();
                    }
                }
                break;
            default:
                break;
            }
        }

        #endregion

        bool h_initOnce = false;
        public void Init() {
            if (h_initOnce)
                return;
            h_initOnce = true;

            if(MatchManager.s_currentMatch.m_teamHolder.Count == 0) {
                MatchManager.s_currentMatch.m_teamHolder = null;
            }

            m_pillarSpeed = (r_maxHeight.position.y - r_rightMostPlayer.position.y) / m_pillarRiseTime;
            m_hightPerKill = (r_maxHeight.position.y - r_rightMostPlayer.position.y) / Player.s_sortRef[0].m_stats.m_points;

            SpawnPillars();
        }

        void SpawnPillars() {
            for (int i = 0; i < Player.s_references.Count; i++) {
                GameObject pillar = Instantiate(p_pillar);
                Player.s_references[i].r_pillar = pillar.GetComponent<PillarRefHolder>();
                Player.s_references[i].SetPlayerActive(false);
                PositionPlayer(Player.s_references[i], Vector3.Lerp(r_leftMostPlayer.position, r_rightMostPlayer.position, Player.s_references.Count == 1 ? 0.5f : ((float)i) / (Player.s_references.Count - 1)),0);//1[0.5], 2[0, 1], 3[0, 0.5, 1], 4[0, 0.33, 0.66, 1], 5[0, 0.25, 0.5, 0.75, 1]
            }

            m_startTime = Time.time;

            m_state = e_winScreenState.PILLARRISING;
        }

        void RisePillar() {
            foreach(var it in Player.s_references) {
                if(m_hightPerKill * it.m_stats.m_points > it.transform.position.y - r_leftMostPlayer.position.y) {
                    PositionPlayer(it, it.transform.position + new Vector3(0, m_pillarSpeed * Time.deltaTime, 0), Mathf.FloorToInt((it.transform.position.y - r_leftMostPlayer.position.y) / m_hightPerKill));
                }
            }
        }

        void PositionPlayer(Player player, Vector3 position, int points) {
            player.transform.position = position;
            player.r_pillar.transform.position = position;
            player.r_pillar.transform.position -= new Vector3(0, player.m_distanceToGround, 0);
            player.r_pillar.r_points.text = points.ToString();
        }

        IEnumerator IEStartAnim() {
            float maxTime = 0;
            foreach(var it in Player.s_references) {
                float time;
                if(MatchManager.s_currentMatch.m_teamHolder != null ?
                    it.m_team == Player.s_sortRef[0].m_team :
                    it == Player.s_sortRef[0]) {

                    GameObject fwHolder = Instantiate(p_fireworkParent);
                    fwHolder.transform.position = new Vector2(it.r_pillar.transform.position.x, r_leftMostPlayer.position.y);
                    time = it.StartAnim(StringCollection.A_WIN);
                    it.r_pillar.r_light.color = m_winColor;
                } else {
                    time = it.StartAnim(StringCollection.A_LOSE);
                    it.r_pillar.r_light.color = m_loseColor;
                }
                it.r_pillar.r_light.gameObject.SetActive(true);

                if (time > maxTime)
                    maxTime = time;
            }
            
            yield return new WaitForSeconds(maxTime);

            if(MatchManager.s_currentMatch.m_matchCount == 0) {
                m_state = e_winScreenState.CONTINUE;
                Continue();
            } else {
                m_startLerpTime = Time.time;
                CalcMatchPoints();

                foreach(var it in Player.s_references) {
                    it.r_pillar.r_light.gameObject.SetActive(false);
                }

                m_state = e_winScreenState.LERPTOMATCH;
            }

            foreach(var it in Player.s_references) {
                it.SetPlayerActive(true);
            }
        }

        void CalcMatchPoints() {
            if (MatchManager.s_currentMatch.m_teamHolder != null) {
                for (int i = 0; i < Player.s_sortRef.Count; i++) {
                    Player.s_sortRef[i].r_pillar.m_previousMatchPoints = Player.s_sortRef[i].m_stats.m_matchPoints;

                    if(Player.s_sortRef[0].m_team == Player.s_sortRef[i].m_team) {
                        Player.s_sortRef[i].m_stats.m_matchPoints += m_matchPoints[0];
                    } else {
                        Player.s_sortRef[i].m_stats.m_matchPoints += m_matchPoints[1];
                    }

                    m_upperMatchPoints = Mathf.Max(m_upperMatchPoints, Player.s_sortRef[i].m_stats.m_matchPoints);
                }
            } else {
                for (int i = 0; i < Player.s_sortRef.Count; i++) {
                    Player.s_sortRef[i].r_pillar.m_previousMatchPoints = Player.s_sortRef[i].m_stats.m_matchPoints;

                    Player.s_sortRef[i].m_stats.m_matchPoints += m_matchPoints[i < m_matchPoints.Length ? i : m_matchPoints.Length -1];

                    m_upperMatchPoints = Mathf.Max(m_upperMatchPoints, Player.s_sortRef[i].m_stats.m_matchPoints);
                }
            }
            
            foreach(var it in Player.s_references) {
                it.r_pillar.m_targetPos = new Vector3(it.transform.position.x,
                                        r_leftMostPlayer.position.y + (r_maxHeight.position.y - r_leftMostPlayer.position.y) / m_upperMatchPoints * it.m_stats.m_matchPoints,
                                        r_leftMostPlayer.position.z);
                it.r_pillar.m_middlePos = new Vector3(it.transform.position.x,
                                        r_leftMostPlayer.position.y + (r_maxHeight.position.y - r_leftMostPlayer.position.y) / m_upperMatchPoints * it.r_pillar.m_previousMatchPoints,
                                        r_leftMostPlayer.position.z);
            }
        }

        void InitSelect() {
            for (int i = 0; i < m_nextGames.Length; i++) {
                //m_NextGames[i].first = Random.Range(0, DataHolder.s_maps.Lenght);
                m_nextGames[i].second = Random.Range(0, DataHolder.s_modis.Length);
            }

            foreach(var it in Player.s_references) {
                Destroy(it.r_pillar.gameObject);
            }

            r_selectParent.SetActive(true);
        }

        public void SelectFinished(int index) {
            DataHolder.s_currentMap = m_nextGames[index].first;
            DataHolder.s_currentModi = m_nextGames[index].second;

            Continue();
        }

        void Continue() {
            TransitionHandler.StartOutTransition();
        }
    }
}