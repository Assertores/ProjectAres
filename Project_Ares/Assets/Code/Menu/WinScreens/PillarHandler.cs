using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class PillarHandler : MonoBehaviour, IScriptQueueItem {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_pillarPrefab;
        [SerializeField] Transform m_rightMostPlayer;
        [SerializeField] Transform m_leftMostPlayer;
        [SerializeField] Transform m_maxHeight;

        [Header("Balancing")]
        [SerializeField] float m_pillarRiseTime;

        List<PillarRefHolder> m_pillar = new List<PillarRefHolder>();

        float m_pillarSpeed = 1;
        float m_hightPerKill = 1;
        float m_startTime;

        public System.Action CallBack;
        #endregion
        #region MonoBehaviour

        void Start() {
            if (!m_pillarPrefab.GetComponent<PillarRefHolder>()) {
                print("no pillar ref on the Prefab");
                Destroy(this);
                return;
            }

            m_pillarSpeed = (m_maxHeight.position.y - m_rightMostPlayer.position.y) / m_pillarRiseTime;
            m_hightPerKill = (m_maxHeight.position.y - m_rightMostPlayer.position.y) / Player.s_sortedRef[0].m_stats.m_points;
            
            EndScreenManager.s_ref?.AddItem(this, 0);
        }

        #endregion
        #region IScriptQueueItem

        public bool FirstTick() {
            for (int i = 0; i < Player.s_references.Count; i++) {
                Player.s_references[i].transform.position = Vector3.Lerp(m_leftMostPlayer.position, m_rightMostPlayer.position, ((float)i + 1) / (Player.s_references.Count + 1));
                m_pillar.Add(Instantiate(m_pillarPrefab, Player.s_references[i].transform.position, Player.s_references[i].transform.rotation).GetComponent<PillarRefHolder>());

                //----- ----- FeedBack ----- -----
                m_pillar[i].m_screen.text = "0";
                m_pillar[i].m_playerName.text = Player.s_references[i].m_stats.m_name;
                m_pillar[i].m_pillarGradient.gameObject.SetActive(false);
                m_pillar[i].m_pillarField.text = "";
            }
            m_startTime = Time.timeSinceLevelLoad;

            return DoTick();
        }

        public bool DoTick() {
            for (int i = 0; i < Player.s_references.Count; i++) {
                if (m_pillar[i].gameObject.transform.position.y < m_leftMostPlayer.position.y + (Player.s_references[i].m_stats.m_kills * m_hightPerKill)) {
                    m_pillar[i].m_screen.text = Mathf.RoundToInt((m_pillar[i].gameObject.transform.position.y - m_leftMostPlayer.position.y) / m_hightPerKill).ToString();
                    m_pillar[i].gameObject.transform.position += new Vector3(0, m_pillarSpeed * Time.deltaTime, 0);

                    Player.s_references[i].transform.position = m_pillar[i].gameObject.transform.position;
                } else {
                    int index = Player.s_sortedRef.IndexOf(Player.s_references[i]);
                    m_pillar[i].m_pillarField.text = ("Platz " + (index + 1).ToString());//TODO: Lokalisierung, Bracket anstadt finale
                    Color c = Color.green;
                    c.a = m_pillar[i].m_pillarGradient.color.a;
                    m_pillar[i].m_pillarGradient.color = c;
                    m_pillar[i].m_pillarGradient.gameObject.SetActive(true);

                    if (index == 0) {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}