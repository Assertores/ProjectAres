using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class FollowObject : MonoBehaviour {

        [SerializeField] Transform m_target;

        Vector3 m_offset;

        void Start() {
            if (!m_target) {
                Destroy(this);
                return;
            }
            m_offset = m_target.position - transform.position;
        }

        void Update() {
            transform.position = m_target.position + m_offset;
        }
    }
}