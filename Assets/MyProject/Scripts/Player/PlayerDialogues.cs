using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is contained by Player
[RequireComponent(typeof(Yarn.Unity.Example.NPC))]
public class PlayerDialogues : MonoBehaviour
{
    Yarn.Unity.Example.NPC dialogue;
    [SerializeField] Yarn.Unity.DialogueRunner dl;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = GetComponent<Yarn.Unity.Example.NPC>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("TalkAlone"))
        {
            dl.StartDialogue(dialogue.talkToNode);
        }
    }

    public void SetNode(string _node)
    {
        dialogue.talkToNode = _node;
    }
}
