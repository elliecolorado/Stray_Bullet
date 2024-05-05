using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

namespace Com.Elrecoal.Stray_Bullet
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public TMP_InputField usernameField;
        //Recordatorio: Static se usa para poder acceder al atributo sin instanciar la clase (en este caso, acceder a myProfile sin instanciar un objeto Launcher) para poder acceder a ello desde escenas diferentes a la de main menu o settings
        public static User myProfile = new User();
        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            if (usernameField != null) usernameField.text = myProfile.username;
            Connect();
        }
        public override void OnConnectedToMaster() { base.OnConnectedToMaster(); }
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
            PhotonNetwork.GameVersion = "0.9.2.2";
            PhotonNetwork.ConnectUsingSettings();
        }
        public void Join() { PhotonNetwork.JoinRandomRoom(); }
        public void Create() { PhotonNetwork.CreateRoom("Sala de " + myProfile.username); }
        public void StartGame()
        {
            if (string.IsNullOrEmpty(myProfile.username))
            {
                myProfile.username = "RANDOM_USER_" + Random.Range(1, 9999);
            }
            //-----------------------------------Modificar segun parámetros (ahora mismo carga la pantalla de DevSpace por defecto, dar la posibilidad de elegir mapa)-----------------------------------
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1) PhotonNetwork.LoadLevel("Dev Space");
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            // Si el host se desconecta, cierra la sala y carga la escena del menu principal
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                SceneManager.LoadScene("Main Menu");
            }
        }
        public void SaveSettings()
        {
            //-----------------------------------
            //Modificar (que el nombre de usuario sea por defecto el que se introduce al registrarse)
            //-----------------------------------
            if (string.IsNullOrEmpty(usernameField.text)) myProfile.username = "RANDOM_USER_" + Random.Range(1, 9999);
            else myProfile.username = usernameField.text;
            usernameField.text = myProfile.username;
        }
    }
}
