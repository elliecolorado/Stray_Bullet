using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Com.Elrecoal.Stray_Bullet
{

    public class Manager : MonoBehaviour
    {
        public string player_prefab;
        public Transform[] spawn_points;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => PhotonNetwork.InRoom);
            Spawn();
        }

        public void Spawn()
        {
            Transform t_spawn = spawn_points[UnityEngine.Random.Range(0,spawn_points.Length)];
            PhotonNetwork.Instantiate(player_prefab, t_spawn.position, t_spawn.rotation);
        }
    }
}