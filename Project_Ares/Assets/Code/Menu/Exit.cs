using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres
{
    [RequireComponent(typeof(Collider2D))]
    public class Exit : MonoBehaviour
    {

        #region Physics

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player tmp = collision.gameObject.GetComponent<Player>();
            if (tmp)
            {
                tmp.m_control.DoDisconect();
                if (Player.s_references.Count <= 0)
                {
                    MenuManager._singelton?.Exit();
                }
            }
        }

        #endregion
    }
}

