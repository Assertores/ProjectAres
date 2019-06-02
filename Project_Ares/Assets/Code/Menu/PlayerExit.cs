using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PPBC
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerExit : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] GameObject m_exitHatch;
        [SerializeField] StartGame m_startButton;
        [SerializeField] TextMeshProUGUI m_countdownText;
        [SerializeField] TextMeshProUGUI m_exitText;
        
        
        

        [Header("Variables")]
        [SerializeField] float m_exitTime;
        private float m_time;
        private int m_collInd = 0;
        [SerializeField] int m_exitHealth;


        #endregion

        private void Start()
        {

            m_exitText.text = "";
        }

        private void Update()
        {
            
            if (m_startButton.m_currentHealth >= m_exitHealth) {
                m_exitText.text = "";
                m_exitHatch.SetActive(true);
                GetComponent<BoxCollider2D>().enabled = true;
                if (Player.s_references.Count != 0) {
                    if (Player.s_references.Count == 1) {
                        m_exitText.text = "Exit";
                    } else {
                        m_exitText.text = "Disconnect";

                    }
                }
                float tmp = m_exitTime + m_time - Time.timeSinceLevelLoad;
                if (m_collInd > 0) {

                    if (m_exitTime + m_time <= Time.timeSinceLevelLoad) {
                        m_exitHatch.SetActive(false);

                    }
                    m_countdownText.text = Mathf.RoundToInt(tmp).ToString();
                    if (tmp <= 0) {
                        m_exitText.text = "";
                        m_countdownText.text = "Hatch is Opening";
                    }
                } else {
                    m_countdownText.text = "";
                    m_exitHatch.SetActive(true);

                }
            } else {
                m_countdownText.text = "";
                m_exitText.text = "Exit blocked";
                GetComponent<BoxCollider2D>().enabled = false;
                m_exitHatch.SetActive(true);
            }

        }
        #region Physics




        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == StringCollection.PLAYER) {
                m_collInd++;
                if (m_collInd == 1)
                    m_time = Time.timeSinceLevelLoad;
            }
            

        }

    
        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.gameObject.tag == StringCollection.PLAYER) {
                m_collInd--;
                if (m_collInd == 0)
                    m_time = float.MinValue;
            }
            
        }

    }
}


        #endregion
    

