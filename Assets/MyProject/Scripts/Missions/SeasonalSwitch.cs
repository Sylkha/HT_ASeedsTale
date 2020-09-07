using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is contained by Altar
[RequireComponent(typeof(Yarn.Unity.Example.NPC))]
public class SeasonalSwitch : MonoBehaviour
{
    [Header("Mission Part")]
    [SerializeField] MissionNotes mn;
    [SerializeField] string n_CompletedMission;
    [SerializeField] string n_NotCompletedMission;

    Yarn.Unity.Example.NPC dialogue;
    bool canSwitch = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = GetComponent<Yarn.Unity.Example.NPC>();
        dialogue.talkToNode = n_NotCompletedMission;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerCollisions>())
        {
            int missionsCompleted = 0;
            for(int i = 0; i < mn.Get_Notes().Length; i++)
            {
                if (mn.Get_Notes()[i].ourObject.GetCollected())
                    missionsCompleted++;
            }
            if(missionsCompleted == mn.Get_Notes().Length)  //Todas están completadas.
            {
                dialogue.talkToNode = n_CompletedMission;
                canSwitch = true;
                // Activamos el cambio de estación desde el dialogo
            }
        }
    }

    // Comando cambio de estación!!

}
