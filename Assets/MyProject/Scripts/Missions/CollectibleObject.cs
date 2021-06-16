// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

/// <summary>
/// Este script lo contiene cada objeto con el que podemos interactuar
/// </summary>
[RequireComponent(typeof(Yarn.Unity.Example.NPC))]
public class CollectibleObject : MonoBehaviour
{
    [Header("References")]
    Yarn.Unity.Example.NPC dialogue;
    [SerializeField] Yarn.Unity.DialogueRunner dl;

    /// <summary>
    /// Si nuestro objeto es un objeto de misión, lo marcamos y 
    /// le indicamos los nodos de los diálogos que dirá si no se ha cogido su misión, cuando se haya cogido y cuando se haya cogido el propio objeto.
    /// </summary>
    [Header("Is this a mission object?")]
    [SerializeField] bool missionObject;
    [ShowIf("missionObject")] [SerializeField] string n_NotTakenMission;
    [ShowIf("missionObject")] [SerializeField] string n_TakenMission;
    [ShowIf("missionObject")] [SerializeField] string nodeFinal;

    /// <summary>
    /// Si el objeto al interactuar con él tiene que reaparecer en otro sitio, lo marcamos e indicamos la posición.
    /// </summary>
    [Header("Only if the object have to reappear somewhere else.")]
    [SerializeField] bool needPositionDelivered;
    [ShowIf("needPositionDelivered")] [SerializeField] Transform positionDelivered;

    bool noteTaken = false;
    bool collected = false;
    bool delivered = false;

    [FMODUnity.EventRef]
    public string objectSFX;

    private void Start()
    {
        dialogue = GetComponent<Yarn.Unity.Example.NPC>();
        SetNode(n_NotTakenMission);
    }

    public void chat()
    {
        dl.StartDialogue(dialogue.talkToNode);
    }

    /// <summary>
    /// Esta función la llamamos a través del comando takeCollectable en los scripts de diálogo
    /// </summary>    
    [Yarn.Unity.YarnCommand("takeCollectable")]
    public void SetCollected()
    {
        if (noteTaken || !missionObject)
        {
            collected = true;
            //SFX Collect/Pick Object
            this.gameObject.SetActive(false);
            //Desde el dialogo le ponemos si cambian la conversación de la nota
            if (positionDelivered != null)
            {
                this.gameObject.transform.localPosition = positionDelivered.localPosition;

                if (missionObject)
                {
                    SetDelivered();
                }
            }
        }        
    }

    /// <summary>
    /// Esta función hace los cambios respectivos tras que se ha cogido el objeto y ya está en otra posición.
    /// </summary>
    public void SetDelivered()
    {
        delivered = true;
        this.gameObject.SetActive(true);
        SetNode(nodeFinal);
        Debug.Log("HEMOS COGIDO EL OBJETO");
    }
   
    /// <summary>
    /// Esta función se activa desde MissionNotes para indicar que la misión de este objeto ha sido cogida.
    /// </summary>
    public void SetNoteTaken()
    {
        //SFX Collect/Pick Note
        FMODUnity.RuntimeManager.PlayOneShot("event:/Diary-Notes/Notes_Open");
        noteTaken = true;
        SetNode(n_TakenMission);
    }

    /// <summary>
    /// Esta función cambia de nodo en el diálogo,
    /// </summary>
    /// <param name="_node"></param>
    public void SetNode(string _node)
    {
        dialogue.talkToNode = _node;
    }

    public bool GetCollected() { return collected; }
    public bool GetNoteTaken() { return noteTaken; }
    public bool GetDelivered() { return delivered; }
}