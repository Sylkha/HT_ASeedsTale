using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamChange : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] CamManager manager;

    void Start()
    {
        virtualCamera.enabled = false;
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Cuando entramos, desactivamos la que era la actual y setteamos esta como la actual.
            if (manager.actualVCamera != null)
                manager.actualVCamera.enabled = false;

            manager.actualVCamera = virtualCamera;
            virtualCamera.enabled = true;
        }
    }
}
