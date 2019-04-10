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
        

        [Header("Variables")]
        [SerializeField] float m_exitTime;
        private float m_time;
        private int m_collInd = 0;

        #endregion

        private void Start()
        {
            
        }

        private void Update()
        {
            if (m_collInd > 0)
            {
                if (m_exitTime + m_time <= Time.timeSinceLevelLoad)
                {
                    m_exitHatch.SetActive(false);
                    
                }

            }

            else {

                m_exitHatch.SetActive(true);
                
            }

        }
        #region Physics




        private void OnTriggerEnter2D(Collider2D collision)
        {
            m_collInd++;
            if(m_collInd == 1) m_time = Time.timeSinceLevelLoad;

        }

    
        private void OnTriggerExit2D(Collider2D collision)
        {
            
            m_collInd--;
        }

    }
}


        #endregion
    

