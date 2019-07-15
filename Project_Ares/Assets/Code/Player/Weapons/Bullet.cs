using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class Bullet : Sauerbraten, IHarmingObject {

        #region Variables
        [Header("References")]
        [SerializeField] ParticleSystem FX_wallHit;
        [SerializeField] GameObject m_spriteParent;
        [SerializeField] AudioClip[] SFX_wallHit;
        [SerializeField] AudioSource m_audioSource;
        
        [Header("Balancing")]
        [SerializeField] float m_killDistance = 1000;
        [SerializeField] float m_damage = 1;

        Player m_source = null;
        Sprite m_icon = null;

        Rigidbody2D m_rb;

        #endregion
        #region MonoBehaviour

        private void Update() {
            if (Vector2.Distance(transform.position, Vector2.zero) > m_killDistance) {
                Destroy(gameObject);
            }
        }

        #endregion
        #region IHarmingObject

        public Rigidbody2D Init(Player reverence, Sprite icon) {
            m_rb = GetComponent<Rigidbody2D>();
            if (!m_rb) {
                Destroy(gameObject);
                return null;
            }

            m_source = reverence;
            m_icon = icon;
            return m_rb;
        }

        #endregion
        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
            
            if(m_source && collision.gameObject == m_source.gameObject) {//null reference test
                return;
            }

            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                tmp.TakeDamage(m_damage, m_source, m_rb.velocity * m_rb.mass, m_icon);
            }
            //CameraShake.DoCamerashake(0.01f, 0.1f);
            
            StartCoroutine(BulletDie(0.1f));
            
            
        }

        #endregion

        IEnumerator BulletDie(float m_wait) {
            int index = Random.Range(0, SFX_wallHit.Length);
            m_audioSource.PlayOneShot(SFX_wallHit[index]);
            m_rb.velocity = new Vector2(0,0);
            m_spriteParent.SetActive(false);
            FX_wallHit.Play();
            yield return new WaitForSeconds(m_wait);
            Destroy(gameObject);

        }
    }
}
