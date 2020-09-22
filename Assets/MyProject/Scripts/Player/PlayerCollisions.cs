using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is contained by the model.
[RequireComponent(typeof(Rigidbody))]
public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] Yarn.Unity.DialogueRunner dl;

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
            if (Input.GetButton("Interaction"))
            {
                dl.StartDialogue(collision.gameObject.GetComponent<Yarn.Unity.Example.NPC>().talkToNode);
            }
        }
        if (collision.gameObject.GetComponent<CollectibleObject>()) // lo cambiaremos a layers
        {
            if (Input.GetButton("Interaction"))
            {
                collision.gameObject.GetComponent<CollectibleObject>().SetCollected();

            }
        }
        
    }

    void OnTriggerExit(Collider collision)
    {
        // Ocultamos la tecla que pulsar
        if (collision.gameObject.GetComponent<MissionNotes>()) // lo cambiaremos a layers
        {
           /* dl.Stop();
            dl.Clear();
            Debug.Log("joder");*/
        }
    }

}
