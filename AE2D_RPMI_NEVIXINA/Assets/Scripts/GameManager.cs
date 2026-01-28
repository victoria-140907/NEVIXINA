using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Datos persistentes
    public int vidas = 3;
    public int frutas = 0;
    public float tiempoRestante = 120f;

    // ESTADO DE PAUSA
    public bool IsPaused { get; set; } = false;

    // Objetivo del nivel actual
    public int frutasObjetivo = 10;

    // UI - Asegúrate de asignar en Inspector
    public TextMeshProUGUI textoVidas;
    public TextMeshProUGUI textoFrutas;
    public TextMeshProUGUI textoTiempo;

    // Paneles de UI
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    // Pause System
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button quitButton;
    public Button optionsButton;

    // Referencia al AudioManager para control de música
    private AudioManager audioManager;

    // ===== INICIALIZACIÓN =====
    private void Awake()
    {
        Debug.Log("GameManager Awake llamado");
        
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager creado como Singleton");
        }
        else
        {
            Debug.Log("GameManager duplicado detectado. Destruyendo: " + gameObject.name);
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        Debug.Log("GameManager Start iniciado");
        
        // Obtener referencia al AudioManager
        audioManager = AudioManager.Instance;
        
        // Verificar asignaciones
        VerificarAsignaciones();
        
        // Configurar UI inicial
        ActualizarUI();
        
        // Configurar botones de pausa
        ConfigurarPausa();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("GameManager: Escena cargada - " + scene.name);
        
        // Si volvemos al menú principal, asegurarnos de que la música sea la correcta
        if (scene.name == "MainMenu" && audioManager != null)
        {
            // Pequeño delay para asegurar que el AudioManager ya procesó la escena
            StartCoroutine(DelayedMusicCheck(0.1f));
        }
    }

    IEnumerator DelayedMusicCheck(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Forzar música de menú si no está sonando
        if (audioManager != null)
        {
            audioManager.ForceChangeToMenuMusic();
        }
    }

    void VerificarAsignaciones()
    {
        Debug.Log("=== VERIFICANDO ASIGNACIONES ===");
        
        if (textoVidas == null) Debug.LogError("textoVidas NO asignado");
        else Debug.Log("textoVidas: " + textoVidas.name);
        
        if (textoFrutas == null) Debug.LogError("textoFrutas NO asignado");
        else Debug.Log("textoFrutas: " + textoFrutas.name);
        
        if (textoTiempo == null) Debug.LogError("textoTiempo NO asignado");
        else Debug.Log(" textoTiempo: " + textoTiempo.name);
        
        if (panelVictoria == null) Debug.LogError("panelVictoria NO asignado");
        else Debug.Log("panelVictoria: " + panelVictoria.name);
        
        if (panelDerrota == null) Debug.LogError("panelDerrota NO asignado");
        else Debug.Log("panelDerrota: " + panelDerrota.name);
    }

    void ConfigurarPausa()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(TogglePause);
            Debug.Log("PauseButton configurado");
        }
        
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(ResumeGame);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(QuitToMenu);
        }

        if (optionsButton != null)
        {
            optionsButton.onClick.RemoveAllListeners();
            optionsButton.onClick.AddListener(OpenOptions);
        }
    }

    // ===== ACTUALIZACIÓN =====
    void Update()
    {
        // Si está pausado, no actualizar
        if (IsPaused) return;
        
        // Actualizar tiempo
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarUI();

            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                PerderNivel("¡Se acabó el tiempo!");
            }
        }
    }

    // ===== UI =====
    public void ActualizarUI()
    {
        if (textoVidas != null)
            textoVidas.text = "Lives: " + vidas;
        
        if (textoFrutas != null)
            textoFrutas.text = "Fruits: " + frutas + " / " + frutasObjetivo;
        
        if (textoTiempo != null)
        {
            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);
            textoTiempo.text = "Time: " + string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

    // ===== SISTEMA DE FRUTAS =====
    public void SumarFruta()
    {
       frutas = frutas + 1;
       ActualizarUI();
    }

    // ===== SISTEMA DE VIDAS =====
    public void PerderVida()
    {
        vidas--;
        ActualizarUI();

        if (vidas <= 0)
        {
            PerderNivel("¡Te quedaste sin vidas!");
        }
    }

    // ===== SISTEMA DE NIVELES =====
    public void GanarNivel()
    {
        Time.timeScale = 0f;
        
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }
    }

    public void PerderNivel(string motivo)
    {
        Time.timeScale = 0f;
        
        if (panelDerrota != null)
        {
            panelDerrota.SetActive(true);
        }
    }

    // ===== NAVEGACIÓN =====
    public void GoToNextLevel()
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        frutas = 0;
        tiempoRestante = 120f;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Asegurar música de juego antes de cargar
            if (audioManager != null)
            {
                audioManager.ForceChangeToGameMusic();
            }
            StartCoroutine(LoadSceneAfterDelay(nextSceneIndex, 0.3f));
        }
        else
        {
            // Ir al menú final con música de menú
            if (audioManager != null)
            {
                audioManager.ForceChangeToMenuMusic();
            }
            StartCoroutine(LoadSceneAfterDelay(0, 0.3f));
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        frutas = 0;
        tiempoRestante = 120f;
        
        // Mantener música de juego al reiniciar nivel
        StartCoroutine(LoadSceneAfterDelay(SceneManager.GetActiveScene().buildIndex, 0.2f));
    }

    // MÉTODO MEJORADO PARA VOLVER AL MENÚ
    public void VolverAlMenu()
    {
        Debug.Log("VolverAlMenu con control de música");
        Time.timeScale = 1f;
        
        // Cambiar a música de menú antes de cargar
        if (audioManager != null)
        {
            audioManager.ForceChangeToMenuMusic();
        }
        
        // Pequeño delay para que empiece la transición musical
        StartCoroutine(LoadSceneAfterDelay(0, 0.5f));
    }

    // MÉTODO MEJORADO PARA COMENZAR JUEGO
    public void StartGame(string levelName = "Level1")
    {
        Debug.Log("StartGame: " + levelName);
        
        // Cambiar a música de juego antes de cargar
        if (audioManager != null)
        {
            audioManager.ForceChangeToGameMusic();
        }
        
        // Pequeño delay para que empiece la transición musical
        StartCoroutine(LoadSceneAfterName(levelName, 0.5f));
    }

    IEnumerator LoadSceneAfterDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator LoadSceneAfterName(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    // ===== PAUSA =====
    public void TogglePause()
    {
        if (pausePanel == null) return;
        
        IsPaused = !pausePanel.activeSelf;
        pausePanel.SetActive(IsPaused);
        
        if (pauseButton != null)
            pauseButton.gameObject.SetActive(!IsPaused);
        
        Time.timeScale = IsPaused ? 0f : 1f;
        
        // Control de audio durante pausa
        if (audioManager != null)
        {
            if (IsPaused)
            {
                audioManager.SetMusicVolume(0.3f); // Bajar volumen en pausa
            }
            else
            {
                audioManager.SetMusicVolume(1f); // Restaurar volumen
            }
        }
    }

    void ResumeGame()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        if (pauseButton != null)
            pauseButton.gameObject.SetActive(true);
        
        Time.timeScale = 1f;
        IsPaused = false;
        
        // Restaurar volumen de música
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(1f);
        }
    }

    // MÉTODO MEJORADO PARA SALIR AL MENÚ DESDE PAUSA
    void QuitToMenu()
    {
        Debug.Log("QuitToMenu desde pausa");
        Time.timeScale = 1f;
        IsPaused = false;
        
        // Cambiar a música de menú
        if (audioManager != null)
        {
            audioManager.ForceChangeToMenuMusic();
        }
        
        StartCoroutine(LoadSceneAfterDelay(0, 0.5f));
    }

    void OpenOptions()
    {
        Debug.Log("Abrir opciones");
        // Aquí puedes añadir lógica para abrir un panel de opciones
    }

    // ===== MÉTODOS PÚBLICOS PARA BOTONES =====
    public void OnStartButtonClicked()
    {
        StartGame("Level1");
    }

    public void OnMainMenuButtonClicked()
    {
        VolverAlMenu();
    }

    public void OnRestartButtonClicked()
    {
        ReiniciarNivel();
    }

    public void OnNextLevelButtonClicked()
    {
        GoToNextLevel();
    }
}