using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class KeyboardControl : MonoBehaviour, IControl {

        #region Variables

        Camera m_myCamera = null;

        #endregion
        #region MonoBehaviour

        void Start() {
            //if (CameraControler._singelton)
            //    _myCamera = CameraControler._singelton.AddCamera();
        }
        private void OnDestroy() {
            //if (_myCamera)
            //    CameraControler._singelton.RemoveCamera(_myCamera);
        }

        void Update() {

            m_dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

            if (Input.GetButtonDown(StringCollection.FIRE)) {
                StartShooting?.Invoke();
            } else if (Input.GetButtonUp(StringCollection.FIRE)) {
                StopShooting?.Invoke();
            }

            if (Input.GetKeyUp(KeyCode.Alpha1)) {
                ChangeWeapon?.Invoke(0);
            } else if (Input.GetKeyUp(KeyCode.Alpha2)) {
                ChangeWeapon?.Invoke(1);
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                Dash?.Invoke();
            }

            //if (_myCamera) {
            //    _myCamera.transform.position = new Vector3(transform.position.x, transform.position.y, _myCamera.transform.position.z);
            //}

            if (Input.GetKeyUp(KeyCode.Escape)) {
                Disconnect?.Invoke();
            }
        }

        #endregion
        #region IControl

        public Vector2 m_dir { get; set; }
        public Action StartShooting { get; set; }
        public Action StopShooting { get; set; }
        public Action Dash { get; set; }
        public Action<int> SelectWeapon { get; set; }
        public Action<int> ChangeWeapon { get; set; }
        public Action<int> UseItem { get; set; }
        public Action Disconnect { get; set; }

        #endregion
    }
}