using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerrb;
    private Animator anim;
    private float horizontalInput;

    public float speed;
    public float jumpForce;
    
    
    public int frutas = 0;
    public int vidas = 3;
    public TextMeshProUGUI textofrutas;
    public TextMeshProUGUI textovidas;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other. CompareTag("PickUp"))
        {
            frutas++;
            textofrutas.text = "Frutas: " + frutas;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("PickUpMalo"))
        {
            vidas--;
            textovidas.text = "Vidas:" + vidas;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerrb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        playerrb.linearVelocity = new Vector2(horizontalInput*speed, playerrb.linearVelocity.y);

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerrb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);

        }


    }

}
