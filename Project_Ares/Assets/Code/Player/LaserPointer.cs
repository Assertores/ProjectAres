using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(LineRenderer))]
    public class LaserPointer : MonoBehaviour {

        LineRenderer m_laser;

        void Start() {
            m_laser = GetComponent<LineRenderer>();
            //m_laser.enabled = false;
        }

        private void Update() {
            Debug.DrawRay(transform.position, transform.right);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
            if (hit.collider == null) {
                print("hit nothing");
                return;
            }
            print(hit.collider.name);
            m_laser.SetPosition(1, new Vector2(Vector2.Distance(hit.point, transform.position),0));
        }
    }
}