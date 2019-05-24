using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(LineRenderer))]
    public class LaserPointer : MonoBehaviour {

        [SerializeField] LayerMask m_layerMask;

        LineRenderer m_laser;

        void Start() {
            m_laser = GetComponent<LineRenderer>();
            //m_laser.enabled = false;
        }

        private void FixedUpdate() {
            Debug.DrawRay(transform.position, transform.right);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right,1000f,m_layerMask);
            if (hit.collider == null) {
                m_laser.SetPosition(1, new Vector2(100, 0));
                return;
            }

            m_laser.SetPosition(1, new Vector2(Vector2.Distance(hit.point, transform.position),0));
        }
    }
}