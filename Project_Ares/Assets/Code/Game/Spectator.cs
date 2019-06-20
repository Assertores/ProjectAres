using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class Spectator : Sauerbraten {

        //static Spectator _singelton = null;

        #region Variables
        
        [Header("Reverences")]
        [SerializeField] Camera m_myCamera;

        [Header("Balancing")]
        [SerializeField] float m_minCameraSize = 15;
        [SerializeField] float m_padding = 10;

        

        #endregion
        #region MonoBehaviour

        /*void Awake() {
            //if (_singelton) {
            //    Destroy(this);
            //    return;
            //}
            //_singelton = this;
            
            m_myCamera = CameraController.s_singelton?.AddCamera();
        }
        private void OnDestroy() {
            //if (_singelton == this)
            //    _singelton = null;
            CameraController.s_singelton?.RemoveCamera(m_myCamera);
        }*/
        
        void Update() {
            if (Player.s_references.Count == 0)
                return;

            float minX = Player.s_references[0].transform.position.x;//TODO: eventuell nicht so geil, wenn keiner alive ist.
            float maxX = minX;
            float minY = Player.s_references[0].transform.position.y;
            float maxY = minY;

            foreach (var it in Player.s_references) {
                if (it.m_alive) {
                    minX = Mathf.Min(minX, it.transform.position.x);
                    maxX = Mathf.Max(maxX, it.transform.position.x);
                    minY = Mathf.Min(minY, it.transform.position.y);
                    maxY = Mathf.Max(maxY, it.transform.position.y);
                }
            }

            m_myCamera.orthographicSize = Mathf.Max(Mathf.Max((maxX - minX + 2* m_padding) / m_myCamera.aspect, maxY - minY + 2 * m_padding) / 2, m_minCameraSize);

            m_myCamera.transform.position = new Vector3(minX + (maxX-minX)/2, minY + (maxY-minY)/2, m_myCamera.transform.position.z);
        }

        #endregion
    }
}