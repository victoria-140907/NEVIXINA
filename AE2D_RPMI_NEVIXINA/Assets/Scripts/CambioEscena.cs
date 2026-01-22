using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    public void Cambioescena()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Salida()
    {
        Debug.Log("Estas saliendo del juego");
        Application.Quit();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void cambioescena()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Pruebica");
        }
    }
}
