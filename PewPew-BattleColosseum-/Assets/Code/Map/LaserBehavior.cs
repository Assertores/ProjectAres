using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class LaserBehavior : MonoBehaviour, IHarmingObject, ITracer {

        public static LaserBehavior s_singelton { get; private set; }

        #region Variables
        [Header("References")]
        [SerializeField] Animation r_animation;
        [SerializeField] Sprite m_icon_;
        [Header("Lagecy")]
        [SerializeField] ParticleSystem fx_laserStart;
        [SerializeField] ParticleSystem fx_laserLoop;
        [SerializeField] ParticleSystem fx_laserEnd;
        [SerializeField] ContactFilter2D m_contFilter;

        AudioSource SFX_laserOff;
        AudioSource SFX_laserOn;

        List<GameObject> collisionObjects = new List<GameObject>();
        BoxCollider2D m_collider;

        int m_lastIndex = -1;

        System.Random m_randomDevice;

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
            SFX_laserOff = fx_laserEnd.GetComponent<AudioSource>();
            SFX_laserOn = fx_laserStart.GetComponent<AudioSource>();

            m_randomDevice = new System.Random(DataHolder.s_maps[DataHolder.s_currentMap].m_name.GetHashCode());
        }

        #endregion
        #region IHarmingObject

        public Sprite m_icon => m_icon_;

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
            if (!m_collider)
                m_collider = GetComponent<BoxCollider2D>();
            
            int newIndex;
            while (m_lastIndex == (newIndex = m_randomDevice.Next(0, LaserSpawner.s_references.Count))) ;
            m_lastIndex = newIndex;

            r_animation.Play();
        }

        #region AnimCalls

        public void DoEndEffect() {
            fx_laserLoop.Stop();
            fx_laserEnd.Play();
        }

        public void DoStartEffect() {
            fx_laserStart.Play();
        }

        public void DoLoopEffect() {
            fx_laserLoop.Play();
        }

        public void DoLaserSpawnerEffect() {
            LaserSpawner.s_references[m_lastIndex].fx_on.SetActive(true);
            LaserSpawner.s_references[(m_lastIndex + 1) % LaserSpawner.s_references.Count].fx_on.SetActive(true);
        }

        public void StopLaserSpawnerEffect() {
            LaserSpawner.s_references[m_lastIndex].fx_on.SetActive(false);
            LaserSpawner.s_references[(m_lastIndex + 1) % LaserSpawner.s_references.Count].fx_on.SetActive(false);
        }

        public void Move() {
            transform.position = LaserSpawner.s_references[m_lastIndex].transform.position;
            Vector3 target = LaserSpawner.s_references[(m_lastIndex + 1) % LaserSpawner.s_references.Count].transform.position;
            transform.rotation = Quaternion.LookRotation(transform.forward, new Vector2(-(target.y - transform.position.y), target.x - transform.position.x));
            transform.localScale = new Vector3((transform.position - target).magnitude, 1, 1);
        }

        public void ActivateOldProps() {
            for (int i = 0; i < collisionObjects.Count; i++) {
                collisionObjects[i].SetActive(true);
            }
            collisionObjects.Clear();
        }

        public void DeactivateNewProps() {
            Collider2D[] tmp = new Collider2D[10];
            int count = Physics2D.OverlapCollider(m_collider, m_contFilter, tmp);
            for (int i = 0; i < count; i++) {
                collisionObjects.Add(tmp[i].gameObject);
                tmp[i].gameObject.SetActive(false);
            }

            LaserSpawner.s_references[m_lastIndex].Reactivate();
            LaserSpawner.s_references[(m_lastIndex + 1) % LaserSpawner.s_references.Count].Reactivate();
        }

        public void DeactivateCollider() {
            m_collider.enabled = false;
        }

        public void ActivateCollider() {
            m_collider.enabled = true;
        }

        public void StopLoopSound() {

        }
        
        public void PlayDeactivateSound() {
            SFX_laserOff.Play();
        }

        public void PlayActivateSound() {
            SFX_laserOn.Play();
        }

        public void PlayLoopSound() {

        }

        #endregion
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