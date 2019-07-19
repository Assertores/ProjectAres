using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class CoopEdit : MonoBehaviour, IGameMode {

        #region Variables

        [Header("References")]
        [SerializeField] Sprite r_icon_;
        [SerializeField] string r_text_;
        [SerializeField] GameObject p_EditingHUD;

        #endregion

        private void Start() {
            foreach (Transform it in transform) {
                it.gameObject.SetActive(false);
            }
        }

        #region IGameMode

        public Sprite m_icon => r_icon_;

        public string m_name => StringCollection.M_COOPEDIT;

        public string m_text => r_text_;

        public bool m_isTeamMode => false;

        public System.Action<bool> EndGame { get; set; }

        public void StartTransition() {
            //Player transition
        }

        public void SetUpGame() {
            foreach (var it in Player.s_references) {
                it.EditAble(Instantiate(p_EditingHUD, it.transform).GetComponent<EditorHUDAndPlayerLogic>());
                it.Respawn(SpawnPoint.s_references[Random.Range(0, SpawnPoint.s_references.Count)].transform.position);
            }
        }

        public void StartGame() {
            foreach(Transform it in transform) {
                it.gameObject.SetActive(true);
            }
        }

        public void AbortGame() {
            foreach (Transform it in transform) {
                it.gameObject.SetActive(false);
            }

            foreach(var it in Player.s_references) {
                it.EditAble(null);
            }

            EndGame?.Invoke(false);
        }

        public void DoEndGame() {
            MapHandler.s_singelton.SaveMap(System.DateTime.Now.ToString());

            AbortGame();
        }

        public e_mileStones[] GetMileStones() {
            e_mileStones[] value = new e_mileStones[Player.s_references.Count];
            //impliment milestones
            return value;
        }

        public void PlayerDied(IHarmingObject killer, Player victim) {

            victim.Respawn(SpawnPoint.s_references[Random.Range(0, SpawnPoint.s_references.Count)].transform.position);
        }

        public void ScorePoint(Player scorer, float amount) {
        }

        #endregion
    }
}