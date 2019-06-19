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

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                ChangeCharacter?.Invoke(true);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                ChangeCharacter?.Invoke(false);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                ChangeCharacter?.Invoke(true);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                ChangeCharacter?.Invoke(false);
            }

            if (Input.GetKeyUp(KeyCode.Escape)) {
                OptionMenu?.Invoke();
                //if(Time.timeScale > 0) {
                //    Time.timeScale = 0;
                //} else {
                //    Time.timeScale = 1;
                //}
                //PauseAudioHandler.UpdateAudio();
            }

            if (Input.GetKeyUp(KeyCode.Return)) {
                Accept?.Invoke();
            }
        }

        #endregion
        #region IControl

        public Vector2 m_dir { get; set; }
        public Action StartShooting { get; set; }
        public Action StopShooting { get; set; }
        public Action<bool> ChangeCharacter { get; set; }
        public Action ChangeWeapon { get; set; }
        public Action OptionMenu { get; set; }
        public Action Accept { get; set; }
        public Action<bool> ChangeType { get; set; }
        public Action Disconnect { get; set; }

        public void DoDisconect() {
            DataHolder.s_players[4] = false;
            Disconnect?.Invoke();
        }

        #endregion
    }
}