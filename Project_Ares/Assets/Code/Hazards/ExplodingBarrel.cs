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
        [Tooltip("Muss am besten zwischen 1 u. 2 liegen")]
        [SerializeField] float m_bouncinessFactor;
        float m_currentHealth;


        Dictionary<Collider2D, Vector2> m_collisionNormals = new Dictionary<Collider2D, Vector2>();

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
            if (Vector2.Dot(m_rb.velocity, m_collisionNormal) < 0)
                m_rb.velocity = Vector2.Reflect(m_rb.velocity, m_collisionNormal);
                  
        }
    }

}


/*Vector2 tmp = ((m_bouncinessFactor * m_rb.velocity));
tmp = Vector2.Reflect(tmp, m_collisionNormal);
            m_rb.velocity = tmp;*/