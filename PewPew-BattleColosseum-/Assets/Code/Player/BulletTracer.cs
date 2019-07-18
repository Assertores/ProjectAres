using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class BulletTracer : MonoBehaviour, ITracer {

        enum e_impactEffects { Player, Level, CreditsObject};

        [System.Serializable]
        struct d_imactEffects {
            public e_impactEffects m_type;
            public AudioClip[] m_clips;

            [HideInInspector] public string m_typeString;
        }

        #region Variables

        [Header("References")]
        [SerializeField] GameObject r_bullet;
        [SerializeField] ContactFilter2D m_filter;
        [SerializeField] GameObject r_wallHitParent;
        [SerializeField] ParticleSystem Vfx_wallHit;
        [SerializeField] AudioSource m_audio;
        [SerializeField] d_imactEffects[] m_sounds;

        [Header("Balancing")]
        [SerializeField] float m_killDistance = 1000;
        [SerializeField] float m_damage= 1;

        Rigidbody2D m_rb;
        Collider2D m_col;

        Collider2D m_spawnInCollider = null;

        #endregion

        private void Awake() {
            if(!m_rb)
                m_rb = GetComponent<Rigidbody2D>();
            if(!m_col)
                m_col = GetComponent<Collider2D>();

            Collider2D[] result = new Collider2D[10];
            int count = Physics2D.OverlapCollider(m_col, m_filter, result);
            for (int i = 0; i < count; i++) {
                if(result[i].tag == StringCollection.T_PLAYER) {
                    m_spawnInCollider = result[i];
                    break;
                }
            }

            for (int i = 0; i < m_sounds.Length; i++) {
                switch (m_sounds[i].m_type) {
                case e_impactEffects.Player:
                    m_sounds[i].m_typeString = StringCollection.T_PLAYER;
                    break;
                case e_impactEffects.Level:
                    m_sounds[i].m_typeString = StringCollection.T_LEVEL;
                    break;
                case e_impactEffects.CreditsObject:
                    m_sounds[i].m_typeString = StringCollection.T_CREOBJ;
                    break;
                default:
                    break;
                }
            }
        }

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

        IEnumerator IEEffects(string impactTag) {
            Vfx_wallHit.Play();
            int i = 0;
            for (; i < m_sounds.Length && m_sounds[i].m_typeString != impactTag; i++) ;
            if(i < m_sounds.Length) {
                m_audio.PlayOneShot(m_sounds[i].m_clips[Random.Range(0, m_sounds[i].m_clips.Length)]);
            }

            yield return new WaitForSeconds(Vfx_wallHit.main.duration);
            Destroy(this.gameObject);//TODO: objectPooling
        }

        #region Physics

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision == m_spawnInCollider)
                return;
            if (collision.isTrigger)
                return;

            IDamageableObject hit = collision.gameObject.GetComponent<IDamageableObject>();
            if (hit != null) {
                hit.TakeDamage(this, m_damage, m_rb.velocity * m_rb.mass);
            }
            
            m_rb.isKinematic = true;
            m_rb.velocity = Vector3.zero;
            RaycastHit2D ray = Physics2D.Raycast(transform.position, -transform.right, 2);
            if(ray)
                transform.position = ray.point;

            m_col.enabled = false;
            r_bullet.SetActive(false);

            StartCoroutine(IEEffects(collision.tag));
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (m_spawnInCollider = collision)
                m_spawnInCollider = null;
        }

        #endregion
    }
}