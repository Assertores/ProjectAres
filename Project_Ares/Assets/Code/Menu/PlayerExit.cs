using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerExit : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] GameObject m_exitHatch;
        [SerializeField] GameObject m_exit;

        [Header("Variables")]
        [SerializeField] float m_exitTime;
        private float m_time;
        private int m_collInd = 0;

        #endregion

        private void Start()
        {
            m_time = Time.timeSinceLevelLoad;
        }
        #region Physics


        private void OnTriggerEnter2D(Collider2D collision)
        {
            m_collInd++;
            while (m_collInd != 0)
            {
                if (m_exitTime + m_time <= Time.timeSinceLevelLoad)
                {
                    m_exitHatch.SetActive(false);
                    Player tmp = collision.gameObject.GetComponent<Player>();
                    if (tmp)
                    {
                        tmp.Disconect();
                        if (Player.s_references.Count <= 0)
                        {
                            MenuManager._singelton?.Exit();

                        }


                    }

                }

            }
        }

    }
}


        #endregion
    

