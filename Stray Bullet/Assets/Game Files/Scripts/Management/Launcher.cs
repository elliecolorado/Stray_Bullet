using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Elrecoal.Stray_Bullet
{

    public class Launcher : MonoBehaviourPunCallbacks
    {

        #region Unity Methods

        public void Awake()
        {

            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }

        public override void OnConnectedToMaster()
        {

            Join();

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

        #endregion

        #region Personal Methods

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
                PhotonNetwork.LoadLevel(1); 

            }

        }

        #endregion

    }

}
