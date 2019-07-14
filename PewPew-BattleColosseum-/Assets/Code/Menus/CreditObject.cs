using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace PPBC {
    [RequireComponent(typeof(Rigidbody2D))]
    public class CreditObject : MonoBehaviour, IDamageableObject {

        [SerializeField] SkeletonAnimation r_anim;

        Rigidbody2D m_rb;
        Vector2 m_inVel;

        #region MonoBehaviour

        private void Awake() {
            m_rb = GetComponent<Rigidbody2D>();

            StartAnim(StringCollection.A_IDLE, true);
        }

        private void FixedUpdate() {
            m_inVel = m_rb.velocity;
        }

        #endregion
        #region IDamageableObject

        public bool m_alive => true;

        public void Die(IHarmingObject source, bool doTeamDamage = true) {
            return;
        }

        public void TakeDamage(IHarmingObject source, float damage, Vector2 recoilDir, bool doTeamDamage = true) {
            m_rb.AddForce(recoilDir);
            StartCoroutine(IEHit());
        }

        #endregion

        IEnumerator IEHit() {
            yield return new WaitForSeconds(StartAnim(StringCollection.A_HIT));
            StartAnim(StringCollection.A_IDLE, true);
        }

        /// <summary>
        /// starts the animation
        /// </summary>
        /// <param name="animName"></param>
        /// <param name="loop"></param>
        /// <param name="track"></param>
        /// <returns>the time of the animation in seconds (-1 if looping) (float.MinValue if animation is already running or Anim not found)</returns>
        public float StartAnim(string animName, bool loop = false, int track = 0) {
            /*if (GetCurrentAnim() == animName) {
                return float.MinValue;
            }*/

            if (r_anim != null) {
                float time = r_anim.AnimationState.SetAnimation(track, animName, loop).Animation.Duration;
                return loop ? -1 : time;
            }
            return float.MinValue;
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            Vector2 tmp = collision.contacts[0].normal;
            if (Vector2.Dot(m_inVel.normalized, tmp) < 0) {
                m_rb.velocity = /*m_bounciness * */(Vector2.Reflect(m_inVel, tmp));
            }
            StartAnim(StringCollection.A_IMPACT);
        }
    }
}