using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PPBC {
    public class ExitHandler : MonoBehaviour {

        [Header("References")]
        [SerializeField] GameObject m_hatch;
        [SerializeField] TextMeshProUGUI m_ContdownText;
        [SerializeField] TextMeshProUGUI m_ExitText;

        [Header("Balancing")]
        [SerializeField] float m_hatchDelay = 3;

        float m_triggerTime = float.MaxValue;

        // Start is called before the first frame update
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {
            if(m_triggerTime < Time.time) {
                if(m_triggerTime+m_hatchDelay <= Time.time) {
                    m_ContdownText.text = "";
                    m_hatch.SetActive(false);
                } else {
                    m_ContdownText.text = Mathf.RoundToInt(m_hatchDelay - (Time.time - m_triggerTime)).ToString();
                }
            }
            if(Player.s_references.Count > 1) {
                m_ExitText.text = "Disconnect"; //Localisation
            } else {
                m_ExitText.text = "Quit"; //Localisation
            }
        }

        public void StartCountDown() {
            m_triggerTime = Time.time;
        }

        public void StopCountDown() {
            m_triggerTime = float.MaxValue;
            m_ContdownText.text = "";
            m_hatch.SetActive(true);
        }

        public void DoDisconnect() {
            if(Player.s_references.Count == 1) {
                print("Quiting the Game");
                Application.Quit();
            }

            DataHolder.s_hoPlayer?.m_control.DoDisconect();
        }
    }
}