using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class BulletTracer : MonoBehaviour, ITracer {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject r_bullet;

        [Header("Balancing")]
        [SerializeField] float m_killDistance = 1000;
        [SerializeField] float m_damage= 1;

        Rigidbody2D m_rb;
        Collider2D m_col;

        #endregion

        private void Update() {
            if (transform.position.magnitude > m_killDistance) {
                Destroy(this.gameObject);
            }
        }
        
        #region ITracer

        public IHarmingObject m_trace { get; private set; }

        public Rigidbody2D Init(IHarmingObject trace) {
            m_trace = trace;

            m_rb = GetComponent<Rigidbody2D>();
            m_col = GetComponent<Collider2D>();
            return m_rb;
        }

        #endregion

        IEnumerator IEEffects() {//TODO: hand over angle
            //TODO: rotate effect
            //TODO: start effects
            //Play Audio

            yield return null;//TODO: wait for effect finish
            Destroy(this.gameObject);//TODO: objectPooling
        }

        #region Physics

        private void OnTriggerEnter2D(Collider2D collision) {
            IDamageableObject hit = collision.gameObject.GetComponent<IDamageableObject>();
            if (hit != null) {
                hit.TakeDamage(m_trace, m_damage, m_rb.velocity * m_rb.mass);
            }
            
            m_rb.isKinematic = true;
            m_col.enabled = false;
            r_bullet.SetActive(false);

            StartCoroutine(IEEffects());
        }

        #endregion
    }
}