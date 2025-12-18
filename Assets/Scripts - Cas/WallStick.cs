using UnityEngine;

public class WallStick : MonoBehaviour
{

    
    [Header("Wall Stick Settings")]

    public float wallStickGravityScale = 0f; // GRAVITY SCALE WHEN PLAYER IS STICKING TO THE WALL (0 MEANS NO FALLING)

    public float normalGravityScale = 5f;    // NORMAL GRAVITY WHEN PLAYER IS NOT ON THE WALL

    public float wallJumpForce = 10f;        // FORCE APPLIED WHEN JUMPING OFF A WALL



    [Header("Ground Check")]

    public Transform groundCheck;            // TRANSFORM USED TO CHECK IF THE PLAYER IS STANDING ON THE GROUND

    public LayerMask groundLayer;            // LAYER MASK TO DETECT WHAT COUNTS AS GROUND



    private Rigidbody2D rb;                  // REFERENCE TO THE PLAYER'S RIGIDBODY2D COMPONENT

    private bool isWallSticking;             // TRACKS IF THE PLAYER IS CURRENTLY STICKING TO A WALL

    private bool isGrounded;                 // TRACKS IF THE PLAYER IS TOUCHING THE GROUND



    void Start()
    {

        // GET THE RIGIDBODY2D COMPONENT AT THE START OF THE GAME

        rb = GetComponent<Rigidbody2D>();



        // SET THE DEFAULT GRAVITY TO NORMAL

        //NORMAL GRAVITY WHEN NOT ON WALL//

        rb.gravityScale = normalGravityScale;

    }



    void Update()
    {

        // CHECK IF THE PLAYER IS ON THE GROUND USING A SMALL CIRCLE AT THE GROUNDCHECK POSITION
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);



        // IF THE PLAYER IS ON THE WALL AND PRESSES THE JUMP BUTTON, PERFORM A WALL JUMP

        if (isWallSticking && Input.GetButtonDown("Jump"))
        {

            // RESTORE NORMAL GRAVITY AFTER JUMPING OFF THE WALL

            rb.gravityScale = normalGravityScale;



            // APPLY A FORCE AWAY FROM THE WALL AND UPWARD

            // MATHF.SIGN(TRANSFORM.LOCALSCALE.X) CHECKS THE DIRECTION THE PLAYER IS FACING

            rb.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpForce, wallJumpForce);



            // THE PLAYER IS NO LONGER STICKING TO THE WALL AFTER JUMPING

            isWallSticking = false;

        }

    }



    void FixedUpdate()
    {

        // THIS RUNS EVERY PHYSICS UPDATE

        // IF THE PLAYER IS STICKING TO A WALL, STOP THEIR VERTICAL VELOCITY SO THEY DON'T SLIDE DOWN

        if (isWallSticking)
        {

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        }

    }



    void OnCollisionEnter2D(Collision2D collision)
    {

        // WHEN THE PLAYER FIRST COLLIDES WITH AN OBJECT

        // CHECK IF IT’S TAGGED AS "WALL" AND THE PLAYER ISN’T GROUNDED

        if (collision.gameObject.CompareTag("Wall") && !isGrounded)
        {

            // PLAYER STARTS STICKING TO THE WALL

            isWallSticking = true;



            // DISABLE GRAVITY WHILE STICKING SO THEY DON’T FALL

            rb.gravityScale = wallStickGravityScale;



            // STOP ALL MOTION TO MAKE SURE THE PLAYER STICKS INSTANTLY

            rb.linearVelocity = Vector2.zero;

        }

    }



    void OnCollisionExit2D(Collision2D collision)
    {

        // WHEN THE PLAYER STOPS TOUCHING THE WALL

        if (collision.gameObject.CompareTag("Wall"))
        {

            // THE PLAYER IS NO LONGER STICKING

            isWallSticking = false;



            // RESTORE NORMAL GRAVITY SO THE PLAYER CAN FALL AGAIN

            rb.gravityScale = normalGravityScale;

        }

    }

}