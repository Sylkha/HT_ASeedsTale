// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Esta clase va activando y desactivando las c�maras seg�n entramos a ciertas zonas. (Se recomienda tener una c�mara est�ndar e ir a�adiendo zonas con modificaciones).
/// Esta clase ser� contenida por cada zona.
/// </summary>
[RequireComponent(typeof(Collider))]
public class CamZone : MonoBehaviour
{
    /// <summary>
    /// Referencia a la c�mara virtual que va a controlar cierta zona
    /// </summary>
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera.enabled = false;
    }

    /// <summary>
    /// Por si acaso, nos aseguramos que sea trigger.
    /// </summary>
    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    /// <summary>
    /// Cuando el player entra en nuestra zona, activamos nuestra c�mara.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = true;
        }
    }

    /// <summary>
    /// Cuando el player sale de nuestra zona, desactivaremos nuestra c�mara.
    /// Por lo que el player pasar� o a otra c�mara o a la est�ndar. (Cuidado con las prioridades de los settings de las c�maras)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = false;
        }
    }
}
