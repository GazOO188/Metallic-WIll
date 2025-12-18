using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    
    // THE PLAYER/TARGET THE CAMERA SHOULD FOLLOW
    public Transform target; 
   
    [Header("Camera Follow Settings")]
    public float smoothTime = 0.3f; 
    // HOW SMOOTH THE CAMERA MOVES (HIGHER = MORE LAG, LOWER = SNAPPY)

    public float teleportDelay = 0.08f; 
    // HOW LONG THE CAMERA PAUSES AFTER TELEPORT

    public float maxOffSet = 3f; 
    // IF THE PLAYER GETS THIS FAR FROM THE CAMERA, CAMERA SPEEDS UP

    private Vector3 velocity = Vector3.zero; 
    // REQUIRED BY SMOOTHDAMP TO STORE MOVEMENT SPEED

    private Vector3 offset; 
    // THE POSITION DIFFERENCE BETWEEN CAMERA AND PLAYER WHEN GAME STARTS

    private bool isSuspended = false; 
    // IF TRUE → CAMERA STOPS FOLLOWING TEMPORARILY


    void Start()
    {
        if (target != null) 
            // MAKE SURE THE PLAYER EXISTS BEFORE ACCESSING THEM

            //THIS LINE GETS THE DIRECTION BETWEEN PLAYER AND CAMERA OR SAVES THE DISTANCE BETWEEN THEM//
            offset = transform.position - target.position;
            // SAVE THE INITIAL DISTANCE BETWEEN CAMERA AND PLAYER
    }


    void LateUpdate()
    {
        if (target == null) return;  
    // IF THERE IS NO TARGET TO FOLLOW, STOP HERE TO AVOID ERRORS//

    // ONLY DO THE OLLOWING IF IT IS NOT CURRENTLY PAUSED/FROZEN//
    if (!isSuspended)  
        
    {

        //THIS LINE IS SAYING THE TARGETPOS IS THE PLAYER POSITION + OFFSET
        Vector3 targetPos = target.position + offset;
        // CALCULATE WHERE THE CAMERA SHOULD MOVE TO (PLAYER POSITION + OFFSET)

        // MAKE SURE CAMERA KEEPS SAME Z-AXIS POSITION (NO ZOOM OR DEPTH CHANGE)
        targetPos.z = transform.position.z;
        
       
        // START WITH THE DEFAULT SMOOTHNESS (HOW SLOW/SOFT THE CAMERA MOVES)
        float dynamicSmooth = smoothTime;
   
        // CHECK HOW FAR AWAY THE CAMERA IS FROM WHERE IT SHOULD BE
        float distance = Vector3.Distance(transform.position, targetPos);
    

        if (distance > maxOffSet)
        {
            dynamicSmooth = smoothTime / 2f; 
            // IF CAMERA IS TOO FAR FROM TARGET, MAKE IT MOVE FASTER TO CATCH UP
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,  // CURRENT CAMERA POSITION
            targetPos,           // TARGET POSITION CAMERA WANTS TO REACH
            ref velocity,        // INTERNAL VARIABLE TO TRACK SPEED (REQUIRED BY SMOOTHDAMP)
            dynamicSmooth        // HOW SLOWLY THE CAMERA MOVES (SMOOTHING TIME)
        );
        // MOVE THE CAMERA SMOOTHLY TOWARDS THE TARGET POSITION
    }
}
    


    public void SuspendCamera(float duration)
    {
        if (!isSuspended) 
            // ONLY START SUSPENDING IF NOT ALREADY SUSPENDED
            StartCoroutine(SuspendCoroutine(duration));
            // START THE FREEZE TIMER
    }


    //THIS COUROUTINE STOPS THE CAMERA FROM FOLLOWING THE PLAYER//

    private System.Collections.IEnumerator SuspendCoroutine(float duration)
    {
        isSuspended = true;  
        // STOP CAMERA FROM FOLLOWING PLAYER

        yield return new WaitForSeconds(duration); 
        // WAIT FOR X SECONDS (FREEZE TIME)

        isSuspended = false; 
        // TURN CAMERA FOLLOW BACK ON
    }
}