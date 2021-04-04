using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Movements.Fly
{
    public class FlyingMovement : MonoBehaviour
    {
        CharacterController characterController;
        Transform tr;
        Vector3 dir;
        Vector3 direction;

        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            tr = GetComponent<Transform>();
            speed_final = speed_minForward;
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        float pitchAxis;
        float rollAxis;

        float pitch_speed;
        float roll_speed;

        [SerializeField] float speed_maxForward;
        [SerializeField] float speed_minForward;
        [SerializeField] float speed_maxRoll;
        [SerializeField] float speed_maxPitchDown;

        float speed_final;

        [SerializeField]float yaw_maxSpeed;

        [SerializeField] float pitch_maxAngle_UP; // A parte del 0, que será nuestra vuelta al ángulo máximo, cuando se recupere de caer, este será su máximo
        [SerializeField] float pitch_maxAngle_DOWN;
        [SerializeField] float roll_maxAngle;

        public bool falling;

        enum Direction { Left, Right};
        Direction actualDirection;
        Direction lastDirection;

        public void Fly(float _PitchAxis, float _RollAxis, Transform camera_transf)
        {
            // Actualizamos los axis
            pitchAxis = _PitchAxis;
            rollAxis = _RollAxis;


            Move(camera_transf);
            Rotate();
            Direction_Track();
            Conditions();

            characterController.Move(direction * Time.deltaTime);
        }
        [SerializeField] float turnSmoothTime = 0.1f;
        float turnSmoothVelocity;
        // En este movimiento tendremos que hacer el sumatorio de lo que tenemos para que se mueva.
        private void Move(Transform camera_transf)
        {
            dir.x = pitchAxis;
            dir.z = Mathf.Abs(rollAxis);
            Vector3 camDirection = camera_transf.rotation * dir;

            direction = new Vector3(camDirection.x, 0f, camDirection.z).normalized * speed_final;
        /*    if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }*/
        }
        // En esa función, mientras no haya nada pulsado (y obviamente seguimos volando), el personaje cae en picado con cierto ángulo (60-70º).
        // En este estado, se dilata la velocidad máxima (por lo que desde alguna función que limite la velocidad y contorle que vaya aumentando, volverá a aumentar)
        // Luego, cuando retomamos el control, tiene la velocidad base aumentada, y poco a poco va volviendo al anterior límite de velocidad.
        // Si al retomar el control va a la misma dirección que cuando caía en picado, se recoloca angulando por encima del ángulo al que se vuela y luego se recoloca al ángulo al que se vuela. (Se vuelve a dilatar el ángulo máximo hacia arriba y luego se va cerrando).
        // Si no vamos a la misma dirección, solo gira con la velocidad de giro, no con la de movimiento.
        // Mientras se va en forward tiene una velocidad base que aumenta hasta una máxima, y cuando se retoma, vuelve a tener la base y recuperará el máximo.
        private void Conditions()
        {
            // Si dejamos de usar los controles pero seguimos volando
            if(pitchAxis == 0 && rollAxis == 0)
            {
                // Reseteamos el roll
                Reset_Roll();

                Mathf.Lerp(speed_final, speed_maxPitchDown, pitch_speed * Time.deltaTime);

                //Pitch();

                falling = true;
            }
            // Si estamos controlando el personaje
            else
            {
                // Si retomamos los mandos en lo que estaba cayendo
                if(falling == true)
                {
                    // Vuelve al ángulo 0, pasando por un ángulo más elevado.
                    if (isForward())
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
                    if (isForward())
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

        void Direction_Track()
        {
            if (rollAxis > 0)
            {
                lastDirection = actualDirection;
                actualDirection = Direction.Right;
            }
            else if (rollAxis < 0)
            {
                lastDirection = actualDirection;
                actualDirection = Direction.Left;
            }
        }

        // Hacemos las comprobaciones para comprobar que estamos yendo hacia alante, incluso si estamos girando, siempre y cuando giremos mientras vamos hacia alante.
        bool isForward()
        {
            if (lastDirection == actualDirection)
                return true;
            else
                return false;
        }

        Vector3 targetRotation;
        // Giro arriba y abajo. X
        void Pitch()
        {
            Debug.Log(canPitch());
            //if (!canPitch()) return;

            float pitch = pitch_speed * Time.deltaTime;
            //transform.Rotate(pitch, 0, 0);
            Vector3 actualRotation = tr.localEulerAngles;

            targetRotation.x = actualRotation.x + pitch;
            targetRotation.x = ClampAngle(targetRotation.x, pitch_maxAngle_DOWN, pitch_maxAngle_UP);

            tr.localEulerAngles = targetRotation;
          
        }
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < 0.0f)
                angle = 360.0f + angle;
            if (angle > 180.0f)
                return Mathf.Max(angle, 360.0f + min);
            return Mathf.Min(angle, max);
        }



        // Giro hacia los lados con el cuerpo. Z
        void Roll()
        {
            //if (!canRoll()) return;

            float roll = rollAxis * roll_speed * Time.deltaTime;
            //tr.Rotate(0, 0, roll);

            Vector3 actualRotation = tr.localEulerAngles;

            targetRotation.z = actualRotation.x + roll;
            targetRotation.z = ClampAngle(targetRotation.z, -roll_maxAngle, roll_maxAngle);

            tr.localEulerAngles = targetRotation;
        }
        // Giro hacia los lados. Y
        void Yaw()
        {
            float yaw = transform.rotation.z * yaw_maxSpeed / 90 * Time.deltaTime;
            //tr.Rotate(0, yaw, 0);
            Vector3 actualRotation = tr.localEulerAngles;

            targetRotation.y = actualRotation.x + yaw;

            tr.localEulerAngles = targetRotation;
        }

        
        bool canPitch() 
        {
            // bool que te diga si puedes seguir rotando o no
            // derecha || izquierda
            return (pitchAxis > 0 && transform.rotation.x < pitch_maxAngle_UP) || (pitchAxis < 0 && transform.rotation.x > -pitch_maxAngle_DOWN);
        }
        
        bool canRoll() 
        {
            // bool que te diga si puedes seguir rotando o no
            return (rollAxis > 0 && transform.rotation.z < roll_maxAngle) || (rollAxis < 0 && transform.rotation.z > -roll_maxAngle);
        }

        void Reset_Roll()
        {
            float rotationZ = Mathf.LerpAngle(tr.rotation.z, 0, roll_speed * Time.deltaTime);

            tr.Rotate(0, 0, rotationZ);

            /*
            Vector3 forward = transform.forward;
            forward.y = 0f;
            if (forward.sqrMagnitude > 0f)
            {
                forward.Normalize();
                Vector3 vector = transform.InverseTransformDirection(forward);
                pitchAngle = Mathf.Atan2(vector.y, vector.z);
                Vector3 direction = Vector3.Cross(Vector3.up, forward);
                Vector3 vector2 = transform.InverseTransformDirection(direction);
                rollAngle = Mathf.Atan2(vector2.y, vector2.x);
            }

            bankedTurnAmount = Mathf.Sin(rollAngle);

            if (rollAxis == 0)
            {
                rollAxis = -rollAngle * autoRollLevel;
            }
            if (pitchAxis == 0)
            {
                pitchAxis = -pitchAngle * autoPitchLevel;
                pitchAxis -= Mathf.Abs(bankedTurnAmount * bankedTurnAmount * autoTurnPitch);
            }
            */
        }

        void Rotate()
        {
            Pitch();
            Roll();
            Yaw();
        }
    }
}