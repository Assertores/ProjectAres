using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class LaserBehavior : MonoBehaviour, IHarmingObject, ITracer {

        public static LaserBehavior s_singelton { get; private set; }

        #region Variables
        [Header("References")]
        [SerializeField] ParticleSystem fx_laserStart;
        [SerializeField] ParticleSystem fx_laserLoop;
        [SerializeField] ParticleSystem fx_laserEnd;
        [SerializeField] ContactFilter2D m_contFilter;

        List<GameObject> collisionObjects = new List<GameObject>();
        BoxCollider2D m_collider;

        int m_lastIndex = -1;

        #endregion
        #region MonoBehaviour

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
        #region IHarmingObject

        public Sprite m_icon => null;

        public e_HarmingObjectType m_type => e_HarmingObjectType.LASOR;

        public Player m_owner => null;

        #endregion
        #region ITracer

        public IHarmingObject m_trace => this;

        public Rigidbody2D Init(IHarmingObject trace) {
            return null;
        }

        #endregion

        public void ChangePosition() {
            StopAllCoroutines();
            StartCoroutine(IEChangePosition());
        }

        IEnumerator IEChangePosition() {

            fx_laserLoop.Stop();

            fx_laserEnd.Play();

            int newIndex;
            while (m_lastIndex == (newIndex = Random.Range(0, LaserSpawner.s_references.Count))) ;
            m_lastIndex = newIndex;

            LaserSpawner.s_references[newIndex].fx_on.SetActive(true);
            LaserSpawner.s_references[(newIndex + 1) % LaserSpawner.s_references.Count].fx_on.SetActive(true);

            yield return new WaitForSeconds(fx_laserLoop.main.duration);

            for (int i = 0; i < collisionObjects.Count; i++) {
                collisionObjects[i].SetActive(true);
            }
            collisionObjects.Clear();
            

            transform.position = LaserSpawner.s_references[newIndex].transform.position;
            Vector3 target = LaserSpawner.s_references[(newIndex + 1) % LaserSpawner.s_references.Count].transform.position;
            transform.rotation = Quaternion.LookRotation(transform.forward, new Vector2(-(target.y - transform.position.y), target.x - transform.position.x));
            transform.localScale = new Vector3((transform.position - target).magnitude, 1, 1);
            
            fx_laserStart.Play();

            yield return new WaitForSeconds(Time.fixedDeltaTime);
            
            if(!m_collider)
                m_collider = GetComponent<BoxCollider2D>();

            //int count = m_collider.OverlapCollider(m_contFilter, tmp);
            Collider2D[] tmp = new Collider2D[10];
            int count = Physics2D.OverlapCollider(m_collider, m_contFilter, tmp);
            for (int i = 0; i < count; i++) {
                collisionObjects.Add(tmp[i].gameObject);
                tmp[i].gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(fx_laserStart.main.duration);

            LaserSpawner.s_references[newIndex].fx_on.SetActive(false);
            LaserSpawner.s_references[(newIndex + 1) % LaserSpawner.s_references.Count].fx_on.SetActive(false);

            fx_laserLoop.Play();
        }

        #region Physics

        private void OnTriggerEnter2D(Collider2D collision) {
            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                tmp.Die(this);
            } else {
                Destroy(collision.gameObject);
            }
        }
        
        #endregion
    }
}