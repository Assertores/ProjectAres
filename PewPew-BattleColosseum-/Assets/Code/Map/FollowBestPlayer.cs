using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class FollowBestPlayer : MonoBehaviour {

        #region Variables
        [Header("Balancing")]
        [SerializeField] float m_lerpTime;
        [SerializeField] float m_crownOffset;
        #endregion
        void LateUpdate() {

            transform.position = Vector2.Lerp(transform.position, new Vector2(Player.s_sortRef[0].transform.position.x, Player.s_sortRef[0].transform.position.y + Player.s_sortRef[0].m_distanceToGround + m_crownOffset), m_lerpTime);

        }
    }
}