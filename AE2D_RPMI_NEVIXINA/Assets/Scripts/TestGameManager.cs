using UnityEngine;
using TMPro;

public class TestManager : MonoBehaviour
{
    public TextMeshProUGUI testVidasText;
    public TextMeshProUGUI testFrutasText;
    public TextMeshProUGUI testTiempoText;
    
    private int vidas = 3;
    private int frutas = 0;
    private float tiempo = 120f;
    
    void Start()
    {
        Debug.Log("🟢 TEST MANAGER INICIADO");
        
        if (testVidasText == null)
            Debug.LogError("❌ testVidasText es NULL");
        else
            testVidasText.text = "TEST VIDAS: " + vidas;
            
        if (testFrutasText == null)
            Debug.LogError("❌ testFrutasText es NULL");
        else
            testFrutasText.text = "TEST FRUTAS: " + frutas;
            
        if (testTiempoText == null)
            Debug.LogError("❌ testTiempoText es NULL");
        else
            testTiempoText.text = "TEST TIEMPO: " + tiempo;
    }
    
    void Update()
    {
        // Test con teclas
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            vidas--;
            if (testVidasText != null)
                testVidasText.text = "TEST VIDAS: " + vidas;
            Debug.Log("Tecla 1 - Vidas: " + vidas);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            frutas++;
            if (testFrutasText != null)
                testFrutasText.text = "TEST FRUTAS: " + frutas;
            Debug.Log("Tecla 2 - Frutas: " + frutas);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            tiempo -= 10;
            if (testTiempoText != null)
                testTiempoText.text = "TEST TIEMPO: " + tiempo;
            Debug.Log("Tecla 3 - Tiempo: " + tiempo);
        }
    }
}