using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PPBC {
    [RequireComponent(typeof(Collider2D))]
    public class Hatch : MonoBehaviour {

        #region Variables
        [Header("References")]
        [SerializeField] GameObject m_exitHatch;
        [SerializeField] TextMeshProUGUI m_countdownText;

        [Header("Variables")]
        [SerializeField] float m_exitTime;

        private float m_time;
        private int m_collInd = 0;

        #endregion
        #region MonoBehaviour

        private void Awake() {
            if (!m_exitHatch) {
                print("no exit hatch");
                Destroy(this);
                return;
            }
            if (!m_countdownText) {
                print("no count down text");
                Destroy(this);
                return;
            }
        }

        private void Update() {
            if(m_collInd > 0) {
                if (m_exitTime + m_time <= Time.time) {
                    m_exitHatch.SetActive(false);
                    m_countdownText.text = "";
                } else {
                    m_countdownText.text = Mathf.RoundToInt(m_exitTime+ m_time - Time.time).ToString();
                }
            } else {
                m_exitHatch.SetActive(true);
                m_countdownText.text = "";
            }
        }

        #endregion
        #region Physics

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.GetComponent<Player>()) {
                m_collInd++;
                if(m_collInd == 1) {
                    m_time = Time.time;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.GetComponent<Player>())
                m_collInd--;
        }

        #endregion
    }
}