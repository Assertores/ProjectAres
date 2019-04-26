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
        [Tooltip("Muss am besten zwischen 1 u. 2 liegen")]
        [SerializeField] float m_bouncinessFactor;
        int m_currentHealth;


        Dictionary<Collider2D, Vector2> m_collisionNormals = new Dictionary<Collider2D, Vector2>();

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
            Vector2 tmpNormal = new Vector2(0,0);
            m_collisionNormals[collision.collider] = tmpNormal.normalized;
            if (collision.gameObject.tag =="Level" ){
                ReflectDirection(collision.contacts[0].normal);
                foreach(var it in collision.contacts) {
                    Debug.DrawRay(it.point, it.normal);
                }

            }
            
        }
        
        private void OnTriggerExit2D(Collider2D collision) {
            ReflectDirection(Vector2.up);
        }


        #endregion

        void ReflectDirection(Vector2 m_collisionNormal) {
            Vector2 tmp = (m_bouncinessFactor*(m_rb.velocity));
            print(tmp);
            tmp = Vector2.Reflect(tmp, m_collisionNormal);
            print(tmp);
            m_rb.velocity = tmp;
        }
    }

}
