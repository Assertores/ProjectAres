using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovingObject : MonoBehaviour {
        #region Variables
        [Header("Balancing")]
        [Tooltip("Muss am besten zwischen 1 und 2 liegen")]
        [SerializeField] float m_bouncinessFactor;

        Rigidbody2D m_rb;

        Dictionary<Collider2D, Vector2> m_collisionNormals = new Dictionary<Collider2D, Vector2>();

        #endregion

        #region Monobehaviour

        // Start is called before the first frame update
        void Start() {
            m_rb = GetComponent<Rigidbody2D>();
        }
        #endregion

        #region Physics
        private void OnCollisionEnter2D(Collision2D collision) {
            Vector2 tmpNormal = new Vector2(0, 0);

            m_collisionNormals[collision.collider] = tmpNormal.normalized;
            if (collision.gameObject.tag == "Level") {
                ReflectDirection(collision.contacts[0].normal);
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
