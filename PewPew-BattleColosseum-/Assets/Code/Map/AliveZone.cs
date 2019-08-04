using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(BoxCollider2D))]
    public class AliveZone : MonoBehaviour, IHarmingObject, ITracer {
        [SerializeField] Sprite m_icon_;

        bool m_active = false;

        public Sprite m_icon => m_icon_;

        public e_HarmingObjectType m_type => e_HarmingObjectType.DEATHZONE;

        public Player m_owner => null;

        public IHarmingObject m_trace => this;

        public Rigidbody2D Init(IHarmingObject trace) {
            return null;
        }

        private void Start() {
            BoxCollider2D col = GetComponent<BoxCollider2D>();
            col.size = FitCameraToAABB.m_aABB.size;
            col.offset = FitCameraToAABB.m_aABB.offset;

            TransitionHandler.ReadyToStart += SetActive;
        }

        void SetActive() {
            TransitionHandler.ReadyToStart -= SetActive;

            m_active = true;
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (!m_active)
                return;
            
            IDamageableObject obj = collision.GetComponent<IDamageableObject>();
            if (obj != null) {
                obj.Die(this);
            }
        }
    }
}