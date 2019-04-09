using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    [RequireComponent(typeof(Collider2D))]
    public class Explosion : MonoBehaviour {

        #region Variables

        [Header("Balancing")]
        [SerializeField] int m_baseDamage = 1;
        [SerializeField] float m_baseKnockback = 3;
        [SerializeField] AnimationCurve m_fallOff;

        #endregion
        #region MonoBehaviour

        void Start() {
            //animation von explosion abspielen
        }

        void Update() {

        }

        #endregion
        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                Vector2 dir = collision.transform.position - transform.position;
                float fallOff = m_fallOff.Evaluate(dir.magnitude);
                tmp.TakeDamage((int)Mathf.Round(fallOff * m_baseDamage), null, dir.normalized * fallOff * m_baseKnockback);//eventuell doch in rocket mit rein schreiben wegen reference zu source player
            }
        }

        #endregion
    }
}