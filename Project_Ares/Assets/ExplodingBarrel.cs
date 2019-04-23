using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ExplodingBarrel : MonoBehaviour, IDamageableObject
    {
        #region Variables
        [Header("References")]
        [SerializeField] GameObject m_explosion;

        [Header("Balancing")]
        [SerializeField] int m_maxHealth;
        int m_currentHealth;

        bool m_isExploded = false;
        Rigidbody2D m_rb;

        #endregion

        #region IDamageableObject


        public bool m_alive { get; set; }

        public void Die(Player source) {
            throw new System.NotImplementedException();
        }

        public int GetHealth() {
            throw new System.NotImplementedException();
        }

        public void TakeDamage(int damage, Player source, Vector2 force) {
            m_currentHealth -= damage;
            m_rb.AddForce(force);
            if(!m_isExploded && m_currentHealth <= 0) {
                m_isExploded = true;
                Instantiate(m_explosion, transform.position, transform.rotation).GetComponentInChildren<IHarmingObject>()?.Init(source);
                Destroy(gameObject);

            }

            
        }
        #endregion

        #region MonoBehaviour
        void Start()
        {
          
            m_currentHealth = m_maxHealth;
            m_rb = GetComponent<Rigidbody2D>();
        }

        #endregion
    }

}
