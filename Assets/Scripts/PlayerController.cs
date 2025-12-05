using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;

    [Header("Jumping")]
    [SerializeField]
    private float jumpHeight;

    [Header("JumpBuffer")]
    [SerializeField]
    private float jumpBufferTimer = 1f;
    private float jumpBufferCounter;
    private bool wasGroundedLastFrame;


    [Header("Ground Checking")]
    [SerializeField]
    private Vector2 groundCheckOffset;
    [SerializeField]
    private Vector2 groundCheckBoxSize = new Vector2(0.9f,0.1f); 
    [SerializeField]
    private LayerMask groundMask;
    private bool isGrounded;
    

    private Rigidbody2D rb;
    private Vector2 MoveInput { get; set; }



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        InputManager.OnMoveEvent += HandleMovement;
        InputManager.OnJumpEvent += HandleJump;
    }

    private void OnDisable()
    {
        InputManager.OnMoveEvent -= HandleMovement;
        InputManager.OnJumpEvent -= HandleJump;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        UpdateGroundState();

        if (wasGroundedLastFrame == false && isGrounded && jumpBufferCounter > 0)
        { 
            ApplyJumpForce();
            ResetJumpBuffer();
        }

        if(!isGrounded && jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.fixedDeltaTime;
            jumpBufferCounter = Mathf.Max(jumpBufferCounter, 0);
        }

        wasGroundedLastFrame = isGrounded;
    }
    public void HandleMovement(Vector2 input)
    {
        MoveInput = input;
    }

    private void ApplyMovement()
    {
            rb.linearVelocity = new Vector2(MoveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        
        if (isGrounded)
        {
            ApplyJumpForce();
            ResetJumpBuffer();
        }
        else
        {
            jumpBufferCounter = jumpBufferTimer;
        }
           

    }

    
    //============JUMP LOGIC===========================
    //- applying JumpForce. (completed)
    //- ground checking.(completed)
    //-jumpbuffer(completed)
    //-coyote time
    //-mario kind of gravity difference in jump and fall.
    // Unity tells me , give me the jumpforce or velocity so that i apply it to my linearvelocityy(horizontal component).
    // but in the game , we set the jumpHeight=? this is the tweaking value. how much the character should jump. 
    // we need to use some physics calculation to find how much jumpvelocity = ? if we want to jump for example jumpHeight =3. so we need some equation.
    // finding the equation. V^2 = u^2 + 2*a*s.
    // Here V = final velocity, u = initial velocity ,
    // a = aceleration(here it is gravity),
    // s = displacement (here it is jumpheight).
    //  here  u is the intial velcity we need to find.V is the final velocity will be zero at the top after the jump height reaches. 
    // v = 0,u =? , a = -g, s = jumpHeight. 
    // 0 = u^2 + 2 * -g * jumpHeight. 
    // 0 = u^2 - 2* g * jumpHeight
    // u^2 = 2 * g * jumpHeight.
    // u = root of ( 2 * g * JumpHeight.). this is the equation.
    //so after we get u . it is simple just give it to the unity linear velocityy component.
    //===============================================================
    private void ApplyJumpForce()
    {
            float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
            float jumpForce = Mathf.Sqrt(2f * gravity * jumpHeight);
            rb.linearVelocityY = jumpForce; 
    }

    private void UpdateGroundState()
    {
        isGrounded = CheckGrounded();

    }
    private bool CheckGrounded()
    {
        Vector2 checkPosition = (Vector2)transform.position + groundCheckOffset;
        Collider2D collider2D = Physics2D.OverlapBox(checkPosition, groundCheckBoxSize, 0f, groundMask);

        return collider2D != null;

    }

    private void ResetJumpBuffer()
    {
        jumpBufferCounter = 0;
    }

    //For Debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 checkPosition = new Vector3(groundCheckOffset.x, groundCheckOffset.y, 0f) + transform.position;
        Vector3 checkBoxSize = new Vector3(groundCheckBoxSize.x, groundCheckBoxSize.y, 0f);
        Gizmos.DrawWireCube(checkPosition,checkBoxSize);
    }

}
