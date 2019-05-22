using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Camera))]
    public class CameraStartAdaption : MonoBehaviour {

        [SerializeField] BoxCollider2D m_boundingBox;

        void Awake() {
            if (!m_boundingBox) {
                Destroy(this);
                return;
            }
            float tmpZ = transform.position.z;
            transform.position = m_boundingBox.transform.position;
            transform.position += new Vector3(m_boundingBox.offset.x, m_boundingBox.offset.y, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y, tmpZ);
            Camera cam = GetComponent<Camera>();
            cam.orthographicSize = m_boundingBox.size.y / 2;
            if(cam.orthographicSize * cam.aspect < m_boundingBox.size.x / 2) {
                cam.orthographicSize = (m_boundingBox.size.x / 2) / cam.aspect;
            }

            Destroy(m_boundingBox);
            Destroy(this);
            return;
        }
    }
}