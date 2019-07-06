using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Rigidbody2D))]
    public class BulletTracer : MonoBehaviour, ITracer {

        #region Variables

        [Header("Balancing")]
        [SerializeField] float m_killDistance = 1000;
        [SerializeField] float m_damage= 1;

        Rigidbody2D m_rb;

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
            return m_rb;
        }

        #endregion

        IEnumerator IEEffects() {
            //TODO: start effects

            yield return null;//TODO: wait for effect finish
            Destroy(this.gameObject);
        }

        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
            IDamageableObject hit = collision.gameObject.GetComponent<IDamageableObject>();
            if (hit != null) {
                hit.TakeDamage(m_trace, m_damage, m_rb.velocity * m_rb.mass);
            }

            //TODO: deactivate bullet

            StartCoroutine(IEEffects());
        }

        #endregion
    }
}