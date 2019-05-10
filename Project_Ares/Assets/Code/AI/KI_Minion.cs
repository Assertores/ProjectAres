using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {

    public class KI_Minion : MonoBehaviour, IControl {

        #region Variables

        Player my_Player;
        const int maxNearest = 3;
        List<Player> nearestPlayers = new List<Player>();
        List<float> distances;

        #endregion

        #region MonoBehaviour

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneSwap;
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneSwap;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion

        #region IControl

        public Vector2 m_dir { get; set; }
        public Action StartShooting { get; set; }
        public Action StopShooting { get; set; }
        public Action Dash { get; set; }
        public Action<int> SelectWeapon { get; set; }
        public Action<bool> ChangeName { get; set; }
        public Action<int, bool> ChangeCharacter { get; set; }
        public Action<int, bool> ChangeWeapon { get; set; }
        public Action<int> UseItem { get; set; }
        public Action Disconnect { get; set; }
        public Action<bool> ShowStats { get; set; }

        public void DoDisconect() {
            Disconnect?.Invoke();
        }

        #endregion

        void SceneSwap(UnityEngine.SceneManagement.Scene loadedScene, UnityEngine.SceneManagement.LoadSceneMode loadmode)
        {
            foreach (Player other in Player.s_references)
            {
                if (this == other.m_control)
                {
                    my_Player = other;
                    break;
                }
            }

            UpdateNearest();
        }

        //waffenwechsel = stopshooting
        //waffenwechsel = stopshooting

        void UpdateNearest()
        {
            nearestPlayers.Clear();
            distances.Clear();
            float nextDistance;

            foreach (Player other in Player.s_references)
            {
                if (other == my_Player)
                    continue;

                nextDistance = (other.transform.position - my_Player.transform.position).magnitude;
                if (nearestPlayers.Count < maxNearest)
                {
                    nearestPlayers.Add(other);
                    distances.Add(nextDistance);

                    SortInLast();
                }
                else if (nextDistance < distances[maxNearest - 1])
                {
                    nearestPlayers[maxNearest - 1] = other;
                    distances[maxNearest - 1] = nextDistance;

                    SortInLast();
                }
            }
            //nearestPlayers.Sort(delegate (Player a, Player b)
            //{
            //    return (a.transform.position - my_Player.transform.position).sqrMagnitude
            //    .CompareTo(
            //      (a.transform.position - my_Player.transform.position).sqrMagnitude);
            //});
        }

        void SortInLast()
        {
            int nextPos = nearestPlayers.Count - 1;
            float shiftedDisstance = distances[nextPos];
            Player shiftedPlayer = nearestPlayers[nextPos];

            while (nextPos - 1 >= 0 && distances[nextPos] < distances[nextPos - 1])
            {
                distances[nextPos] = distances[nextPos - 1];
                nearestPlayers[nextPos] = nearestPlayers[nextPos - 1];
                nextPos -= 1;
            }
            if (nextPos != nearestPlayers.Count - 1)
            {
                distances[nextPos] = shiftedDisstance;
                nearestPlayers[nextPos] = shiftedPlayer;
            }
        }
    }
}