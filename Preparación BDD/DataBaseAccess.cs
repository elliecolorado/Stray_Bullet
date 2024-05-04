using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class DataBaseAccess : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;
    public string phpLoginURL = "http://yourserver.com/login.php";
    public string phpRegisterURL = "http://yourserver.com/register.php";

    public void OnRegisterButton()
    {
        StartCoroutine(Register(usernameInput.text, passwordInput.text));
    }

    public void OnLoginButton()
    {
        StartCoroutine(Login(usernameInput.text, passwordInput.text));
    }

    IEnumerator Register(string user, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", user);
        form.AddField("pass", pass);

        using (UnityWebRequest www = UnityWebRequest.Post(phpRegisterURL, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                errorText.text = "Error: " + www.error;
            }
            else
            {
                errorText.text = www.downloadHandler.text;
            }
        }
    }

    IEnumerator Login(string user, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", user);
        form.AddField("pass", pass);

        using (UnityWebRequest www = UnityWebRequest.Post(phpLoginURL, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                errorText.text = "Error: " + www.error;
            }
            else
            {
                errorText.text = www.downloadHandler.text;
            }
        }
    }
}
