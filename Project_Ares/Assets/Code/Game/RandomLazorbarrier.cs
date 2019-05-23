using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC
{
    
    public class RandomLazorbarrier : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] GameObject[] m_positions = new GameObject[4];
        [SerializeField] GameObject[] m_walls = new GameObject[4];
        [SerializeField] GameObject m_lazor;
        private int m_lastindex;       

        [Header("Balancing")]
        [SerializeField] int m_switchTime;

        static System.Random s_ranPosGen = new System.Random(0);
        float m_time;
        #endregion
        // Start is called before the first frame update
        void Start() {
            SetLazorPosition(m_positions[0].transform);
            m_time = Time.timeSinceLevelLoad;
            m_walls[0].SetActive(false);
            m_lastindex = 0;
        }

        // Update is called once per frame
        void Update() {
            if (m_switchTime <= Time.timeSinceLevelLoad - m_time) {
                int tmp = s_ranPosGen.Next(0, 4);
                if (tmp != m_lastindex) {
                    SetLazorPosition(m_positions[tmp].transform);
                    m_walls[tmp].SetActive(false);
                    m_walls[m_lastindex].SetActive(true);
                    m_lastindex = tmp;
                }
                
            }
        }

        void SetLazorPosition(Transform position ) {
            m_lazor.transform.position = position.transform.position;
            m_lazor.transform.rotation = position.transform.rotation;
            m_time = Time.timeSinceLevelLoad;
        }
    }

   
}
