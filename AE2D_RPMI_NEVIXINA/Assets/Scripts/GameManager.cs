using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

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

    // ===== INICIALIZACIÓN =====
    private void Awake()
    {
        Debug.Log("🎮 GameManager Awake llamado");
        
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("✅ GameManager creado como Singleton");
        }
        else
        {
            Debug.Log("⚠️ GameManager duplicado detectado. Destruyendo: " + gameObject.name);
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        Debug.Log("🎮 GameManager Start iniciado");
        
        // Verificar asignaciones
        VerificarAsignaciones();
        
        // Configurar UI inicial
        ActualizarUI();
        
        // Configurar botones de pausa
        ConfigurarPausa();
    }

    void VerificarAsignaciones()
    {
        Debug.Log("=== VERIFICANDO ASIGNACIONES ===");
        
        if (textoVidas == null) Debug.LogError("❌ textoVidas NO asignado");
        else Debug.Log("✅ textoVidas: " + textoVidas.name);
        
        if (textoFrutas == null) Debug.LogError("❌ textoFrutas NO asignado");
        else Debug.Log("✅ textoFrutas: " + textoFrutas.name);
        
        if (textoTiempo == null) Debug.LogError("❌ textoTiempo NO asignado");
        else Debug.Log("✅ textoTiempo: " + textoTiempo.name);
        
        if (panelVictoria == null) Debug.LogError("❌ panelVictoria NO asignado");
        else Debug.Log("✅ panelVictoria: " + panelVictoria.name);
        
        if (panelDerrota == null) Debug.LogError("❌ panelDerrota NO asignado");
        else Debug.Log("✅ panelDerrota: " + panelDerrota.name);
    }

    void ConfigurarPausa()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(TogglePause);
            Debug.Log("✅ PauseButton configurado");
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
        // Debug temporal
        Debug.Log("🔄 ActualizarUI - Vidas: " + vidas + ", Frutas: " + frutas + ", Tiempo: " + tiempoRestante);
        
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
        Debug.Log("💔 PerderVida llamado. Antes: " + vidas);
        vidas--;
        ActualizarUI();

        if (vidas <= 0)
        {
            PerderNivel("¡Te quedaste sin vidas!");
        }
        else
        {
            Debug.Log("💔 Después: " + vidas);
        }
    }

    // ===== SISTEMA DE NIVELES =====
    public void GanarNivel()
    {
        Debug.Log("🏆 GanarNivel llamado!");
        Time.timeScale = 0f;
        
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
            Debug.Log("✅ Panel Victoria activado");
        }
        else
        {
            Debug.LogError("❌ panelVictoria es null!");
        }
    }

    public void PerderNivel(string motivo)
    {
        Debug.Log("💀 PerderNivel: " + motivo);
        Time.timeScale = 0f;
        
        if (panelDerrota != null)
        {
            panelDerrota.SetActive(true);
            Debug.Log("✅ Panel Derrota activado");
        }
        else
        {
            Debug.LogError("❌ panelDerrota es null!");
        }
    }

    // ===== NAVEGACIÓN =====
    public void GoToNextLevel()
    {
        Debug.Log("➡️ GoToNextLevel");
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        frutas = 0;
        tiempoRestante = 120f;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);
        else
            SceneManager.LoadScene(0);
    }

    public void ReiniciarNivel()
    {
        Debug.Log("🔄 ReiniciarNivel");
        Time.timeScale = 1f;
        frutas = 0;
        tiempoRestante = 120f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VolverAlMenu()
    {
        Debug.Log("🏠 VolverAlMenu");
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // ===== PAUSA =====
    public void TogglePause()
    {
        Debug.Log("⏸️ TogglePause llamado");
        
        if (pausePanel == null)
        {
            Debug.LogError("❌ pausePanel es null!");
            return;
        }
        
        IsPaused = !pausePanel.activeSelf;
        pausePanel.SetActive(IsPaused);
        
        if (pauseButton != null)
            pauseButton.gameObject.SetActive(!IsPaused);
        
        Time.timeScale = IsPaused ? 0f : 1f;
        Debug.Log("⏸️ Pausa: " + IsPaused + ", TimeScale: " + Time.timeScale);
    }

    void ResumeGame()
    {
        Debug.Log("▶️ ResumeGame");
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        if (pauseButton != null)
            pauseButton.gameObject.SetActive(true);
        
        Time.timeScale = 1f;
        IsPaused = false;
    }

    void QuitToMenu()
    {
        Debug.Log("🚪 QuitToMenu");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}