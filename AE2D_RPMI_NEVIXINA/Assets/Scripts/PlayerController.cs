/*using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerrb;
    private Animator anim;
    private float horizontalInput;

    public float speed;
    public float jumpForce;

    private bool isFacingRight = true;

    [SerializeField] bool isGrounded;
    [SerializeField] GameObject groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int musicToPlay;
    
    public int frutas = 0;
    public int vidas = 3;
    public TextMeshProUGUI textofrutas;
    public TextMeshProUGUI textovidas;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PickUp"))
        {
            AudioManager.Instance.PlaySFX(0);
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

    void Start()
    {
        playerrb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        AudioManager.Instance.PlayMusic(musicToPlay);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.1f, groundLayer);

        Debug.Log("isGrounded = " + isGrounded);
        if (Input.GetKeyDown(KeyCode.Space))
        {
             Debug.Log("ESPACIO PRESIONADO - isGrounded = " + isGrounded);
        }

        Movement();
        Jump();

    }

    void Movement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // Cambiado a GetAxisRaw
        playerrb.linearVelocity = new Vector2(horizontalInput * speed, playerrb.linearVelocity.y); // CORREGIDO: velocity

        //Flip
        if (horizontalInput > 0)
        {
            anim.SetBool("Speed", true);
            if (!isFacingRight)
            {
                Flip();
            }
        }
        else if (horizontalInput < 0)
        {
            anim.SetBool("Speed", true);
            if (isFacingRight)
            {
                Flip();
            }
        }
        else if (horizontalInput == 0)
        {
            anim.SetBool("Speed", false);
        }
    }

    void Jump()
    {
        anim.SetBool("Jump", !isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            AudioManager.Instance.PlaySFX(1);
            playerrb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    
    void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        isFacingRight = !isFacingRight;
    }
}*/



            
   
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float horizontalInput;

    public float speed = 5f;
    public float jumpForce = 12f;
    public int maxJumps = 2;

    private bool isFacingRight = true;
    private int jumpsRemaining;

    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    public GameObject collectedPrefab; // Arrastra el prefab de animación recogida aquí

    public int vidas = 3;
    public TextMeshProUGUI textovidas;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpsRemaining = maxJumps;

        // Actualizar UI al empezar nivel
        if (GameManager.Instance != null)
            GameManager.Instance.ActualizarUI();
    }

    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.3f, groundLayer);
        
        // Resetear saltos si está en el suelo
        if (isGrounded)
        {
            jumpsRemaining = maxJumps;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

        // Animaciones
        anim.SetBool("Speed", Mathf.Abs(horizontalInput) > 0.1f);
        anim.SetBool("Jump", !isGrounded);

        // Flip
        if (horizontalInput > 0 && !isFacingRight) Flip();
        if (horizontalInput < 0 && isFacingRight) Flip();

        // Salto (con doble salto)
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpsRemaining--;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PickUp"))
        {
            // Efecto de recolección
            if (collectedPrefab != null)
            {
                GameObject collectedEffect = Instantiate(collectedPrefab, other.transform.position, Quaternion.identity);
                Destroy(collectedEffect, 1f);
            }

            // Sonido
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(0);

            // Sumar fruta al GameManager
            if (GameManager.Instance != null)
                GameManager.Instance.SumarFruta();

            // Destruir la fruta
            Destroy(other.gameObject);

            Debug.Log("¡Fruta recogida!");
        }

        if (other.CompareTag("PickUpMalo"))
        {
            // Restar vida al GameManager
            if (GameManager.Instance != null)
                GameManager.Instance.PerderVida();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}