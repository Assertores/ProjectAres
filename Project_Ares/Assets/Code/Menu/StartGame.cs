﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PPBC {
    public class StartGame : MonoBehaviour, IDamageableObject {




        #region Variables
        public bool m_alive { get; set; }

        [SerializeField]private int m_maxHealth = 100;

        public float m_currentHealth;

        public Text m_healthText;
        public Text m_startText;

        private float m_time;
        [SerializeField] private float m_regenTime;
        [SerializeField] private float m_regeneration;

        #endregion
        #region MonoBehaviour
        void Start()
        {
            m_currentHealth = m_maxHealth;
            m_healthText.text = (Mathf.RoundToInt(m_currentHealth)).ToString();
            m_startText.text = "";
        }

        private void Update()
        {
            if (m_currentHealth > 0)
            {
                if (m_time + m_regenTime <= Time.timeSinceLevelLoad)
                {

                    m_currentHealth += m_regeneration * Time.deltaTime;
                    m_healthText.text = (Mathf.RoundToInt(m_currentHealth)).ToString();

                    if (m_currentHealth > m_maxHealth)
                    {

                        m_currentHealth = m_maxHealth;

                    }


                }
            }

        }
        #endregion
        #region IDamageableObject

        public void TakeDamage(float damage, Player source, Vector2 force, Sprite icon) {

            if (m_currentHealth > 0)
            {
                m_time = Time.timeSinceLevelLoad;
                m_currentHealth -= damage;
                m_healthText.text = (Mathf.RoundToInt(m_currentHealth)).ToString();

            }
            if (m_currentHealth <= 0)
            {
                m_healthText.text = "";
                m_startText.text = "Game is Starting";
                StartCoroutine(ChangeScene(3.0f));
 

            }
        }

        public void Die(Player source) {
            return;
        }

        public float GetHealth() {
            return 0;
        }

        #endregion

        IEnumerator ChangeScene(float m_wait)
        {
            
            yield return new WaitForSeconds(m_wait);
            
            MenuManager._singelton?.StartGame();
            
        }

    }

  }