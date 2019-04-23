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

        public Rigidbody2D Init(Player reference) {
            Debug.LogError("Crash1");
            
            m_explosion.SetActive(true);
            
            Debug.LogError("Crash2");
            
            m_time = Time.timeSinceLevelLoad;
            Debug.LogError("Crash3");
            
            if (m_sounds.Length > 0) {
                Debug.LogError("Crash4");
                
                m_audio.PlayOneShot(m_sounds[Random.Range(0, m_sounds.Length - 1)]);
                Debug.LogError("Crash5");
                
            }
            
            CameraShake.DoCamerashake(0.1f, 0.7f);
            Debug.LogError("Crash6");
            
            foreach (var it in Physics2D.OverlapCircleAll(transform.position, m_radius)) {
                Debug.LogError("Crash7");
                
                IDamageableObject tmp = it.gameObject.GetComponent<IDamageableObject>();
                Debug.LogError("Crash8");
                
                if (tmp != null) {
                    Debug.LogError("Crash9");
                    
                    Vector2 dir = it.transform.position - transform.position;
                    Debug.LogError("Crash10");
                    
                    float fallOff = m_fallOff.Evaluate(dir.magnitude/m_radius);
                    Debug.LogError("Crash11");
                    
                    tmp.TakeDamage((int)Mathf.Round(fallOff * m_baseDamage), reference == null ? null : reference, dir.normalized * fallOff * m_baseKnockback);//eventuell doch in rocket mit rein schreiben wegen reference zu source player
                    Debug.LogError("Crash12");
                    return null;
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