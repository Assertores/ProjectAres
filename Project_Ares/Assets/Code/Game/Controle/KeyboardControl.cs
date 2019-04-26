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
                ChangeWeapon?.Invoke(0, false);
            } else if (Input.GetKeyUp(KeyCode.Alpha2)) {
                ChangeWeapon?.Invoke(1, false);
            }
            if (Input.GetKeyUp(KeyCode.Space)) {
                ChangeWeapon?.Invoke(1, true);
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                ChangeCharacter?.Invoke(1, true);
            }

            //if (_myCamera) {
            //    _myCamera.transform.position = new Vector3(transform.position.x, transform.position.y, _myCamera.transform.position.z);
            //}

            if (Input.GetKeyUp(KeyCode.Escape)) {
                if(Time.timeScale > 0) {
                    Time.timeScale = 0;
                } else {
                    Time.timeScale = 1;
                }
                PauseAudioHandler.UpdateAudio();
                //Disconnect?.Invoke();
            }
        }

        #endregion
        #region IControl

        public Vector2 m_dir { get; set; }
        public Action StartShooting { get; set; }
        public Action StopShooting { get; set; }
        public Action Dash { get; set; }
        public Action<int> SelectWeapon { get; set; }
        public Action<int, bool> ChangeCharacter { get; set; }
        public Action<int, bool> ChangeWeapon { get; set; }
        public Action<int> UseItem { get; set; }
        public Action Disconnect { get; set; }

        #endregion
    }
}