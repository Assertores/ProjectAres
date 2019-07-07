using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class ExplosionTracer : MonoBehaviour, ITracer {

        #region Variables
        [Header("References")]
        [SerializeField] GameObject r_explosion;
        [SerializeField] GameObject r_explosionStains;
        [SerializeField] AudioSource fx_audio;
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

        void Update() {
            if (m_time + m_explosionTime < Time.timeSinceLevelLoad) {
                Destroy(r_explosion);
                r_explosionStains.SetActive(true);
                Destroy(this.gameObject);

            }
        }

        #endregion
        #region ITracer

        public IHarmingObject m_trace { get; private set; }

        public Rigidbody2D Init(IHarmingObject trace) {
            m_trace = trace;

            r_explosion.SetActive(true);
            m_time = Time.timeSinceLevelLoad;

            if (m_sounds.Length > 0) {
                fx_audio.pitch = Random.Range(fx_audio.pitch - m_halfPitchRange, fx_audio.pitch + m_halfPitchRange);
                fx_audio.volume = Random.Range(fx_audio.volume - m_halfVolumeRange, fx_audio.volume + m_halfVolumeRange);
                fx_audio.PlayOneShot(m_sounds[Random.Range(0, m_sounds.Length)]);
            }

            CameraShake.DoCamerashake(0.1f, 0.7f);

            foreach (var it in Physics2D.OverlapCircleAll(transform.position, m_radius)) {
                IDamageableObject tmp = it.gameObject.GetComponent<IDamageableObject>();
                if (tmp != null) {
                    Vector2 dir = it.transform.position - transform.position;
                    float fallOff = m_fallOff.Evaluate(dir.magnitude / m_radius);
                    tmp.TakeDamage(m_trace, (fallOff * m_baseDamage), dir.normalized * fallOff * m_baseKnockback);
                }
            }
            return null;
        }

        #endregion
    }
}