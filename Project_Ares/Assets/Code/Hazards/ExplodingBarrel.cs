using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace ProjectAres
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ExplodingBarrel : MonoBehaviour, IDamageableObject
    {
        #region Variables
        [Header("References")]
        [SerializeField] GameObject m_explosion;
        TextMeshPro m_healthText;

        [Header("Balancing")]
        [SerializeField] float m_maxHealth;
        float m_currentHealth;


        

        bool m_isExploded = false;
        Rigidbody2D m_rb;

        #endregion

        #region IDamageableObject


        public bool m_alive { get; set; }

        public void Die(Player source) {
            return;

        }

        public float GetHealth() {
            throw new System.NotImplementedException();
        }

        public void TakeDamage(float damage, Player source, Vector2 force) {
            m_currentHealth -= damage;
            m_healthText.text = (Mathf.RoundToInt(m_currentHealth)).ToString();
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
            m_healthText = this.GetComponent<TextMeshPro>();
            m_currentHealth = m_maxHealth;
            m_rb = GetComponent<Rigidbody2D>();
            
            m_healthText.text = (m_currentHealth.ToString());
        }

        #endregion


    }

}


/*Vector2 tmp = ((m_bouncinessFactor * m_rb.velocity));
tmp = Vector2.Reflect(tmp, m_collisionNormal);
            m_rb.velocity = tmp;*/