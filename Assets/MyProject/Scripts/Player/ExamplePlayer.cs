using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bindings;
public class ExamplePlayer : MonoBehaviour
{
    Transform tr;
    CharacterController controller;

    MyPlayerActions actions;
    string saveActionsData;

    public float rotationSpeed = 45;
    public float translationSpeed = 1;

    [SerializeField] float speed_maxForward;
    [SerializeField] float speed_minForward;
    [SerializeField] float speed_maxRoll;
    [SerializeField] float speed_maxPitchDown;

    float speed_final;

    [SerializeField] float pitch_speed = 70;
    [SerializeField] float roll_speed = 70;
    [SerializeField] float yaw_speed = 60;

    [SerializeField] float pitch_maxAngle_UP; // A parte del 0, que será nuestra vuelta al ángulo máximo, cuando se recupere de caer, este será su máximo
    [SerializeField] float pitch_maxAngle_DOWN;
    [SerializeField] float roll_maxAngle;
    [SerializeField] float yaw_maxAngle;

    public bool falling;

    enum Direction { Left, Right, Back, Forward };
    Direction actualDirection;
    Direction lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
        controller = GetComponent<CharacterController>();
    }

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

    // Update is called once per frame
    void Update()
    {
        float forwardAxis = actions.Move.Y;
        float rollAxis = actions.Move.X;
        //tr.Rotate(actions.Move.X * Time.deltaTime * rotationSpeed, 0, 0);
        //Pitch();
        Roll(rollAxis);
        Yaw(rollAxis);

        tr.localEulerAngles = targetRotation;

        controller.Move(transform.forward * forwardAxis * Time.deltaTime * translationSpeed);       
    }

    // Giro arriba y abajo. X
    void Pitch()
    {
        //Para girar hacia abajo
        RotateWithLimits(tr, pitch_speed, 0, pitch_maxAngle_DOWN, -1);
        // Para girar hacia arriba
        RotateWithLimits(tr, pitch_speed, 0, 360 - pitch_maxAngle_UP, 1);
        /*float pitch = pitch_speed * Time.deltaTime;
        Debug.Log(canPitch(pitch));
        if (!canPitch(pitch)) return;


        tr.Rotate(pitch, 0, 0);*/
    }

    // Giro hacia los lados con el cuerpo. Z
    void Roll(float rollAxis)
    {
        /*  float roll = rollAxis * roll_speed * Time.deltaTime;
            Debug.Log(canRoll(tr.localEulerAngles.z));
            if (canRoll(tr.localEulerAngles.z) == false) return;

            tr.Rotate(0, 0, roll);
        */

        //RotateWithLimits(tr, roll_speed, 360 - roll_maxAngle, roll_maxAngle, rollAxis);
        Vector3 actualRotation = tr.localEulerAngles;

        if (rollAxis != 0.0f)
        {
            // z rotation
            float toRotateZ = Time.deltaTime * roll_speed * rollAxis;

            targetRotation.z = actualRotation.z + toRotateZ;
            targetRotation.z = ClampAngle(targetRotation.z, -roll_maxAngle, roll_maxAngle);           

        }
    }

    // Giro hacia los lados. Y
    void Yaw(float rollAxis)
    {
        Vector3 actualRotation = tr.localEulerAngles;
        if (rollAxis != 0.0f)
        {
            // z rotation
            float yaw = yaw_speed * Time.deltaTime * rollAxis;

            targetRotation.y = actualRotation.y + yaw;
            targetRotation.y = ClampAngle(targetRotation.y, -yaw_maxAngle, yaw_maxAngle);

        }
        //tr.Rotate(0, yaw, 0);
    }

    bool canPitch(float pitch)
    {
        // bool que te diga si puedes seguir rotando o no
        // derecha || izquierda
        return (pitch > 0 && transform.rotation.x < pitch_maxAngle_UP) || (pitch < 0 && transform.rotation.x > 360 - pitch_maxAngle_DOWN);
    }

    bool canRoll(float rollAxis)
    {
        // bool que te diga si puedes seguir rotando o no
        return (rollAxis >= 0 && transform.rotation.z <= roll_maxAngle) || (rollAxis <= 0 && transform.rotation.z > -roll_maxAngle);
    }


    // En esa función, mientras no haya nada pulsado (y obviamente seguimos volando), el personaje cae en picado con cierto ángulo (60-70º).
    // En este estado, se dilata la velocidad máxima (por lo que desde alguna función que limite la velocidad y contorle que vaya aumentando, volverá a aumentar)
    // Luego, cuando retomamos el control, tiene la velocidad base aumentada, y poco a poco va volviendo al anterior límite de velocidad.
    // Si al retomar el control va a la misma dirección que cuando caía en picado, se recoloca angulando por encima del ángulo al que se vuela y luego se recoloca al ángulo al que se vuela. (Se vuelve a dilatar el ángulo máximo hacia arriba y luego se va cerrando).
    // Si no vamos a la misma dirección, solo gira con la velocidad de giro, no con la de movimiento.
    // Mientras se va en forward tiene una velocidad base que aumenta hasta una máxima, y cuando se retoma, vuelve a tener la base y recuperará el máximo.
    private void Conditions(float forwardAxis, float rollAxis)
    {
        // Si dejamos de usar los controles pero seguimos volando
        if (forwardAxis == 0 && rollAxis == 0)
        {
            // Reseteamos el roll
            // Reset_Roll();

            Mathf.Lerp(speed_final, speed_maxPitchDown, pitch_speed * Time.deltaTime);

            //Pitch();

            falling = true;
        }
        // Si estamos controlando el personaje
        else
        {
            // Si retomamos los mandos en lo que estaba cayendo
            if (falling == true)
            {
                // Vuelve al ángulo 0, pasando por un ángulo más elevado.
                if (isGoingSameDirection())
                {

                    if (tr.rotation.x >= pitch_maxAngle_UP)
                    {
                        falling = false;
                    }
                }
                // Simplemente rota, aquí no hay nada más que volvemos al estado anterior.
                else
                {
                    falling = false;
                }
            }
            // Si ya estábamos volando.
            else
            {
                // La velocidad va aumentando o disminuyendo hasta cierto punto en la velocidad forward
                if (isGoingSameDirection())
                {
                    Mathf.Lerp(speed_final, speed_maxForward, pitch_speed * Time.deltaTime);
                }
                // Aquí dejamos la velocidad forward al mínimo porque estamos girando.
                else
                {
                    speed_final = speed_minForward;
                }
            }

        }
    }

    // Hacemos las comprobaciones para comprobar que estamos yendo hacia alante, incluso si estamos girando, siempre y cuando giremos mientras vamos hacia alante.
    bool isGoingSameDirection()
    {
        if (lastDirection == actualDirection)
            return true;
        else
            return false;
    }

    private Vector3 targetRotation;
    void RotateWithLimits(Transform _objectToRotate, float _rotationSpeed, float _minRotation, float _maxRotation, float input = 1)
    {
        Vector3 actualRotation = _objectToRotate.localEulerAngles;

        if (input != 0.0f)
        {
            // x rotation
            float toRotateX = Time.deltaTime * _rotationSpeed * input;

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

}
