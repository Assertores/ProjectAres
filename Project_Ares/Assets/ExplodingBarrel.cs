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


       // Dictionary<Collider2D, Vector2> m_collisionNormals = new Dictionary<Collider2D, Vector2>();

        bool m_isExploded = false;
        Rigidbody2D m_rb;

        #endregion

        #region IDamageableObject


        public bool m_alive { get; set; }

        public void Die(Player source) {
            return;

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

        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag =="Level" ){
                ReflectDirection(collision.contacts[0].normal);
            }

        }
        /*private void OnTriggerExit2D(Collider2D collision) {
            ReflectDirection(collision.normal);
        }*/


        #endregion

        void ReflectDirection(Vector2 m_collisionNormal) {
            Vector2 tmp = (m_rb.velocity);
            print(tmp);
            tmp = Vector2.Reflect(tmp, m_collisionNormal);
            print(tmp);
            m_rb.velocity = tmp;
        }
    }

}
