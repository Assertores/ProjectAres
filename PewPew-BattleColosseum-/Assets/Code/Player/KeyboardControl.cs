using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class KeyboardControl : MonoBehaviour, IControl {

        #region MonoBehaviour
        
        void Update() {

        }

        #endregion
        #region IControl

        public int m_index { get; set; }

        public Vector2 m_dir { get; private set; }

        public Action TriggerDown { get; set; }
        public Action TriggerUp { get; set; }
        public Action ChangeWeapon { get; set; }
        public Action<bool> ChangeCharacter { get; set; }
        public Action Accept { get; set; }
        public Action<bool> ChangeType { get; set; }
        public Action Disconnect { get; set; }
        public void DoDisconnect() {
            DataHolder.s_players[m_index] = false;
            Disconnect?.Invoke();
        }

        #endregion
    }
}