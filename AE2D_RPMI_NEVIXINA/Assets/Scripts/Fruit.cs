using UnityEngine;

public class Fruit : MonoBehaviour
{
    private bool yaRecolectada = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Si ya fue recolectada, ignorar
        if (yaRecolectada) return;
        
        // SOLO si es el jugador (por nombre, no por tag)
        if (other.gameObject.name.Contains("Player") || 
            other.gameObject.name.Contains("Jugador") ||
            other.gameObject.name.Contains("Character"))
        {
            yaRecolectada = true;
            
            // SUMAR 1 PUNTO DIRECTAMENTE
            GameManager.Instance.frutas += 1;
            GameManager.Instance.ActualizarUI();
            
            Debug.Log($" +1 Fruta. Total: {GameManager.Instance.frutas}");
            
            // Hacer invisible y destruir después
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
}