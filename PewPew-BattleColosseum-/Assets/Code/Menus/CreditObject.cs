using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Rigidbody2D))]
    public class CreditObject : MonoBehaviour, IDamageableObject {

        Rigidbody2D m_rb;

        #region MonoBehaviour

        private void Awake() {
            m_rb = GetComponent<Rigidbody2D>();
        }

        #endregion
        #region IDamageableObject

        public bool m_alive => true;

        public void Die(IHarmingObject source, bool doTeamDamage = true) {
            return;
        }

        public void TakeDamage(IHarmingObject source, float damage, Vector2 recoilDir, bool doTeamDamage = true) {
            m_rb.AddForce(recoilDir);
        }

        #endregion
    }
}