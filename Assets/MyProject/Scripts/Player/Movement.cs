using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bindings;

//This script is contained by the player, father of the model.
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    #region Variables
    MyPlayerActions actions;
    string saveActionsData;

    [Header("Movement")]
    [SerializeField] float speed_Ground = 7.5f;
    [SerializeField] float jumpSpeed_Ground = 10.0f;
    [SerializeField] float gravity_Ground = 20.0f;
    [SerializeField] float turnSmoothTime_Ground = 0.1f;
    float slopeForce;
    [SerializeField] float slopeForceRayLength = 0.5f;
    float fallVelocity = 0;
    float turnSmoothVelocity;
    Vector3 direction;                                  // Movement direction

    // For going down on slopes
    public bool isOnSlope = false;
    Vector3 hitNormal;
    [SerializeField] float slideVelocity = 5;
    [SerializeField] float slopeForceDown = 10;

    [Header("Glide")]
    [SerializeField] float speed_Glide = 10.0f;
    [SerializeField] float gravity_Glide = 150.0f;
    [SerializeField] float turnSmoothTime_Glide = 0.1f; // Rotate smooth while gliding
    [SerializeField] float heightToFly = 3;             // Height we need for the player from the floor to can glide
    // [SerializeField] float glideChangeCD = 0.5f;     // Some Cooldown
    [SerializeField] float _minimumHeldDuration = 0.5f; // How much time we need to held buttom in order to get the player gliding
    // bool canChangeGlide = false;
       
    private float _spacePressedTime = 0;
    private bool _spaceHeld = false;
    private bool _getbuttondown_glide = false;
    private bool _getbutton_glide = false;
    private bool _getbuttonup_glide = false;

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
    bool is_diving = false;

    [Header("Rotation")]    // Min and Max Rotation while swimming
    [SerializeField] float minRotX = -20;   
    [SerializeField] float maxRotX = 20;
    
    [Header("References")]
    [SerializeField] Transform model;               // Reference to the model of the character (son)
    [SerializeField] Animator anim;
    [SerializeField] Transform centerWater;         // Reference to the point of the Character 
    [SerializeField] Transform camera_transf;


    float speed;
    float jumpSpeed;
    float gravity;
    float turnSmoothTime;

    // Axis container vector (for the movement)
    Vector3 dir;

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

    #region InControl
    void OnEnable()
    {
        // See PlayerActions.cs for this setup.
        actions = MyPlayerActions.CreateWithDefaultBindings();
        //playerActions.Move.OnLastInputTypeChanged += ( lastInputType ) => Debug.Log( lastInputType );

        LoadBindings();
    }
    void OnDisable()
    {
        // This properly disposes of the action set and unsubscribes it from
        // update events so that it doesn't do additional processing unnecessarily.
        actions.Destroy();
    }
    void LoadBindings()
    {
        if (PlayerPrefs.HasKey("Bindings"))
        {
            saveActionsData = PlayerPrefs.GetString("Bindings");
            actions.Load(saveActionsData);
        }
    }
    void SaveBindings()
    {
        saveActionsData = actions.Save();
        PlayerPrefs.SetString("Bindings", saveActionsData);
    }
    #endregion InControl

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
    
    void BasicMovement()
    {
        dir.x = actions.Move.X;
        dir.z = actions.Move.Y;
        Vector3 camDirection = camera_transf.rotation * dir;

        // We can rotate if we're not flying (jump or glide), or if we're gliding
        if(glide == true || typeMovement != Terrain.flying)
            direction = new Vector3(camDirection.x, 0f, camDirection.z).normalized * speed;
        if (direction.magnitude >= 0.1f && (glide == true || typeMovement != Terrain.flying))
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
        if (IsGrounded())
        {
            typeMovement = Terrain.grounded;
            GroundAttributes();
            if (characterController.velocity.magnitude != 0) anim.SetInteger("Movement", 1);
            else anim.SetInteger("Movement", 0);

            is_Jumping = false;
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
                Debug.Log("glide");
                GlideAttributes();
            }
            // We're not gliding
            else
            {
                GroundAttributes();
            }          
            
        }
        
        Gravity();
        // Here we have jump so we need it to be called after gravity
        PlayerSkills();
        //Debug.DrawRay(transform.position , transform.TransformDirection(Vector3.down)* heightToFly);

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

        SlideDown();
    }

    // For going down on slopes
    void SlideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= characterController.slopeLimit;

        if (isOnSlope == true)
        {
            direction.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            direction.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;

            direction.y -= slopeForceDown;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    bool is_Jumping = false;
    bool IsGrounded()
    {
        if (characterController.isGrounded)
            return true;

        if(is_Jumping)
            return false;

        // Here we detect the slopes in the terrain so that the character was on flat terrain. 
        // We make it close to the ground if it is between the ground and a short vertical distance 
        if (MyRaycast(slopeForceRayLength))
        {
            slopeForce = -hit.distance;
            characterController.Move(new Vector3(0, slopeForce, 0));
            return true;
        }

        return false;
    }

    RaycastHit hit;
    bool MyRaycast(float distance)
    {
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, distance);
    }

    IEnumerator Cooldown(float time_cd, bool can)
    {
        can = false;
        yield return new WaitForSeconds(time_cd);
        can = true;
    }
    
    void Update() // We need inputs to be in Update instead of FixedUpdate
    {
        if (actions.JumpGlide.WasPressed && typeMovement == Terrain.flying && (!MyRaycast(heightToFly)))
        {
            _getbuttondown_glide = true;
        }
        if (actions.JumpGlide.IsPressed && typeMovement == Terrain.flying && (!MyRaycast(heightToFly)))
        {
            _getbutton_glide = true;
        }
        if (actions.JumpGlide.WasReleased)
        {
            _getbuttonup_glide = true;
        }
    }
    void PlayerSkills()
    {
        if (IsGrounded() && actions.JumpGlide.IsPressed && canMove && is_Jumping == false)
        {        
            Jump(jumpSpeed);            
        }

        if (_getbuttondown_glide)
        {
            // Use has pressed the Space key. We don't know if they'll release or hold it, so keep track of when they started holding it.
            _spacePressedTime = 0;
            _spaceHeld = false;
            _getbuttondown_glide = false;
        }
        if (_getbutton_glide)
        {
            _spacePressedTime += Time.deltaTime;
            if (_spacePressedTime > _minimumHeldDuration)
            {
                // Player has held the Space key for .25 seconds. Consider it "held"
                glide = true;
                _spaceHeld = true;
                Debug.Log("lesgo");
            }
            _getbutton_glide = false;
        }
        if (_getbuttonup_glide)
        {
            _spacePressedTime = 0;
            glide = false;
            _spaceHeld = false;
            _getbuttonup_glide = false;
        }
        if (typeMovement == Terrain.swimming || typeMovement == Terrain.diving)
        {
            direction.y = 0;
            if (actions.DiveUp && typeMovement == Terrain.diving)
            {
                direction.y = diving;
                if (dir.x != 0 || dir.z != 0)
                    RotateWithLimits(model, 20, 360, 360 + minRotX);
            }
            if (actions.DiveDown)
            {
                direction.y = -diving;
                if (dir.x != 0 || dir.z != 0)
                    RotateWithLimits(model, 20, 0, maxRotX);
            }

            if (actions.DiveUp || actions.DiveDown)
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
        if (IsGrounded())
        {
            fallVelocity = _jumpSpeed;
            direction.y = fallVelocity;
            is_Jumping = true;
            glide = false;
           // StartCoroutine(Cooldown(glideChangeCD, canChangeGlide));
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
