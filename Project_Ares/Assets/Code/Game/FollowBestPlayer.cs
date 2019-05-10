using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class FollowBestPlayer : MonoBehaviour {

        #region MonoBehaviour
        
        void Update() {
            transform.LookAt(Player.s_sortedRef[0].transform);
        }

        #endregion
    }
}