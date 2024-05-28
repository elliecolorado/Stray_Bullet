using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using Com.Elrecoal.Stray_Bullet;

public class SessionManager : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TMP_Text messageText;

    public void OnSignupButtonClick() { StartCoroutine(Signup()); }
    public void OnLoginButtonClick() { StartCoroutine(Login()); }

    public void LogInScreen() { SceneManager.LoadScene("LogInMenu"); }
    public void SignUpScreen() { SceneManager.LoadScene("SignUpMenu"); }

    IEnumerator Signup()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://90.74.45.43/stray_bullet/signup.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                messageText.text = "Error de conexión";
            }
            else
            {
                if (www.downloadHandler.text == "success")
                {
                    messageText.text = "Registro exitoso. Inicia sesion para continuar.";
                }
                else if (www.downloadHandler.text == "user_exists")
                {
                    messageText.text = "El usuario ya existe";
                }
                else if (www.downloadHandler.text == "error")
                {
                    messageText.text = "Error en el registro";
                }
            }
        }
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text);
        form.AddField("password", passwordField.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://90.74.45.43/stray_bullet/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                messageText.text = "Error de conexión";
            }
            else
            {
                if (www.downloadHandler.text == "user_not_found" || www.downloadHandler.text == "invalid_password")
                {
                    messageText.text = "Usuario o contraseña incorrectos";
                }
                else
                {
                    // Parse JSON response to ProfileData object
                    Launcher.myProfile = JsonUtility.FromJson<ProfileData>(www.downloadHandler.text);
                    Data.SaveProfile(Launcher.myProfile);
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
    }

}