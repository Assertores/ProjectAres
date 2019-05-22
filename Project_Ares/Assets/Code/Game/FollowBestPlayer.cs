using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class FollowBestPlayer : MonoBehaviour {

        #region MonoBehaviour
        
        void Update() {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.s_sortedRef[0].transform.position - transform.position), 0.2f);//framerate abhängig
        }

        #endregion
    }
}