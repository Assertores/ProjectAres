using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class FireworkAudio : MonoBehaviour {

        [SerializeField] ParticleSystem m_system;
        [SerializeField] AudioClip[] m_flyClip;
        [SerializeField] AudioClip[] m_explosionClip;

        List<float> m_startTimes = new List<float>();

        [SerializeField] AudioSource[] m_flySource;
        int m_flySourceCurrent_ = 0;
        int m_flySourceCurrent { get {
            m_flySourceCurrent = m_flySourceCurrent_ + 1;
            return m_flySourceCurrent_;
        } set {
            m_flySourceCurrent_ = value;
            m_flySourceCurrent_ %= m_flySource.Length;
        } }
        [SerializeField] AudioSource[] m_expSource;
        int m_expSourceCurrent_ = 0;
        int m_expSourceCurrent {
            get {
                m_expSourceCurrent = m_expSourceCurrent_ + 1;
                return m_expSourceCurrent_;
            }
            set {
                m_expSourceCurrent_ = value;
                m_expSourceCurrent_ %= m_expSource.Length;
            }
        }

        //https://forum.unity.com/threads/access-to-the-particle-system-lifecycle-events.328918/
        private void LateUpdate() {
            ParticleSystem.Particle[] m_particles = new ParticleSystem.Particle[m_system.main.maxParticles];

            int size = m_system.GetParticles(m_particles);

            for(int i = 0; i < size; i++){
                if(!m_startTimes.Exists(x => x == m_particles[i].startLifetime)){
                    m_startTimes.Add(m_particles[i].startLifetime);
                    int j = m_flySourceCurrent;
                    m_flySource[j].Stop();
                    m_flySource[j].clip = m_flyClip[Random.Range(0, m_flyClip.Length)];
                    m_flySource[j].Play();
                    //m_flySource[m_flySourceCurrent].PlayOneShot(m_flyClip[Random.Range(0, m_flyClip.Length)]);

                    StartCoroutine(ParticleLifeEnding(m_particles[i].remainingLifetime));
                }
            }
        }

        private IEnumerator ParticleLifeEnding(float lifetime) {
            print(lifetime);
            yield return new WaitForSeconds(lifetime);
            int i = m_expSourceCurrent;
            m_expSource[i].Stop();
            m_expSource[i].clip = m_explosionClip[Random.Range(0, m_explosionClip.Length)];
            m_expSource[i].Play();
            //m_expSource[m_expSourceCurrent].PlayOneShot(m_explosionClip[Random.Range(0, m_explosionClip.Length)]);
        }
    }
}