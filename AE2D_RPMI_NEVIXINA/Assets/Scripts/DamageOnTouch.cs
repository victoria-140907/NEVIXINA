using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public int damage = 1;
    public string targetTag = "Player";
    public bool destroyOnTouch = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("Pincho hace " + damage + " de daño");
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.vidas -= damage;
                GameManager.Instance.ActualizarUI();
                
                if (GameManager.Instance.vidas <= 0)
                {
                    GameManager.Instance.PerderNivel("Game Over");
                }
            }
            
            if (destroyOnTouch)
                Destroy(gameObject);
        }
    }
}