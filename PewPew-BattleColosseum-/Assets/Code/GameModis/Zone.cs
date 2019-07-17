using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Collider2D))]
    public class Zone : MonoBehaviour {

        #region Variables

        KotH r_refGM;
        float m_moveDelay;
        float m_inBetweenTime;
        float m_pPS;
        float m_threashold;

        float m_startTime;
        Vector2 m_size;

        List<System.Tuple<Player, float>> players = new List<System.Tuple<Player, float>>();

        #endregion
        #region MonoBehaviour

        private void Update() {
            foreach(var it in players) {
                if(it.Item2 > Time.time + m_threashold) {
                    r_refGM.ScorePoint(it.Item1, m_pPS * Time.deltaTime);
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

            m_startTime = Time.time;
        }

        void Reposition() {
            transform.position = new Vector2(Random.Range(-m_size.x / 2, m_size.x / 2), Random.Range(-m_size.y / 2, m_size.y / 2));
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