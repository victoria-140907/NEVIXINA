using TMPro;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerrb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //textovidas.text = "Vidas:" + Vidas;
        AudioManager.Instance.PlayMusic(musicToPlay);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.1f, groundLayer);
        Movement();
        Jump();
    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        playerrb.linearVelocity = new Vector2(horizontalInput*speed, playerrb.linearVelocity.y);

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
            playerrb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        isFacingRight = !isFacingRight;
    }
}
