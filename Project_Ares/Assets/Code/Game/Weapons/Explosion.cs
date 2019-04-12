using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class Explosion : MonoBehaviour, IHarmingObject {

        #region Variables
        [Header("References")]
        [SerializeField] GameObject m_explosion;
        [SerializeField] GameObject m_explosionStains;
        [SerializeField] AudioSource m_audio;

        [Header("Balancing")]
        [SerializeField] int m_baseDamage = 1;
        [SerializeField] float m_radius = 5;
        [SerializeField] float m_baseKnockback = 300;
        [SerializeField] AnimationCurve m_fallOff;
        [SerializeField] float m_explosionTime = 0.5f;

        float m_time;

        #endregion
        #region MonoBehaviour

        void Start() {
            //animation von explosion abspielen
        }

        void Update() {
            if (m_time + m_explosionTime < Time.timeSinceLevelLoad) {
                Destroy(m_explosion);
                m_explosionStains.SetActive(true);
                Destroy(this.gameObject);

            }
        }

        #endregion
        #region IHarmingObject

        public Rigidbody2D Init(Player reverence) {
            m_explosion.SetActive(true);
            m_time = Time.timeSinceLevelLoad;
            m_audio.Play();
            foreach (var it in Physics2D.OverlapCircleAll(transform.position, m_radius)) {
                IDamageableObject tmp = it.gameObject.GetComponent<IDamageableObject>();
                if (tmp != null) {
                    Vector2 dir = it.transform.position - transform.position;
                    float fallOff = m_fallOff.Evaluate(dir.magnitude);
                    tmp.TakeDamage((int)Mathf.Round(fallOff * m_baseDamage), reverence == null ? null : reverence, dir.normalized * fallOff * m_baseKnockback);//eventuell doch in rocket mit rein schreiben wegen reference zu source player
                }
            }
            return null;
        }

        #endregion
        //#region Physics

        //private void OnCollisionEnter2D(Collision2D collision) {
        //    IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
        //    if (tmp != null) {
        //        Vector2 dir = collision.transform.position - transform.position;
        //        float fallOff = m_fallOff.Evaluate(dir.magnitude);
        //        tmp.TakeDamage((int)Mathf.Round(fallOff * m_baseDamage), m_source == null ? null : m_source, dir.normalized * fallOff * m_baseKnockback);//eventuell doch in rocket mit rein schreiben wegen reference zu source player
        //    }
        //}

        //#endregion
    }
}