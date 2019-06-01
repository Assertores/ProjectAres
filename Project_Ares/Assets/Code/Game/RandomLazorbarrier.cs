using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC
{
    
    public class RandomLazorbarrier : MonoBehaviour
    {
        #region Variables

        [Header("Balancing")]
        [SerializeField] int m_switchTime;

        System.Random s_ranPosGen = new System.Random(0);
        float m_time;
        private int m_lastindex;

        #endregion
        // Start is called before the first frame update
        void Awake() {
            transform.position = GameManager.s_singelton.m_mapHandler.m_borders[0].position;
            m_time = Time.timeSinceLevelLoad;
            GameManager.s_singelton.m_mapHandler.m_borders[0].gameObject.SetActive(false);
            m_lastindex = 0;
        }

        // Update is called once per frame
        void Update() {
            if (m_switchTime <= Time.timeSinceLevelLoad - m_time) {
                int tmp = s_ranPosGen.Next(0, 4);
                if (tmp != m_lastindex) {
                    transform.position = GameManager.s_singelton.m_mapHandler.m_borders[tmp].position;
                    transform.rotation = Quaternion.Euler(0, 0, tmp * 90);
                    m_time = Time.timeSinceLevelLoad;
                    GameManager.s_singelton.m_mapHandler.m_borders[tmp].gameObject.SetActive(false);
                    GameManager.s_singelton.m_mapHandler.m_borders[m_lastindex].gameObject.SetActive(true);
                    m_lastindex = tmp;
                }
                
            }
        }

       /* private void OnCollisionEnter2D(Collision2D collision) {
            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                tmp.Die(null);
            } else {
                Destroy(collision.gameObject);
            }
        }*/
    }

   
}
