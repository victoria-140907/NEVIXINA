using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isFinalCheckpoint = false; // Marca si es el checkpoint final

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isFinalCheckpoint)
            {
                // Verificar si el jugador tiene todas las frutas
                if (GameManager.Instance.frutas >= GameManager.Instance.frutasObjetivo)
                {
                    GameManager.Instance.GanarNivel();
                }
                else
                {
                    Debug.Log("¡Necesitas más frutas para pasar!");
                    // Aquí podrías mostrar un mensaje en pantalla
                }
            }
        }
    }
}