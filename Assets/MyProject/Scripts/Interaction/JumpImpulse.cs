// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este script sería contenido por una plataforma de impulso/seta
/// </summary>
public class JumpImpulse : MonoBehaviour
{
    /// <summary>
    /// Cantidad de impulso aplicado
    /// </summary>
    [SerializeField] private float impulse = 1;

    /// <summary>
    /// Accedemos al componente Movement del player para aplicarle ese impulso y que haga un salto sin necesidad de que el player toque ninguna tecla.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Movement>())
        {
            other.GetComponent<Movement>().Jump(impulse);
            other.GetComponent<Movement>().platformJump = true;
        }
    }

}
