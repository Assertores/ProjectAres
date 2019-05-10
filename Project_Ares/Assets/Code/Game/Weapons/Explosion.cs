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
        [SerializeField] AudioClip[] m_sounds;

        [Header("Balancing")]
        [SerializeField] float m_baseDamage = 1;
        [SerializeField] float m_radius = 5;
        [SerializeField] float m_baseKnockback = 300;
        [SerializeField] AnimationCurve m_fallOff;
        [SerializeField] float m_explosionTime = 0.5f;
        [SerializeField] float m_halfPitchRange = 0.1f;
        [SerializeField] float m_halfVolumeRange = 0.1f;

        float m_time;

        float m_startPitch;
        float m_startVolume;

        #endregion
        #region MonoBehaviour

        void Start() {
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

        public Rigidbody2D Init(Player reference, Sprite icon) {
            m_explosion.SetActive(true);
            m_time = Time.timeSinceLevelLoad;

            m_startPitch = m_audio.pitch;
            m_startVolume = m_audio.volume;

            if (m_sounds.Length > 0) {
                m_audio.pitch = Random.Range(m_startPitch - m_halfPitchRange, m_startPitch + m_halfPitchRange);
                m_audio.volume = Random.Range(m_startVolume - m_halfVolumeRange, m_startVolume + m_halfVolumeRange);
                m_audio.PlayOneShot(m_sounds[Random.Range(0, m_sounds.Length - 1)]);
            }

            CameraShake.DoCamerashake(0.1f, 0.7f);
         
            foreach (var it in Physics2D.OverlapCircleAll(transform.position, m_radius)) {
                 IDamageableObject tmp = it.gameObject.GetComponent<IDamageableObject>();
                 if (tmp != null) {
                    Vector2 dir = it.transform.position - transform.position;
                    float fallOff = m_fallOff.Evaluate(dir.magnitude/m_radius);
                    tmp.TakeDamage((fallOff * m_baseDamage), reference == null ? null : reference, dir.normalized * fallOff * m_baseKnockback, icon);//eventuell doch in rocket mit rein schreiben wegen reference zu source player
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