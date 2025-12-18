using UnityEngine;

public class TeleportCusor : MonoBehaviour
{
    [Header("References")]
    public Transform player;           // Reference to the player's transform
    public Camera mainCamera;          // Reference to the main camera
    public SpriteRenderer cursorSprite; // SpriteRenderer for cursor
    public float followSmoothness = 10f; // How quickly cursor follows target position

    [Header("Settings")]
    public float teleportRadius = 5f;   // Match this to PlayerTele.PlayerRadius

    private Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       if (mainCamera == null)
            mainCamera = Camera.main;

        Cursor.visible = false; // Hide system cursor 
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        // Get mouse position in world space

        //CONVERTS MOUSE POSITION TO WORLD COORDINATES, MOUSE COORDINATES IS NORMALLY IN SCREEN COORDINATES//
        //THE GAME WORLD EXISTS IN WORLD SPACE
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        //SET Z TO 0 BECAUSE THE GAME IS IN 2D
        mouseWorld.z = 0f;

        // THE OFFSET VARIABLE FINDS THE DIRECTION FROM THE PLAYER TO THE MOUSE
        //SUBTRACTION GIVES A DIRECTION VECTOR//
        Vector3 offset = mouseWorld - player.position;

        //DISTANCE IS HOW FAR THE MOUSE IS FROM THE PLAYER//

        //MAGNITUDE = THE LENGTH OF THE PLAYER, EX: 5 MEANS THE MOUSE IS 5 UNITS AWAY FROM THE MOUSE//
        float distance = offset.magnitude;

        // IF THE MOUSE IS TOO FAR, PUSH IT BACK TO THE EDGE OF THE CIRCLE//
        // CURSOR STAYS INSIDE THE CIRCLE//
         if (distance > teleportRadius)
        {
            //IF YOU LOWER THE TELEPORTRADIUS VARIABLE, THE CURSOR WON'T MOVE FAR//

            //OFFSET.NORMALIZED GIVES A DIRECTION VECTOR OF LENGTH 1
            offset = offset.normalized * teleportRadius;
        }

        // Calculate target position
        targetPosition = player.position + offset;

        // MAKES CURSOR SMOOTHLY FOLLOW TARGET//
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSmoothness * Time.deltaTime);
    }
}
