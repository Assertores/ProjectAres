using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class RocketTracer : MonoBehaviour, ITracer {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject p_explosion;
        [SerializeField] ContactFilter2D m_filter;

        [Header("Balancing")]
        [Tooltip("distance form levelorigion to autodestry")]
        [SerializeField] float m_killDistance;
        [SerializeField] float m_damage;

        Rigidbody2D m_rb;
        Collider2D m_col;

        Collider2D m_spawnInCollider = null;

        #endregion
        #region MonoBehaviour

        private void Awake() {
            if (!m_rb)
                m_rb = GetComponent<Rigidbody2D>();
            if (!m_col)
                m_col = GetComponent<Collider2D>();

            Collider2D[] tmp = new Collider2D[1];
            if (Physics2D.OverlapCollider(m_col, m_filter, tmp) > 0) {
                m_spawnInCollider = tmp[0];
            }
        }

        void Update() {
            if (transform.position.magnitude > m_killDistance) {
                Destroy(this.gameObject);
            }
        }

        #endregion
        #region ITracer

        public IHarmingObject m_trace { get; private set; }

        public Rigidbody2D Init(IHarmingObject trace) {
            m_trace = trace;

            m_rb = GetComponent<Rigidbody2D>();
            return m_rb;
        }

        #endregion
        #region Physics

        bool h_exploded = false;
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision == m_spawnInCollider)
                return;

            if (h_exploded)
                return;
            h_exploded = true;

            if (p_explosion) {
                GameObject temp = Instantiate(p_explosion, transform.position, transform.rotation);//TODO: objectPooling
                temp.GetComponentInChildren<ITracer>()?.Init(m_trace);
            }

            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                tmp.TakeDamage(m_trace, m_damage, Vector2.zero);//rocket wont give recoil to the hit one
            }

            Destroy(gameObject);//TODO: objectPooling
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (m_spawnInCollider = collision)
                m_spawnInCollider = null;
        }

        #endregion
    }
}