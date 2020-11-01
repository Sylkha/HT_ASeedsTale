using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script is contained by the player, father of the model.
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed_Ground = 7.5f;
    [SerializeField] float jumpSpeed_Ground = 10.0f;
    [SerializeField] float gravity_Ground = 20.0f;
    [SerializeField] float turnSmoothTime_Ground = 0.1f;
    float fallVelocity = 0;
    float turnSmoothVelocity;
    Vector3 direction;                                  // Movement

    [Header("Glide")]
    [SerializeField] float speed_Glide = 10.0f;
    [SerializeField] float gravity_Glide = 150.0f;
    [SerializeField] float turnSmoothTime_Glide = 0.1f;

    [Header("Swimming")]
    [SerializeField] float speed_Swim = 10.0f;
    [SerializeField, Range(0f, 10f)] float waterDrag = 1f;
    [SerializeField, Min(0.1f)] float submergenceOffset = 0.5f;
    [SerializeField] float turnSmoothTime_Swim = 0.1f;
    float diving = 0.5f;

    [Header("Rotation")]    // Min and Max Rotation while swimming
    [SerializeField] float minRotX = -60;   
    [SerializeField] float maxRotX = 60;
    
    [Header("References")]
    [SerializeField] Transform model;               // Reference to the model of the character (son)
    [SerializeField] Animator anim;
    [SerializeField] Transform centerWater;         // Reference to the point of the Character 


    float speed;
    float jumpSpeed;
    float gravity;
    float turnSmoothTime;

    public enum Terrain
    {
        grounded,
        swimming,
        flying
    }
    [Header("Terrain Movement")]
    public Terrain typeMovement;


    bool glide = false;
    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public bool platformJump = false;
    
    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        targetRotation = model.localEulerAngles;
    }

    void FixedUpdate()
    {
        if (typeMovement == Terrain.grounded || typeMovement == Terrain.flying) // Separamos grounded de flying para tener un orden
            GroundMovement();

        else if (typeMovement == Terrain.swimming) 
            SwimmingMovement();

    }
    
    void BasicMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(-horizontal, 0f, -vertical).normalized * speed;
        if (direction.magnitude >= 0.1f && (characterController.isGrounded || typeMovement == Terrain.swimming || glide == true))
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }      
    }

    #region Ground Movement
    void GroundMovement()
    {
        BasicMovement();

        // Grounded
        if (characterController.isGrounded)
        {
            typeMovement = Terrain.grounded;
            GroundAttributes();
            if (characterController.velocity.magnitude != 0)
                anim.SetInteger("Movement", 1);
            else anim.SetInteger("Movement", 0);

            platformJump = false;
            glide = false;           
        }
        // Not Grounded
        else
        {
            typeMovement = Terrain.flying;
            // Glide action 
            if (glide == true)
            {
              //  Debug.Log("glide");
                GlideAttributes();
            }
            // We're not gliding
            else
            {
              //  Debug.Log("no glide");
                GroundAttributes();
            }          
        }

        Gravity();
        // Here we have jump so we need it to be called after gravity
        PlayerSkills();

        characterController.Move(direction * Time.deltaTime);
    }

    // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
    // when the direction is multiplied by deltaTime). This is because gravity should be applied
    // as an acceleration (ms^-2)
    void Gravity()
    {
        if (characterController.isGrounded)
        {           
            fallVelocity = -gravity * Time.deltaTime;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
        }
        direction.y = fallVelocity;
    }

    void PlayerSkills()
    {
        if (typeMovement == Terrain.grounded && Input.GetButton("JumpGlide") && canMove && platformJump == false)
        {        
            Jump(jumpSpeed);           
        }
        if (typeMovement == Terrain.flying && Input.GetButton("JumpGlide"))
        {
            glide = !glide;
        }
        if(typeMovement == Terrain.swimming)
        {
            direction.y = 0;
            if (Input.GetMouseButton(0))
            {
                direction.y = diving;
            }
            if (Input.GetMouseButton(1))
            {
                direction.y = -diving;
            }
        }
    }

    public void Jump(float _jumpSpeed)
    {
        if (characterController.isGrounded)
        {
            fallVelocity = _jumpSpeed;
            direction.y = fallVelocity;
            // We turn it true so at the moment we press for Jump, it turns false.
            glide = true;
        }
    }

    #endregion Ground Movement

    #region Swimming Movement

    void SwimmingMovement()
    {
        WaterAttributes();
        BasicMovement();
        PlayerSkills();

        if (characterController.velocity.magnitude != 0)
            anim.SetInteger("Movement", 6);
        else anim.SetInteger("Movement", 7);

        characterController.Move(direction * Time.deltaTime);
    }

    void EvaluateSubmergence()
    {/*
        if (Physics.Raycast(
            model.position + Vector3.up * submergenceOffset,
            -Vector3.up, out RaycastHit hit, submergenceRange,
            waterMask, QueryTriggerInteraction.Collide

        ))
        {
            submergence = 1f - hit.distance / submergenceRange;
        }*/
    }
    private void OnTriggerStay(Collider other)
    {   // Si está en el agua a X distancia sumergido pues a nadar        
        RaycastHit hit;
        if (other.tag == "Water" && Physics.Raycast(centerWater.position, Vector3.down, out hit, submergenceOffset))
        {
            typeMovement = Terrain.swimming;
        }
        else if(other.tag == "Water" && !Physics.Raycast(centerWater.position, Vector3.down, out hit, submergenceOffset))
        {
            typeMovement = Terrain.grounded;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            typeMovement = Terrain.grounded;
        }
    }

    private Vector3 targetRotation;
    void RotateWithLimits(Transform _objectToRotate, float _rotationSpeed, float _minRotation, float _maxRotation)
    {
        Vector3 actualRotation = _objectToRotate.localEulerAngles;
        //Debug.Log(actualRotation.x);

       // if (Input.GetAxis("Mouse Y") != 0.0f)
        {
            // x rotation
            float toRotateX = Input.GetAxis("Mouse Y") * /*Time.deltaTime * */ - _rotationSpeed;

            targetRotation.x = actualRotation.x + toRotateX;
            targetRotation.x = ClampAngle(targetRotation.x, _minRotation, _maxRotation);

            model.localEulerAngles = targetRotation;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < 0.0f)
            angle = 360.0f + angle;
        if (angle > 180.0f)
            return Mathf.Max(angle, 360.0f + min);
        return Mathf.Min(angle, max);
    }

    #endregion Swimming Movement

    #region Attributes
    void GroundAttributes()
    {
        speed = speed_Ground;
        jumpSpeed = jumpSpeed_Ground;
        gravity = gravity_Ground;
        turnSmoothTime = turnSmoothTime_Ground;
    }
    void GlideAttributes()
    {
        speed = speed_Glide;
        jumpSpeed = 0;
        gravity = gravity_Glide;
        turnSmoothTime = turnSmoothTime_Glide;
    }
    void WaterAttributes()
    {
        speed = speed_Swim * (1 - waterDrag);
        jumpSpeed = 0;
        gravity = 0;
        turnSmoothTime = turnSmoothTime_Swim;
    }
    #endregion Attributes

    #region Dialogue Commands

    [Yarn.Unity.YarnCommand("canMove")]
    public void Set_CanMove(string _canMove)
    {
        switch (_canMove)
        {
            case "true":
                canMove = true;
                break;
            case "false":
                canMove = false;
                break;
            default:
                Debug.Log("No está bien asignado el valor, comprueba que está bien escrito.");
                break;
        }
    }
    //Movimiento camara

    #endregion Dialogue Commands
}
