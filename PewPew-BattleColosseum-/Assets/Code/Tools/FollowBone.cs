using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace PPBC {
    public class FollowBone : MonoBehaviour {

        [SerializeField] SkeletonAnimation m_anim;
        [SerializeField] string m_boneName = "Weapon";
        Spine.Bone m_target;

        void Start() {
            if (!m_anim) {
                Destroy(this);
                return;
            }

            m_target = m_anim.skeleton.FindBone(m_boneName);
            if (m_target == null) {
                Destroy(this);
                return;
            }
        }

        void Update() {
            transform.localPosition = new Vector2(m_target.WorldX, m_target.WorldY);
        }
    }
}