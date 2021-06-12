using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

// This script is contained by Objects we can interact with
[RequireComponent(typeof(Yarn.Unity.Example.NPC))]
public class CollectibleObject : MonoBehaviour
{
    [Header("")]
    Yarn.Unity.Example.NPC dialogue;
    [SerializeField] Yarn.Unity.DialogueRunner dl;

    [Header("Is this a mission object?")]
    [SerializeField] bool missionObject;
    [ShowIf("missionObject")] [SerializeField] string n_NotTakenMission;
    [ShowIf("missionObject")] [SerializeField] string n_TakenMission;
    [ShowIf("missionObject")] [SerializeField] string nodeFinal;
    [FMODUnity.EventRef]
    public string objectSFX;

    [Header("Only if the object have to reappear somewhere else.")]
    [SerializeField] bool needPositionDelivered;
    [ShowIf("needPositionDelivered")] [SerializeField] Transform positionDelivered;

    bool noteTaken = false;
    bool collected = false;
    bool delivered = false;

    private void Start()
    {
        dialogue = GetComponent<Yarn.Unity.Example.NPC>();
        SetNode(n_NotTakenMission);
    }

    public void chat()
    {
        dl.StartDialogue(dialogue.talkToNode);
    }

    // Esto lo llamamos cuando es un objeto de misión desde el MissionNotes, y desde el comando si es un coleccionable
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
                this.gameObject.transform.position = positionDelivered.position;

                if (missionObject)
                {
                    SetDelivered();
                }
            }
        }        
    }

    public void SetDelivered()
    {
        delivered = true;
        this.gameObject.SetActive(true);
        SetNode(n_TakenMission);
        Debug.Log("HEMOS COGIDO EL OBJETO");
    }
   
    public void SetNoteTaken()
    {
        //SFX Collect/Pick Note
        FMODUnity.RuntimeManager.PlayOneShot("event:/Diary-Notes/Notes_Open");
        noteTaken = true;
        SetNode(n_TakenMission);
    }

    public void SetNode(string _node)
    {
        dialogue.talkToNode = _node;
    }

    public bool GetCollected()
    {
        return collected;
    }
    public bool GetNoteTaken()
    {
        return noteTaken;
    }
    public bool GetDelivered()
    {
        return delivered;
    }
}