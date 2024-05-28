using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace Com.Elrecoal.Stray_Bullet
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public static ProfileData myProfile = new ProfileData();
        public TMP_InputField roomNameField;
        public Slider maxPlayersSlider;
        public TMP_Text maxPlayersValue;
        public TMP_Text mapValue;
        public GameObject matchNotFoundError;
        public Map[] maps;
        private int currentMap = 0;

        public TMP_Text userNameText;
        public TMP_Text userDeathsText;
        public TMP_Text userPlayedMatchesText;
        public TMP_Text userSignupDate;

        public void Awake()
        {

            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();

            if (roomNameField != null && mapValue != null && maxPlayersSlider != null && maxPlayersValue != null)
            {
                roomNameField.text = "Nombre por defecto";

                currentMap = 0;
                mapValue.text = "Mapa: " + maps[currentMap].name;

                maxPlayersSlider.value = maxPlayersSlider.maxValue;
                maxPlayersValue.text = Mathf.RoundToInt(maxPlayersSlider.value).ToString();
            }
            if (userDeathsText != null && userPlayedMatchesText != null && userSignupDate != null)
            {
                userDeathsText.text = "Muertes: " + myProfile.deaths;
                userPlayedMatchesText.text = "Partidas totales: " + myProfile.played_matches;
                userSignupDate.text = "Fecha de registro: " + myProfile.signup_date;
            }
            if (userNameText != null) userNameText.text = myProfile.username;


        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected");

            PhotonNetwork.JoinLobby();
            base.OnConnectedToMaster();
        }

        public override void OnJoinedRoom()
        {
            StartGame();
            base.OnJoinedRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            matchNotFoundError.SetActive(true);
            base.OnJoinRandomFailed(returnCode, message);
        }

        public void Connect()
        {
            Debug.Log("Connecting...");
            PhotonNetwork.GameVersion = "0.0.0.0";
            PhotonNetwork.ConnectUsingSettings();
        }

        public void Join()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void Create()
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = (byte)maxPlayersSlider.value;

            options.CustomRoomPropertiesForLobby = new string[] { "map" };

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("map", currentMap);
            options.CustomRoomProperties = properties;

            PhotonNetwork.CreateRoom(roomNameField.text, options);
        }

        public void ChangeMap()
        {
            currentMap++;
            if (currentMap >= maps.Length) currentMap = 0;
            mapValue.text = "Mapa: " + maps[currentMap].name;
        }

        public void changeMaxPlayersSlider(float t_value)
        {
            maxPlayersValue.text = Mathf.RoundToInt(t_value).ToString();
        }


        public void StartGame()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel(maps[currentMap].name);
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            // Si el host se desconecta, cierra la sala y carga la escena del menu principal
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                SceneManager.LoadScene("MainMenu");
            }
        }

        public void SaveSettings()
        {
            Data.SaveProfile(myProfile); //Guarda el perfil en un archivo local
        }

    }
}
