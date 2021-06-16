// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta funci�n la tiene cada nota. Simplemente le crea un sprite que aparece para indicar que el player est� a rango de la misi�n y puede interactuar con ella.
/// </summary>
public class NoteCollisions : MonoBehaviour
{
    [SerializeField] GameObject prefabImageDialogue;
    GameObject imageDialogue;

    void Start()
    {
        imageDialogue = Instantiate(prefabImageDialogue, transform);
        imageDialogue.SetActive(false);
    }
    private void Update()
    {
        //imageDialogue.transform.LookAt(Camera.main.transform.position, -Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        imageDialogue.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        imageDialogue.SetActive(false);
    }
}
