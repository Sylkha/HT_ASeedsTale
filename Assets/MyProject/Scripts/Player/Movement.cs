﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cámara arriba y abajo? Script sujeto a cambios.

//This script is contained by the player, father of the model.
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 7.5f;
    [SerializeField] float jumpSpeed = 10.0f;
    [SerializeField] float gravity = 20.0f;
    [SerializeField] float rotationSpeed = 1;
    
    [Header("Glide")]
    [SerializeField] float speedGlide = 10.0f;
    [SerializeField] float gravityGlide = 150.0f;

    [Header("Camera")]
    [SerializeField] float cameraDelay = 10f;

    [Header("Rotation")]    // Min and Max Rotation while swimming
    [SerializeField] float minRotX = -60;   
    [SerializeField] float maxRotX = 60;
    
    [Header("References")]
    [SerializeField] Transform playerCameraParent;  // Pivot of the camera (son)
    [SerializeField] Transform model;               // Reference to the model of the character (son)
    [SerializeField] Transform waterPlane;          // Reference to the water plane
    [SerializeField] Transform centerWater;         // Reference to the point of the Character 



    public enum Terrain
    {
        grounded,
        swimming,
        flying
    }
    [Header("Terrain Movement")]
    public Terrain typeMovement;

    /*
    [Header("Animations")]
    [SerializeField] AnimationClip idle;
    [SerializeField] AnimationClip walk;
    [SerializeField] AnimationClip run;
    [SerializeField] AnimationClip jump_pose;
    */


    private enum CharAnimation
    {
        idle,
        walking,
        running,
        jumping,
        swimming
    }
    private CharAnimation charAnimation;

    float speedConst;
    float gravityConst;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;

    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public bool platformJump = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        speedConst = speed;
        gravityConst = gravity;

        targetRotation = model.localEulerAngles;
       // typeMovement = Terrain.grounded;
    }

    void FixedUpdate()
    {
        if (typeMovement == Terrain.grounded || typeMovement == Terrain.flying) // Separamos grounded de flying para tener un orden
            GroundMovement();

        else if (typeMovement == Terrain.swimming)
            SwimmingMovement();

        Animate();
    }

    #region Ground Movement
    void GroundMovement()
    {
        // Recalculate axes 
        Vector3 forward = model.TransformDirection(Vector3.forward);
        Vector3 right = model.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? rotationSpeed * Input.GetAxis("Horizontal") : 0;

        //if (canMove == false)
        //   return;

        // Rotate model left-right
        model.Rotate(Vector3.up * curSpeedY);

        // Grounded
        if (characterController.isGrounded)
        {
            GlideAttributes(gravityConst, speedConst);
            // We are grounded, so recalculate move direction based on axes
            moveDirection = (forward * curSpeedX); // + (right * curSpeedY);

            if (curSpeedX == 0 && curSpeedY == 0)
                charAnimation = CharAnimation.idle;

            // Jump action
            if (Input.GetButton("Jump") && canMove && platformJump == false)
            {
                Jump(jumpSpeed);
            }
            else
            {
                charAnimation = CharAnimation.walking;
            }
        }
        // Not Grounded
        else
        {
            // Glide action 
            if (Input.GetButton("Glide"))
            {
                GlideAttributes(gravityGlide, speedGlide);
                moveDirection = forward * speed;
            }
            // We're not gliding
            else
            {
                GlideAttributes(gravityConst, speedConst);
            }
        }
        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            if (Input.GetButton("RightClick"))
            {
                playerCameraParent.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 5);
                return;
            }
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)          
                playerCameraParent.rotation = Quaternion.Lerp(playerCameraParent.rotation, model.rotation, cameraDelay * Time.deltaTime);

        }
    }

    #region Fly and Jump

    public void Jump(float _jumpSpeed)
    {
        moveDirection = model.forward * 2;
        moveDirection.y = _jumpSpeed;
        charAnimation = CharAnimation.jumping;
    }

    void GlideAttributes(float _gravity, float _speed)
    {
        gravity = _gravity;
        speed = _speed;
        //Aquí la animación se le pasa por parametro también.
    }

    #endregion Fly and Jump

    #endregion Ground Movement

    #region Swimming Movement
    bool water_Up;  // Si está en superficie (no parece solucionar mucho)
    private Vector3 cameraRotation;
    private Vector3 modelRotation;
    void SwimmingMovement()
    {
        // Si estamos en la parte de arriba del agua: si tenemos el eje X mirando hacia arriba, 
        // colocamos al personaje para que no se pase de altura de agua y el eje lo dejamos para que ya solo vaya de frente
        if((centerWater.position.y - waterPlane.position.y) <= 0 && Mathf.Abs(centerWater.position.y - waterPlane.position.y) < 0.7f) {
            if((model.localRotation.eulerAngles.x) > 180)  // Entre 0 y 90 está mirando hacia abajo, no podemos permitirle más. Desde 0 hasta 360 - 90 está mirando hacia arriba (también habría que limitarlo).
            {
                if(water_Up == false)
                {
                    modelRotation = model.localEulerAngles;
                    modelRotation.x = 0;
                    Quaternion rotationModel = Quaternion.Euler(modelRotation);

                    model.localRotation = Quaternion.Lerp(model.rotation, rotationModel, 0.7f);

                    water_Up = true;
                }

                if (water_Up == true)
                {
                    modelRotation = model.localEulerAngles;
                    modelRotation.x = 0;

                    model.localEulerAngles = modelRotation;

                    Debug.Log("Hey!");
                }
            }
            //Debug.Log(Mathf.Abs(centerWater.position.y - waterPlane.position.y) );
        }

        else
        {
            water_Up = false;
        }

        // Recalculate axes 
        Vector3 forward = model.TransformDirection(Vector3.forward);
        Vector3 right = model.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? rotationSpeed * Input.GetAxis("Horizontal") : 0;

        // Rotate model left-right
        model.Rotate(Vector3.up * curSpeedY);

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            if (Input.GetButton("RightClick"))
            {
                RotateWithLimits(model, rotationSpeed, minRotX, maxRotX);
            }
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) // La camara que siga en el eje X e Y al personaje
            {

                // float toRotateX = Input.GetAxis("Mouse Y") * /*Time.deltaTime * */ rotationSpeed;

                // cameraRotation.x = rotationCamera.x + toRotateX;
                // cameraRotation.x = ClampAngle(targetRotation.x, 60, -60);

                // cameraRotation.y = rotationCamera.y + (playerCameraParent.localEulerAngles.y - model.localEulerAngles.y); // * Input.GetAxis("Vertical");
               // Debug.Log(playerCameraParent.localEulerAngles.y - model.localEulerAngles.y);

                // Movemos la cámara junto con el personaje.
                cameraRotation = playerCameraParent.localEulerAngles;

                cameraRotation.y = (model.localEulerAngles.y > 180) ? model.localEulerAngles.y - 360 : model.localEulerAngles.y;

                // Pasamos los euler a Quaternion
                Quaternion rotationCamera = Quaternion.Euler(cameraRotation);

                //cameraRotation.y = Mathf.Lerp(playerCameraParent.localEulerAngles.y, cameraRotation.y, 0.8f);
                // rotationCamera = Vector3.Lerp(playerCameraParent.localEulerAngles, cameraRotation, 0.9f);

                playerCameraParent.localRotation = Quaternion.Lerp(playerCameraParent.rotation, rotationCamera, cameraDelay * Time.deltaTime);
            }
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

    #region Animations

    bool reachedApex = false;
    void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            if (moveDirection.y > 0f)
            {
                reachedApex = false;

            }
            else
            {
                reachedApex = true;

            }
        }
    }

    void Animate()
    {
        if (charAnimation == CharAnimation.jumping)
        {
            if (!reachedApex)
            {
                //play forward jumping animations
               // animation["jump_pose"].speed = .3f;
               // animation.CrossFade("jump_pose");
            }
            else
            {
                //play reverse jumping animations//aka falling
                //animation["jump_pose"].speed = -.195f;
                //animation.CrossFade("jump_pose");

            }
        }
        else if (charAnimation == CharAnimation.walking)
        {
            //animation.CrossFade("walk");
        }
        else if (charAnimation == CharAnimation.idle)
        {
            //animation.CrossFade("idle");
        }
    }

    #endregion Animations

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
