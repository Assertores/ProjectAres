using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Camera))]
    public class CameraShake : MonoBehaviour {

        static List<CameraShake> s_reference = new List<CameraShake>();

        #region Variables

        Camera m_camera;
        Vector3 m_starPos;

        float m_magnitude = 0;
        float m_currentMagnitude = 0;
        float m_time = 0;
        float m_startTime = 0;

        #endregion
        #region MonoBehaviour

        private void Awake() {
            s_reference.Add(this);
        }
        private void OnDestroy() {
            s_reference.Remove(this);
        }

        private void Start() {
            m_camera = GetComponent<Camera>();
            m_starPos = transform.position;
        }

        void Update() {
            if(m_time > 0) {
                float duration = Time.timeSinceLevelLoad - m_startTime;
                m_currentMagnitude = m_magnitude * (1 - duration / m_time);

                if (duration > m_time) {
                    m_time = 0;
                    return;
                }

                Vector3 dir = Random.insideUnitCircle.normalized;
                transform.position = m_starPos + dir * m_currentMagnitude;
            }
        }

        #endregion

        public static void DoCamerashake(float magnitude, float time) {
            foreach(var it in s_reference) {
                if(it.m_currentMagnitude < magnitude) {
                    it.m_magnitude = magnitude;
                    it.m_time = time;
                    it.m_startTime = Time.timeSinceLevelLoad;
                }
                
            }
        }
    }
}