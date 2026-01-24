using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    // 1. PARA EL BOTÓN PLAY - va a Level1
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // 2. PARA EL BOTÓN SETTINGS - va a ExplanationScene
    public void GoToExplanation()
    {
        SceneManager.LoadScene("ExplanationScene");
    }

    // 3. PARA EL BOTÓN BACK - vuelve a MainMenu
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
