using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    // 1. PARA EL BOTÓN PLAY - va a Level1
    public void StartGame()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMusic(1);

        SceneManager.LoadScene("Level1");
    }

    // 2. PARA EL BOTÓN SETTINGS - va a ExplanationScene
    public void GoToExplanation()
    {
        string current = SceneManager.GetActiveScene().name;
        Debug.Log("ESCENA ACTUAL: " + current);

        PlayerPrefs.SetString("PreviousScene", current);
        PlayerPrefs.Save();
        SceneManager.LoadScene("ExplanationScene");
    }

    // 3. PARA EL BOTÓN BACK - vuelve a la escena anterior
    public void GoBackToPreviousScene()
    {
        string previousScene = PlayerPrefs.GetString("PreviousScene", "MainMenu");

        if (string.IsNullOrEmpty(previousScene)) // ← CORREGIDO: previousScene
        {
            previousScene = "MainMenu";
        }

        SceneManager.LoadScene(previousScene);
    }

    // 4. PARA EL BOTÓN EXIT - cierra el juego
    public void ExitGame()
    {
        Debug.Log("Has cerrado el juego");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}