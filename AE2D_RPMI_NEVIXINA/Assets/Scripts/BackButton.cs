using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void GoBackToPreviousScene()
    {
        // Cargar la escena que estaba antes (puedes guardarla en PlayerPrefs o variable estática)
        string previousScene = PlayerPrefs.GetString("PreviousScene", "MainMenu");
        SceneManager.LoadScene(previousScene);
    }
}