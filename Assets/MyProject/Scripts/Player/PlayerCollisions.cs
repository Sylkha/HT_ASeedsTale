using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bindings;

// This script is contained by the model.
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

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Yarn.Unity.Example.NPC>())
        {
            //Le aparecerá al personaje la tecla que pulsar
        }
        if (collision.gameObject.GetComponent<CollectibleObject>()) // lo cambiaremos a layers
        {
            //Le aparecerá al personaje la tecla que pulsar
        }
        if (collision.gameObject.GetComponent<MissionNotes>() && collision.gameObject.GetComponent<Yarn.Unity.Example.NPC>()) // lo cambiaremos a layers
        {
            
            //dl.StartDialogue(collision.gameObject.GetComponent<Yarn.Unity.Example.NPC>().talkToNode);
            //Le aparecerá al personaje la tecla que pulsar
        }
    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.GetComponent<Yarn.Unity.Example.NPC>()) //&& !collision.gameObject.GetComponent<MissionNotes>())
        {
            if (actions.Interacion && chating == false)
            {
                chating = true;
                dl.StartDialogue(collision.gameObject.GetComponent<Yarn.Unity.Example.NPC>().talkToNode);
            }
        }
        if (actions.Interacion && collision.gameObject.GetComponent<NoteCollisions>())
        {
            //SFX Interact Note
            FMODUnity.RuntimeManager.PlayOneShot("event:/Diary-Notes/Notes_Open");
        }
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
