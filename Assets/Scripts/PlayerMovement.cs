using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 dashDirection;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] public SpriteRenderer spriteRenderer;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpDeceleration;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;

    public bool isFacingRight = true;

    private bool isFalling = false;
    private bool canDash = true;
    private bool isDashing = false;
    private bool canDecreaseJumpSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleDashInput();

    }

    void HandleMovement()
    {
        if (isDashing) return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        isFacingRight = !spriteRenderer.flipX;
        if (moveInput != 0) spriteRenderer.flipX = moveInput > 0 ? false : true;

        anim.SetFloat("speed", Mathf.Abs(moveInput));
    }

    void HandleJump()
    {
        if (isDashing) return;

        bool isGrounded = IsTouchingGround();

        isFalling = rb.velocity.y < 0 && !isGrounded;
        anim.SetBool("isFalling", isFalling);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canDecreaseJumpSpeed = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && !isGrounded && !isDashing && canDecreaseJumpSpeed && !isFalling)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpDeceleration);
            canDecreaseJumpSpeed = false;
        }
        anim.SetBool("isJumping", !isGrounded);
    }

    void HandleDashInput()
    {
        if (isDashing) return;

        if (Input.GetButtonDown("Fire3") && canDash)
        {
            dashDirection = GetDashDirection();
            if(dashDirection.y <= 0 && IsWallInDashDirection()) return;
            if (dashDirection != Vector2.zero) StartDash();
        }
    }

    bool IsWallInDashDirection()
    {
        float checkDistance = 1.2f; // Distância mínima para considerar uma parede bloqueando
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dashDirection, checkDistance, groundLayer);

        return hit.collider != null; // Retorna true se houver uma parede na frente
    }

    void StartDash()
    {
        canDash = false;
        isDashing = true;
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        FindObjectOfType<AudioManager>().AudioPlay("PlayerDash");
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0; // Desativa a gravidade temporariamente
        anim.SetTrigger("playerDash");
        rb.velocity = dashDirection * dashSpeed; // Aplica velocidade na direção escolhida

        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = originalGravity; // Restaura a gravidade
        anim.ResetTrigger("playerDash");
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    Vector2 GetDashDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        // Se nenhuma direção for pressionada, o dash segue a direção que o player está virado
        if (direction == Vector2.zero) direction = isFacingRight ? Vector2.right : Vector2.left;

        return direction;
    }

    public bool IsTouchingGround()
    {
        var combinedLayerMask = groundLayer;
        return Physics2D.BoxCast(groundCheck.position, new Vector2(.45f, 0.1f), 0, Vector2.down, 0.1f, combinedLayerMask);
    }
}