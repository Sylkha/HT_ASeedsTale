using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script is contained by the player, father of the model.
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField] float speed_Ground = 7.5f;
    [SerializeField] float jumpSpeed_Ground = 10.0f;
    [SerializeField] float gravity_Ground = 20.0f;
    [SerializeField] float turnSmoothTime_Ground = 0.1f;
    float slopeForce = 0.1f;
    float slopeForceRayLength = 0.2f;
    float fallVelocity = 0;
    float turnSmoothVelocity;
    Vector3 direction;                                  // Movement

    [Header("Glide")]
    [SerializeField] float speed_Glide = 10.0f;
    [SerializeField] float gravity_Glide = 150.0f;
    [SerializeField] float turnSmoothTime_Glide = 0.1f;
    [SerializeField] float heightToFly = 3;

    [Header("Swimming")]
    [SerializeField] float speed_Swim = 10.0f;
    [SerializeField] float min_speed_Swim = 5.0f;
    [SerializeField] float force = 20;
    [SerializeField, Range(0f, 10f)] float waterDrag = 1f;
    [SerializeField, Min(0.1f)] float submergenceOffset = 0.5f;
    [SerializeField] float turnSmoothTime_Swim = 0.1f;
    [SerializeField] float diving = 5f;
    [SerializeField] float radioWaterDetect = 0.6f;
    Vector3 impulse;
    float WaterLevel;

    [Header("Rotation")]    // Min and Max Rotation while swimming
    [SerializeField] float minRotX = -20;   
    [SerializeField] float maxRotX = 20;
    
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
        diving,
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

    #endregion Variables

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        targetRotation = model.localEulerAngles;
    }

    void FixedUpdate()
    {
        if (typeMovement == Terrain.grounded || typeMovement == Terrain.flying) // Separamos grounded de flying para tener un orden
            GroundMovement();

        else if (typeMovement == Terrain.swimming || typeMovement == Terrain.diving) 
            SwimmingMovement();

    }
    float horizontal;
    float vertical;
    void BasicMovement()
    {
         horizontal = Input.GetAxisRaw("Horizontal");
         vertical = Input.GetAxisRaw("Vertical");
        
        if (direction.magnitude >= 0.1f && !is_Jumping)//(characterController.isGrounded || typeMovement == Terrain.swimming || typeMovement == Terrain.diving || glide == true))
        {
            direction = new Vector3(-horizontal, 0f, -vertical).normalized * speed;
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

            is_Jumping = false;
            platformJump = false;
            glide = false;           
        }
        // Not Grounded
        else
        {
            // Glide action             
            if (glide == true)
            {
                typeMovement = Terrain.flying;
                  Debug.Log("glide");
                GlideAttributes();
            }
            // We're not gliding
            else
            {
                typeMovement = Terrain.flying;
                //  Debug.Log("no glide");
                GroundAttributes();
            }          
            
        }
        
        Gravity();
        // Here we have jump so we need it to be called after gravity
        PlayerSkills();
        //Debug.DrawRay(transform.position , transform.TransformDirection(Vector3.down)* heightToFly);
        if (OnSlope())
        {
            characterController.Move(new Vector3(0, slopeForce, 0));
        }

        characterController.Move(direction * Time.deltaTime);
    }

    // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
    // when the direction is multiplied by deltaTime). This is because gravity should be applied
    // as an acceleration (ms^-2)
    void Gravity()
    {
        if (characterController.isGrounded || glide == true)
        {           
            fallVelocity = -gravity * Time.deltaTime;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
        }
        direction.y = fallVelocity;
    }

    bool is_Jumping = false;
    bool OnSlope()
    {
        if(is_Jumping)
            return false;

        if (MyRaycast(slopeForceRayLength))
        {
            slopeForce = -hit.distance;
            return true;
        }

        return false;
    }

    RaycastHit hit;
    bool MyRaycast(float distance)
    {
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, distance);
    }

    bool is_diving = false;
    void PlayerSkills()
    {
        if (characterController.isGrounded && Input.GetButton("JumpGlide") && canMove && is_Jumping == false)
        {        
            Jump(jumpSpeed);           
        }

        if (Input.GetButton("JumpGlide") && (typeMovement == Terrain.grounded || typeMovement == Terrain.flying) && (!MyRaycast(heightToFly)))
        {
            glide = !glide;
        }
        if(typeMovement == Terrain.swimming || typeMovement == Terrain.diving)
        {
            direction.y = 0;
            if (Input.GetMouseButton(0) && typeMovement == Terrain.diving)
            {
                direction.y = diving;
                if (horizontal != 0 || vertical != 0)
                    RotateWithLimits(model, 20, 360, 360 + minRotX);
            }
            if (Input.GetMouseButton(1))
            {
                direction.y = -diving;
                if (horizontal != 0 || vertical != 0)
                    RotateWithLimits(model, 20, 0, maxRotX);
            }

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                is_diving = true;
            else
            {
                is_diving = false;
                RotateWithLimits(model, 1, 0, 0);
            }
        }
    }

    public void Jump(float _jumpSpeed)
    {
        if (characterController.isGrounded)
        {
            fallVelocity = _jumpSpeed;
            direction.y = fallVelocity;
            is_Jumping = true;
        }
    }

    #endregion Ground Movement

    #region Swimming Movement
    float speed_aux = 5;
    bool imp = true;
    void SwimmingMovement()
    {
        WaterAttributes();
        BasicMovement();
        PlayerSkills();

        if (characterController.velocity.magnitude != 0)
            anim.SetInteger("Movement", 6);
        else anim.SetInteger("Movement", 7);

        
        Impulse_swim();
        characterController.Move(impulse * Time.deltaTime);        
    }

    void Impulse_swim()
    {
        if (speed_aux >= speed)
            imp = false;
        else if (speed_aux <= min_speed_Swim)
            imp = true;

        if (imp == false)
            speed_aux -= Time.deltaTime * force * (1 - waterDrag); // si queremos el efecto de drag
        else
            speed_aux += Time.deltaTime * (force * (1 - waterDrag)) / 2;    // impulse

        impulse = direction.normalized * speed_aux;
    }

    private void OnTriggerStay(Collider other)
    {   // Si está en el agua a X distancia sumergido pues a nadar        
        WaterLevel = other.bounds.max.y;
        if (other.tag == "Water")
        {
            if (centerWater.position.y + radioWaterDetect / 2 <= WaterLevel && WaterLevel > centerWater.position.y - radioWaterDetect / 2)
                typeMovement = Terrain.swimming;
            if (centerWater.position.y + radioWaterDetect / 2 + 0.1 < WaterLevel)
                typeMovement = Terrain.diving;
            if (centerWater.position.y + radioWaterDetect / 2 > WaterLevel)
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

       // if (Input.GetAxis("Mouse Y") != 0.0f)
        {
            // x rotation
            float toRotateX = Time.deltaTime * _rotationSpeed;

            targetRotation.x = actualRotation.x + toRotateX;
            targetRotation.x = ClampAngle(targetRotation.x, _minRotation, _maxRotation);

            _objectToRotate.localEulerAngles = targetRotation;
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
        speed = speed_Swim; //* (1 - waterDrag);
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
