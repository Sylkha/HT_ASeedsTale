// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bindings;

/// <summary>
/// Este script lo contiene el modelo del Player. Nos sirve para los eventos que se produzcan en trigger con el personaje.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerCollisions : MonoBehaviour
{
    public static PlayerCollisions instance;

    [SerializeField] Yarn.Unity.DialogueRunner dl;
    bool chating = false;

    MyPlayerActions actions;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        actions = Controls.instance.get_actions();
    }

    void OnTriggerStay(Collider collision)
    {
        // Si interactuamos con un NPC para dialogar
        if (collision.gameObject.GetComponent<Yarn.Unity.Example.NPC>()) //&& !collision.gameObject.GetComponent<MissionNotes>())
        {
            if (actions.Interacion && chating == false)
            {
                chating = true;
                dl.StartDialogue(collision.gameObject.GetComponent<Yarn.Unity.Example.NPC>().talkToNode);
            }
        }
        // Si interactuamos con una nota
        if (actions.Interacion && collision.gameObject.GetComponent<NoteCollisions>() && chating == false)
        {
            //SFX Interact Note
            FMODUnity.RuntimeManager.PlayOneShot("event:/Diary-Notes/Notes_Open");
            chating = true;
        }
        // Si interactuamos con un objeto
        if (collision.gameObject.GetComponent<CollectibleObject>()) // lo cambiaremos a layers
        {
            if (actions.Interacion && chating == false)
            {
                //SFX Interact with Object
                FMODUnity.RuntimeManager.PlayOneShot(collision.gameObject.GetComponent<CollectibleObject>().objectSFX);
                chating = true;
                collision.gameObject.GetComponent<CollectibleObject>().chat();
            }

        }
        
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<Yarn.Unity.Example.NPC>()) //&& !collision.gameObject.GetComponent<MissionNotes>())
        {
            chating = false;
        }
        chating = false;
        if (collision.gameObject.GetComponent<CollectibleObject>()) // lo cambiaremos a layers
        {
            chating = false;
        }
        // Ocultamos la tecla que pulsar
        if (collision.gameObject.GetComponent<MissionNotes>()) // lo cambiaremos a layers
        {
            chating = false;
           /* dl.Stop();
            dl.Clear();
            Debug.Log("joder");*/
        }
    }

    public bool is_chating() { return chating; }
}
