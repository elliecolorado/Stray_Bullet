using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.Elrecoal.Stray_Bullet
{
    public class Pause : MonoBehaviourPunCallbacks
    {

        public static bool paused = false;
        private bool disconnecting = false;

        public void TogglePause()
        {
            if (disconnecting) return;

            paused = !paused;

            transform.GetChild(0).gameObject.SetActive(paused);
            Cursor.lockState = (paused) ? CursorLockMode.None : CursorLockMode.Confined;
            Cursor.visible = paused;
        }

        public void Quit()
        {
            disconnecting = true;
            PhotonNetwork.LeaveRoom();
            Launcher.myProfile.played_matches += 1;
            SceneManager.LoadScene("MainMenu");
        }

        public override void OnLeftRoom()
        {
            // Carga la escena del lobby o menú principal
            SceneManager.LoadScene("MainMenu");
        }


    }
}