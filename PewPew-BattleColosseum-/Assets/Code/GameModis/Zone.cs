using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Collider2D))]
    public class Zone : MonoBehaviour {

        #region Variables

        [SerializeField] ContactFilter2D m_filter;

        KotH r_refGM;
        float m_moveDelay;
        float m_inBetweenTime;
        float m_pPS;
        float m_threashold;

        float m_startTime;
        Vector2 m_size;
        bool m_repositioned;

        List<System.Tuple<Player, float>> players = new List<System.Tuple<Player, float>>();

        #endregion
        #region MonoBehaviour

        private void Awake() {
            m_col = GetComponent<Collider2D>();
        }

        private void Update() {
            foreach(var it in players) {
                if(Time.time > it.Item2 + m_threashold) {

                    r_refGM.ScorePoint(it.Item1, m_pPS * Time.deltaTime);
                }
            }
            if((Time.time - m_startTime) % (m_moveDelay + m_inBetweenTime) > m_moveDelay) {
                if (m_repositioned) {
                    m_repositioned = false;
                    Activate(false);
                    players.Clear();
                }
            } else {
                if (!m_repositioned) {
                    Reposition();
                    m_repositioned = true;
                    Activate(true);
                }
            }
        }

        #endregion

        public void Init(KotH refGameMode, float moveDelay, float inBetweenTime, float pointsPerSecond, float diameter, float threashold) {
            r_refGM = refGameMode;
            m_moveDelay = moveDelay;
            m_inBetweenTime = inBetweenTime;
            m_pPS = pointsPerSecond;
            m_threashold = threashold;

            transform.localScale = new Vector3(diameter, diameter, diameter);
            m_size = MapHandler.s_singelton.GetSize();
            Reposition();
            m_repositioned = true;

            m_startTime = Time.time;
        }

        void Reposition() {
            players.Clear();

            transform.position = new Vector2(Random.Range(-m_size.x / 2, m_size.x / 2), Random.Range(-m_size.y / 2, m_size.y / 2));

            Collider2D[] result = new Collider2D[10];
            int count = Physics2D.OverlapCollider(m_col, m_filter, result);
            for (int i = 0; i < count; i++) {
                if(result[i].tag == StringCollection.T_PLAYER) {
                    System.Tuple<Player, float> element = new System.Tuple<Player, float>(result[i].GetComponent<Player>(), Time.time);
                    players.Add(element);
                }
            }
        }

        Collider2D m_col;
        void Activate(bool value) {
            foreach(Transform it in transform) {
                it.gameObject.SetActive(value);
            }
            m_col.enabled = value;
        }

        #region Physics

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag != StringCollection.T_PLAYER)
                return;

            //--> is player <--

            System.Tuple<Player, float> element = new System.Tuple<Player, float>(collision.GetComponent<Player>(), Time.time);
            players.Add(element);
            
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.tag != StringCollection.T_PLAYER)
                return;

            //--> is player <--

            Player player = collision.GetComponent<Player>();

            players.Remove(players.Find(x => x.Item1 == player));
            
        }

        #endregion
    }
}