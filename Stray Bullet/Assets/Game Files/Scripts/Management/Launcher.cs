using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Com.Elrecoal.Stray_Bullet
{

    public class Launcher : MonoBehaviourPunCallbacks
    {

        public int scene;


        public void Awake()
        {

            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }

        public override void OnConnectedToMaster()
        {

            base.OnConnectedToMaster();

        }

        public override void OnJoinedRoom()
        {

            StartGame();

            base.OnJoinedRoom();

        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {

            Create();

            base.OnJoinRandomFailed(returnCode, message);

        }


        public void Connect()
        {

            PhotonNetwork.GameVersion = "0.0.0";

            PhotonNetwork.ConnectUsingSettings();

        }

        public void Join()
        {

            PhotonNetwork.JoinRandomRoom();

        }

        public void Create()
        {

            PhotonNetwork.CreateRoom("");

        }

        public void StartGame()
        {

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {

                //-----------------------------------Modificar segun requisitos (ahora mismo carga la pantalla de DevSpace)-----------------------------------
                PhotonNetwork.LoadLevel(scene); 

            }

        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            // Si el host se desconecta, cierra la sala y carga la escena del lobby
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                SceneManager.LoadScene("Main Menu");
            }
        }


    }

}
