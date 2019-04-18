using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {

    public class KI_Minion : MonoBehaviour, IControl {

        #region Variables

        #endregion

        #region MonoBehaviour

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

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

        //waffenwechsel = stopshooting
        //waffenwechsel = stopshooting
    }
}