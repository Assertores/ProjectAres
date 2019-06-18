using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
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

            m_dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);


            if (Input.GetButtonDown(StringCollection.FIRE)) {
                StartShooting?.Invoke();
            } else if (Input.GetButtonUp(StringCollection.FIRE)) {
                StopShooting?.Invoke();
            }

            if (Input.GetKeyUp(KeyCode.Space)) {
                ChangeWeapon?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                ChangeCharacter?.Invoke(true);
            }
            if (Input.GetKeyDown(KeyCode.V)) {
                ChangeCharacter?.Invoke(false);
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
        public Action<bool> ChangeCharacter { get; set; }
        public Action ChangeWeapon { get; set; }
        public Action Disconnect { get; set; }

        public void DoDisconect() {
            DataHolder.s_players[4] = false;
            Disconnect?.Invoke();
        }

        #endregion
    }
}