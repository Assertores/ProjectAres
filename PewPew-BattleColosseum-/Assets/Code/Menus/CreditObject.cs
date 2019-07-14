using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace PPBC {
    [RequireComponent(typeof(Rigidbody2D))]
    public class CreditObject : MonoBehaviour, IDamageableObject {

        [SerializeField] SkeletonAnimation r_anim;

        Rigidbody2D m_rb;

        #region MonoBehaviour

        private void Awake() {
            m_rb = GetComponent<Rigidbody2D>();

            StartAnim(StringCollection.A_IDLE, true);
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
    }
}