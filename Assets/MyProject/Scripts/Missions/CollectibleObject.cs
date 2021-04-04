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

    [Header("Only if the object have to reappear somewhere else.")]
    [SerializeField] bool needPositionDelivered;
    [ShowIf("needPositionDelivered")] [SerializeField] Transform positionDelivered;

    bool noteTaken = false;
    bool collected = false;
    bool delivered = false;

    [SerializeField] SeasonalSwitch ss;

    private void Start()
    {
        dialogue = GetComponent<Yarn.Unity.Example.NPC>();
        SetNode(n_NotTakenMission);
    }

    // Esto lo llamamos cuando es un objeto de misión desde el MissionNotes, y desde el comando si es un coleccionable
    [Yarn.Unity.YarnCommand("takeCollectable")]
    public void SetCollected()
    {
        if (noteTaken || !missionObject)
        {
            SetNode(n_TakenMission);
            collected = true;
            this.gameObject.SetActive(false);
            Debug.Log("cabrong");
            //Desde el dialogo le ponemos si cambian la conversación de la nota
            if (positionDelivered != null)
            {
                this.gameObject.transform.position = positionDelivered.position;

                if (missionObject)
                {
                    SetDelivered();
                    ss.ChangeSeason();
                }
            }
        }
        dl.StartDialogue(dialogue.talkToNode);
    }

    public void SetDelivered()
    {
        delivered = true;
        this.gameObject.SetActive(true);
        Debug.Log("HEMOS COGIDO EL OBJETO");
    }
   
    public void SetNoteTaken()
    {        
        noteTaken = true;
        Debug.Log("Oye");
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