using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // ASIGNAR EN EL INSPECTOR:
    public GameObject pausePanel;    // El panel grande con botones
    public Button pauseButton;       // El botón pequeño con imagen de pausa ⏸️
    public Button resumeButton;      // Botón "Continuar" dentro del panel
    public Button settingsButton;    // Botón "Opciones" (opcional)
    public Button quitButton;        // Botón "Salir al menú" dentro del panel

    void Start()
    {
        // 1. Ocultar panel al inicio
        pausePanel.SetActive(false);
        
        // 2. Configurar clicks de botones
        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitToMenu);
        
        // 3. Opcional: botón de ajustes
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OpenSettings);
    }

    void Update()
    {
        // 4. También pausar con tecla ESC o P (opcional)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    // 5. Método para abrir/cerrar pausa
    public void TogglePause()
    {
        bool pausar = !pausePanel.activeSelf;
        
        pausePanel.SetActive(pausar);           // Mostrar/ocultar panel
        pauseButton.gameObject.SetActive(!pausar); // Ocultar/mostrar botón ⏸️
        Time.timeScale = pausar ? 0f : 1f;      // Pausar/Reanudar juego
        
        // 6. Notificar al GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.IsPaused = pausar;
    }

    // 7. Continuar juego
    void ResumeGame()
    {
        pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true); // Mostrar botón ⏸️
        Time.timeScale = 1f;
        
        if (GameManager.Instance != null)
            GameManager.Instance.IsPaused = false;
    }

    // 8. Abrir ajustes (puedes dejarlo vacío o crear otro panel)
    void OpenSettings()
    {
        Debug.Log("Abrir ajustes...");
        // Aquí puedes activar otro panel de opciones
    }

    // 9. Volver al menú principal
    void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Cambia "MainMenu" si tu escena tiene otro nombre
    }
}