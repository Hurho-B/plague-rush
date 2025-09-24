using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed; //constant forward speed      
    public float laneChangeSpeed = 10f; //how quickly we move sideway;
    public float maxHorizontalOffset = 3f; // Limit for left/right movement
    private float horizontalInput; // from mouse or keyboard
    private float basePosition;// use to recenter player whe nturnig and using the z axis 
    private bool isWaitingForTurnInput = false;
    private bool allowLeftTurn = false;
    private bool allowRightTurn = false;
    private float speedRot = 360f;                // deg/sec rotation speed
    private float nextBasePosition;
    private Transform platformTransform;

    //know if movement on x or z
    bool xActivated = true;
    bool turning;
    //rotation
    private Quaternion targetRotation;

    [Header("Jump / Gravity")]
    public float jumpForce = 12f;
    public float extraGravity = 20f;         //if not fly forever
    private float extraGravityClone;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;

    private Rigidbody rb;
    Vector3 targetPosition;

    [Header("Slide")]
    private bool sliding;
    private Animator playerAnimation;




    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<Animator>();
        // Start base position whichever axix needed
        basePosition = xActivated ? transform.position.x : transform.position.z;
    }

    void Update()
    {
        //make the horzontal input mouse instead of keyboard
        float mouseX = (Input.mousePosition.x / (float)Screen.width) * 2f - 1f;
        horizontalInput = Mathf.Clamp(mouseX, -1f, 1f);

        // A or D to turn. Can only happen when trigger collider at the choose point 
        if (isWaitingForTurnInput)
        {
            if (allowLeftTurn && Input.GetKeyDown(KeyCode.A))
            {
                StartTurn(-90f); // left
            }
            else if (allowRightTurn && Input.GetKeyDown(KeyCode.D))
            {
                StartTurn(90f); // right
            }
        }
        //check if player jump
        Jump();
        Slide();

    }

    private void FixedUpdate()
    {
        //extra grav 
        GiveExtraGrav();
        //moooovvvement
        Movement();
    }
    private void Movement()
    {
        //check if turning 
        if (turning)
        {
            // rotate smoothly toward target rotation while stopping movement
            Quaternion currentRotation = rb.rotation;
            Quaternion nextRotation = Quaternion.RotateTowards(currentRotation, targetRotation, speedRot * Time.fixedDeltaTime);
            rb.MoveRotation(nextRotation);

            // done rotating, we need to change movement base on the other axis. if x then z vise versa
            if (Quaternion.Angle(currentRotation, targetRotation) < 0.5f)
            {
                rb.MoveRotation(targetRotation);
                turning = false;

                // Toggle axis (we turn 90°, so axis flips)
                xActivated = !xActivated;

                // set the new base position, to center player 
                basePosition = nextBasePosition;

                // Snap the player axis 
                if (xActivated)
                {
                    transform.position = new Vector3(basePosition, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, basePosition);
                }

                // Finish turning
                isWaitingForTurnInput = false;
                allowLeftTurn = allowRightTurn = false;
                horizontalInput = 0f;
            }

            // velocity 0 to turn 
            rb.velocity = Vector3.zero;
            return;
        }
        // Normal movement 
        Vector3 forwardDisp = transform.forward * forwardSpeed * Time.fixedDeltaTime;
        if(!turning)
        {

            // x or z movement
            if (xActivated)
            {
                float unclampedTargetX = transform.position.x + horizontalInput * laneChangeSpeed * Time.fixedDeltaTime;
                float clampedTargetX = Mathf.Clamp(unclampedTargetX, basePosition - maxHorizontalOffset, basePosition + maxHorizontalOffset);
                float newX = Mathf.Lerp(transform.position.x, clampedTargetX, 0.2f);
                Vector3 targetPos = transform.position + forwardDisp;
                targetPos.x = newX;
                rb.MovePosition(targetPos);
            }
            else
            {
                float unclampedTargetZ = transform.position.z + horizontalInput * laneChangeSpeed * Time.fixedDeltaTime;
                float clampedTargetZ = Mathf.Clamp(unclampedTargetZ, basePosition - maxHorizontalOffset, basePosition + maxHorizontalOffset);
                float newZ = Mathf.Lerp(transform.position.z, clampedTargetZ, 0.2f);
                Vector3 targetPos = transform.position + forwardDisp;
                targetPos.z = newZ;
                rb.MovePosition(targetPos);
            }
        }
    }
    private void Jump()
    {
        //Add force 
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            //make sure y 0 for safety 
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (sliding)
            {
                StoppingSlide();
            }
        }
    }
    private void GiveExtraGrav()// make the jumping feel good  --- if not player fly 
    {
        if (!isGrounded())
        {
            rb.AddForce(Vector3.down * extraGravity);
        }
        else
        {
            if(extraGravity != 20)
            {
                extraGravity = 20;
            }
        }
    }
    bool isGrounded()//check touch floor 
    {
         return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Slide()
    {
        if (Input.GetKeyDown(KeyCode.S) && !sliding) 
        {
            if (!isGrounded())
            {
                extraGravityClone = extraGravity;
                extraGravity = extraGravity* 2;
            }
            sliding = true;
            playerAnimation.SetBool("Sliding", true);
            CollideSlide[] slides = FindObjectsOfType<CollideSlide>();
            foreach (var s in slides)
            {
                s.SetColliderActive(false);
            }
            StartCoroutine(SlideStop());
        }
    }

    IEnumerator SlideStop()
    {
        yield return new WaitForSeconds(0.8f);
        StoppingSlide();
    }
    void StoppingSlide()
    {
        sliding = false;
        CollideSlide[] slides = FindObjectsOfType<CollideSlide>();
        foreach (var s in slides)
        {
            s.SetColliderActive(true);
        }
        playerAnimation.SetBool("Sliding", false);
    }
   
    // direction: "L" (left), "R" (right), "B" optional(left or right). Given at the start of new platform 
    public void TurnPlayerTemple(string direction, Transform center)
    {
        isWaitingForTurnInput = true;
        allowLeftTurn = (direction == "L" || direction == "B");
        allowRightTurn = (direction == "R" || direction == "B");
        platformTransform = center;
    }

    // Turning player 
    private void StartTurn(float turnAngle)
    {
        // left or right location 
        Vector3 currentEuler = transform.eulerAngles;
        currentEuler.y += turnAngle;
        targetRotation = Quaternion.Euler(currentEuler);

        // new base position
        if (xActivated)
            nextBasePosition = platformTransform.position.z;
        else
            nextBasePosition = platformTransform.position.x;


        turning = true;

        
        isWaitingForTurnInput = false;
        allowLeftTurn = allowRightTurn = false;
    }


    //swipe movement if change to subway surfer
    /*  private void FixedUpdate()   subway surfer movement
      {
          if (!MovementStopTest)
          {
              Vector3 moveDir = transform.forward; 

              Vector3 velocity = moveDir * forwardSpeed;
              velocity.y = rb.velocity.y; // preserve vertical velocity for gravity/jumping

              // Sideways movement
              if (isMovingSideways)
              {
                  float delta = 0f;
                  if (xActivated)
                      delta = targetOffsets - transform.position.x;
                  else
                      delta = targetOffsets - transform.position.z;

                  float move = Mathf.Clamp(delta, -laneChangeSpeed * Time.fixedDeltaTime, laneChangeSpeed * Time.fixedDeltaTime);

                  if (xActivated)
                      velocity.x = move / Time.fixedDeltaTime;
                  else
                      velocity.z = move / Time.fixedDeltaTime;

                  // Snap to target lane
                  if (Mathf.Abs(delta) < 0.01f)
                  {
                      if (xActivated)
                          transform.position = new Vector3(targetOffsets, transform.position.y, transform.position.z);
                      else
                          transform.position = new Vector3(transform.position.x, transform.position.y, targetOffsets);

                      isMovingSideways = false;
                  }
              }

              rb.velocity = velocity;
          }
          else
          {
              Quaternion currentRotation = rb.rotation;
              Quaternion nextRotation = Quaternion.RotateTowards(currentRotation, targetRotation, speedRot * Time.fixedDeltaTime);
              rb.MoveRotation(nextRotation);

              if (Quaternion.Angle(currentRotation, targetRotation) < 0.1f)
              {
                  rb.MoveRotation(targetRotation);
                  MovementStopTest = false;
              }
              rb.velocity = Vector3.zero;
          }

      }*/
    /*  public void MoveLeft()
      {
          if (currentLane > 0 && !MovementStopTest)
          {
              currentLane--;
              if(xActivated)
              {
                  Debug.Log(transform.position.z);
                 // targetOffsets = transform.position.x;
                  targetOffsets = basePosition + (currentLane - 1) * laneOffest;
              }
              else
              {
                  Debug.Log(transform.position.z);
                 // targetOffsets = transform.position.z;
                  targetOffsets = basePosition + (currentLane - 1) * laneOffest;
              }
              isMovingSideways = true;

          }
      }

    //  public void MoveRight()
      {
          if (currentLane < 2 && !MovementStopTest)
          {
              currentLane++;
              if (xActivated)
              {
                  Debug.Log("moving on the x");
                //  targetOffsets = transform.position.x;
                  targetOffsets = basePosition +(currentLane - 1) * laneOffest;
              }
              else
              {
                  Debug.Log("moving on the z");
                  //targetOffsets = transform.position.z;
                  targetOffsets = basePosition+ (currentLane - 1) * laneOffest;
              }

              isMovingSideways = true;
          }
      }*/
    /*public void TurnPlayer(string direction)
    {
        currentLane = 1;
        Vector3 currentEuler = transform.eulerAngles;
        if (xActivated == true)
        {
            xActivated = false;
            basePosition = transform.position.z;
        }
        else
        {
           
            xActivated = true;
            basePosition = transform.position.x;
        }
        if (direction == "L")
        {
            currentEuler.y -= 90f;
        }
        else if (direction == "R")
        {
            currentEuler.y += 90f;
        }

        targetRotation = Quaternion.Euler(currentEuler);
        MovementStopTest = true;
        
    }*/

}
