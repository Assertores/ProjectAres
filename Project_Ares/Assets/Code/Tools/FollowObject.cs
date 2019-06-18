using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class FollowObject : MonoBehaviour {

        [SerializeField] Transform m_target;

        void Start() {
            if (!m_target) {
                Destroy(this);
                return;
            }
        }

        void Update() {
            transform.position = m_target.position;
        }
    }
}