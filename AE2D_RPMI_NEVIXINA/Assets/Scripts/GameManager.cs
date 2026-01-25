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
    public float tiempoRestante = 120f; // 2 minutos por nivel

    // Objetivo del nivel actual
    public int frutasObjetivo = 10;

    // UI
    public TextMeshProUGUI textoVidas;
    public TextMeshProUGUI textoFrutas;
    public TextMeshProUGUI textoTiempo;
    public TextMeshProUGUI textoObjetivo;

    // Paneles de UI
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        ActualizarUI();
    }

    void Update()
    {
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

        // Verificar victoria
        if (frutas >= frutasObjetivo)
        {
            GanarNivel();
        }
    }

    public void ActualizarUI()
    {
        if (textoVidas != null)
            textoVidas.text = "Vidas: " + vidas;
        if (textoFrutas != null)
            textoFrutas.text = "Frutas: " + frutas + " / " + frutasObjetivo;
        if (textoTiempo != null)
            textoTiempo.text = "Tiempo: " + Mathf.FloorToInt(tiempoRestante);
        if (textoObjetivo != null)
            textoObjetivo.text = "Objetivo: " + frutasObjetivo + " frutas";
    }

    public void SumarFruta()
    {
        frutas++;
        ActualizarUI();
    }

    public void PerderVida()
    {
        vidas--;
        ActualizarUI();

        if (vidas <= 0)
        {
            PerderNivel("¡Te quedaste sin vidas!");
        }
    }

    void GanarNivel()
    {
        Time.timeScale = 0f; //Pausa el juego
        if (panelVictoria != null) 
            panelVictoria.SetActive(true);
    }

    void PerderNivel(string motivo)
    {
        Time.timeScale = 0f;
        if (panelDerrota != null)
            panelDerrota.SetActive(true);
        Debug.Log(motivo);
    }

    // Método para pasar al siguiente nivel
    public void GoToNextLevel()
    {
        Time.timeScale = 1f; // Reanuda el tiempo
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Resetear frutas para el nuevo nivel, pero mantener vidas
        frutas = 0;
        tiempoRestante = 120f; // Resetear tiempo

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
             SceneManager.LoadScene(nextSceneIndex);
        }

        else
        {
            SceneManager.LoadScene(0); // Volver al menú
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        frutas = 0;
        tiempoRestante = 120f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}