using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{


    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpHeight;

    private Rigidbody2D rb;
    private Vector2 MoveInput { get; set; }



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        InputManager.OnMoveEvent += HandleMovement;
        InputManager.OnJumpEvent += Jump;
    }

    private void OnDisable()
    {
        InputManager.OnMoveEvent -= HandleMovement;
        InputManager.OnJumpEvent -= Jump;
    }

    private void FixedUpdate()
    {
        Move();
    }
    public void HandleMovement(Vector2 input)
    {
        MoveInput = input;
    }

    public void Move()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(MoveInput.x * moveSpeed, rb.linearVelocity.y);
        }

    }
    //============JUMP LOGIC===========================
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
    public void Jump()
    {
        if (rb != null)
        {
            float gravity = Mathf.Abs(Physics2D.gravity.y);
            float jumpForce = Mathf.Sqrt(2f * gravity * jumpHeight);
            rb.linearVelocityY = jumpForce;
        }
    }

    bool OnGrounded()
    {
        //To Do:
        return true;
    }

}
