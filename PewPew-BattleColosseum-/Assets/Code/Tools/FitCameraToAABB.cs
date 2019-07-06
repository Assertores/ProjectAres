using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(BoxCollider2D))]
    public class FitCameraToAABB : MonoBehaviour {
        static FitCameraToAABB s_singelton = null;

        public static BoxCollider2D m_aABB { get; private set; }
        Camera m_cam;

        private void Awake() {
            if (s_singelton != null && s_singelton != this) {
                BoxCollider2D tmp = GetComponent<BoxCollider2D>();
                m_aABB.offset = tmp.offset;
                m_aABB.size = tmp.size;
                Destroy(this);
                return;
            }

            m_aABB = GetComponent<BoxCollider2D>();
            m_cam = GetComponent<Camera>();

            if (!m_cam) {
                Destroy(this);
                return;
            }

            s_singelton = this;
        }

        private void OnDestroy() {
            if(s_singelton == this) {
                s_singelton = null;
            }
        }

        private void FixedUpdate() {
            m_cam.orthographicSize = m_aABB.size.y / 2;
            if (m_cam.orthographicSize * m_cam.aspect < m_aABB.size.x / 2) {
                m_cam.orthographicSize = (m_aABB.size.x / 2) / m_cam.aspect;
            }
        }
    }
}