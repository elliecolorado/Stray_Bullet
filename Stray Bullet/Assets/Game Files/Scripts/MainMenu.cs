using UnityEngine;

namespace Com.Elrecoal.Stray_Bullet
{
    public class MainMenu : MonoBehaviour
    {

        public Launcher launcher;

        public void JoinMatch()
        {

            launcher.Join();

        }

        public void CreateMatch()
        {

            launcher.Create();

        }

        public void QuitGame()
        {

            Application.Quit();

        }

    }
}