// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Esta clase va activando y desactivando las cámaras según entramos a ciertas zonas. (Se recomienda tener una cámara estándar e ir añadiendo zonas con modificaciones).
/// Esta clase será contenida por cada zona.
/// </summary>
[RequireComponent(typeof(Collider))]
public class CamZone : MonoBehaviour
{
    /// <summary>
    /// Referencia a la cámara virtual que va a controlar cierta zona
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
    /// Cuando el player entra en nuestra zona, activamos nuestra cámara.
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
    /// Cuando el player sale de nuestra zona, desactivaremos nuestra cámara.
    /// Por lo que el player pasará o a otra cámara o a la estándar. (Cuidado con las prioridades de los settings de las cámaras)
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
