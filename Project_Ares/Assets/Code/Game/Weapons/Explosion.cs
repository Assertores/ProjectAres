using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class Explosion : MonoBehaviour {

        #region Variables

        [Header("Balancing")]
        [SerializeField] int m_baseDamage = 1;
        [SerializeField] float m_range = 5;
        [SerializeField] AnimationCurve m_fallOff;

        #endregion
        #region MonoBehaviour

        void Start() {

        }

        void Update() {

        }

        #endregion
        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                int damage = (int)Mathf.Round(m_fallOff.Evaluate(Vector2.Distance(collision.transform.position, transform.position)) * m_baseDamage);
                tmp.TakeDamage(damage, null);//eventuell doch in rocket mit rein schreiben wegen reference zu source player
            }
        }

        #endregion
    }
}