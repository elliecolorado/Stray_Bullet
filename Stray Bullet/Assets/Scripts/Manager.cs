using Photon.Chat.UtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Com.Elrecoal.Stray_Bullet
{

    public class Manager : MonoBehaviour
    {

        public string player_prefab;

        public Transform spawn_point;

        private IEnumerator Start()
        {

            yield return new WaitUntil(() => PhotonNetwork.InRoom);

            Spawn();

        }

        public void Spawn()
        {

            PhotonNetwork.Instantiate(player_prefab, spawn_point.position, spawn_point.rotation);

        }

    }

}