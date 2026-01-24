using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
       // Si colisiona con el jugador y existe el GameManager
       if (other.CompareTag("Player") && GameManager.Instance != null)
       {
           GameManager.Instance.GoToNextLevel();
       }
    }
}
