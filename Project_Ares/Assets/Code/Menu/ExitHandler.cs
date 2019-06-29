using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class ExitHandler : Sauerbraten {

        [Header("References")]
        [SerializeField] GameObject m_hatch;
        [SerializeField] TextMeshProUGUI m_ContdownText;
        [SerializeField] TextMeshProUGUI m_ExitText;
        [SerializeField] ShootButton m_startButton;

        [Header("Balancing")]
        [SerializeField] float m_hatchDelay = 3;
        [SerializeField] float m_blockLife;

        float m_triggerTime = float.MaxValue;

        // Start is called before the first frame update
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {
            if (m_triggerTime < Time.time) {
                if (m_triggerTime + m_hatchDelay <= Time.time) {
                    m_ContdownText.text = "";
                    m_hatch.SetActive(false);
                } else {
                    m_ContdownText.text = Mathf.RoundToInt(m_hatchDelay - (Time.time - m_triggerTime)).ToString();
                }
            }

            /*if (m_startButton.m_currentLife < m_blockLife) {
                m_ExitText.text = "Exit blocked";
            } else */{ 
                if (Player.s_references.Count > 1) {
                    m_ExitText.text = "Disconnect"; //Localisation
                } else {
                    m_ExitText.text = "Quit"; //Localisation
                }
            }
        }

        public void StartCountDown() {
            //if (m_startButton.m_currentLife > m_blockLife) {
                m_triggerTime = Time.time;
            //}
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