using UnityEngine;

public class SawTrap : MonoBehaviour
{
    public int damage = 1; // Daño que hace

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Restar vida al player
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.vidas -= damage;
                player.textovidas.text = "Vidas:" + player.vidas;
            }

            // Opcional: efecto de sonido/partículas
        }
    }
}