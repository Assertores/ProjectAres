using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Collider2D))]
    public class ShockWave : MonoBehaviour, IHarmingObject, ITracer {

        #region Variables

        [SerializeField] AnimationCurve m_exand;
        [SerializeField] ContactFilter2D m_filter;
        [SerializeField] Sprite m_icon_;

        float m_startTime = 0;
        Collider2D m_col;

        #endregion
        #region MonoBehaviour

        private void Start() {
            m_col = GetComponent<Collider2D>();
        }

        private void Update() {
            float acseleration = m_exand.Evaluate(Time.time - m_startTime) * Time.unscaledDeltaTime;
            transform.localScale += new Vector3(acseleration, acseleration, acseleration);
            transform.position = m_owner.transform.position;
            Collider2D[] holder = new Collider2D[8];
            int count = m_col.OverlapCollider(m_filter, holder);
            for(int i = 0; i < count; i++) {
                holder[i].GetComponent<IDamageableObject>()?.Die(this, false);
            }
        }

        #endregion
        #region IHarmingObject

        public Sprite m_icon => m_icon_;

        public e_HarmingObjectType m_type => e_HarmingObjectType.SHOCKWAVE;

        public Player m_owner { get; private set; } = null;
        
        #endregion
        #region ITracer

        public IHarmingObject m_trace => this;

        public Rigidbody2D Init(IHarmingObject trace) {
            return null;
        }

        #endregion

        public void Init(Player owner) {
            m_owner = owner;
            m_startTime = Time.time;
        }
    }
}