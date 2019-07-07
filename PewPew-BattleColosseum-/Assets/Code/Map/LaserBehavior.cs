using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class LaserBehavior : MonoBehaviour {

        #region Variables
        [Header("References")]
        [SerializeField] ParticleSystem fx_laserStart;
        [SerializeField] ParticleSystem fx_laserLoop;
        [SerializeField] ParticleSystem fx_laserEnd;
        [SerializeField] ContactFilter2D m_contFilter;

        List<GameObject> collisionObjects = new List<GameObject>();
        BoxCollider2D m_collider;

        #endregion
        #region MonoBehaviour

        public static LaserBehavior s_singelton { get; private set; }

        void Awake() {
            if (s_singelton != null && s_singelton != this) {
                Destroy(gameObject);
                return;
            }

            if (!fx_laserStart) {
                print("no laserstart effect");
                Destroy(gameObject);
                return;
            }
            if (!fx_laserLoop) {
                print("no laserloop effect");
                Destroy(gameObject);
                return;
            }
            if (!fx_laserEnd) {
                print("no laserend effect");
                Destroy(gameObject);
                return;
            }

            s_singelton = this;
        }

        void OnDestroy() {
            if (s_singelton == this)
                s_singelton = null;
        }

        private void Start() {
            m_collider = GetComponent<BoxCollider2D>();
        }

        #endregion

        public IEnumerator ChangePosition() {
            StopAllCoroutines();
            
            fx_laserEnd.Play();

            yield return new WaitForSeconds(fx_laserLoop.main.duration);

            for (int i = 0; i < collisionObjects.Count; i++) {
                collisionObjects[i].SetActive(true);
            }
            collisionObjects.Clear();
            int newIndex = Random.Range(0, LaserSpawner.s_references.Count);
            transform.position = LaserSpawner.s_references[newIndex].transform.position;
            Vector2 target = LaserSpawner.s_references[newIndex + 1 % LaserSpawner.s_references.Count].transform.position;
            transform.rotation = Quaternion.LookRotation(transform.forward, new Vector2(-(target.y - transform.position.y), target.x - transform.position.x));
            
            fx_laserStart.Play();


            Collider2D[] tmp = new Collider2D[10];
            int count = m_collider.OverlapCollider(m_contFilter, tmp);
            for (int i = 0; i < count; i++) {
                collisionObjects.Add(tmp[i].gameObject);
                tmp[i].gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(fx_laserStart.main.duration);

            fx_laserLoop.Play();
        }

        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                tmp.Die(null);
            } else {
                Destroy(collision.gameObject);
            }
        }

        #endregion
    }
}