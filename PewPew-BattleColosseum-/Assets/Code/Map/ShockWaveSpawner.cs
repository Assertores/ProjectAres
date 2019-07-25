using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class ShockWaveSpawner : MonoBehaviour {

        static ShockWaveSpawner s_singelton;

        #region Variables

        [SerializeField] GameObject p_shockWave;
        [SerializeField] float m_delay;
        [SerializeField] float m_newTimeScale= 0.25f;

        float m_startTimeScale;

        #endregion
        #region MonoBehaviour

        private void Awake() {
            if(s_singelton != null && s_singelton != this) {
                Destroy(this);
                return;
            }

            if (!p_shockWave) {
                print("no shockwave set");
                Destroy(this);
                return;
            }
            if (!p_shockWave.GetComponent<ShockWave>()) {
                print("shockwave prefab has noch Shockwave script");
                Destroy(this);
                return;
            }

            s_singelton = this;
        }

        private void OnDestroy() {
            if (s_singelton == this)
                s_singelton = null;
        }

        #endregion

        bool h_once = false;
        public static float SpawnShockWaves() {
            if (s_singelton.h_once)
                return float.MinValue;
            s_singelton.h_once = true;
            return s_singelton.DoSpawnShockWaves();
        }

        float DoSpawnShockWaves() {
            m_startTimeScale = Time.timeScale;
            Time.timeScale = m_newTimeScale;
            foreach (var it in Player.s_references) {
                if (MatchManager.s_currentMatch.m_teamHolder != null ?
                    it.m_team == Player.s_sortRef[0].m_team :
                    it == Player.s_sortRef[0]) {

                    Instantiate(p_shockWave, it.transform.position, it.transform.rotation).GetComponent<ShockWave>().Init(it);
                }
                it.InControle(false);
            }

            StartCoroutine(IERestoreTimeScale());

            return m_delay;
        }

        IEnumerator IERestoreTimeScale() {
            yield return new WaitForSecondsRealtime(m_delay + 0.1f);
            Time.timeScale = m_startTimeScale;
        }
    }
}