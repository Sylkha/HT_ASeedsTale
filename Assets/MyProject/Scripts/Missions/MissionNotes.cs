using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This script is contained by MissionNotesManager
public class MissionNotes : MonoBehaviour
{
    [System.Serializable]
    public struct Notes
    {
        public Yarn.Unity.Example.NPC dialogue;
        public CollectibleObject ourObject;
        public string noteNumber;
        public string n_NotTakenMission;
        public string n_TakenMission;
        public string n_OTHERMissTaken;
        public string n_FinishedMission;
        public TMP_Text textDiary;
    }

    [Header("Notes")]
    [SerializeField] Notes[] notes = default;

    private void Start()
    {
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].dialogue.talkToNode = notes[i].n_NotTakenMission;
            if(notes[i].textDiary != null)
                notes[i].textDiary.enabled = false;
        }
    }

    public Notes[] Get_Notes()
    {
        return notes;
    }

    // We call this in the PlayerCollision. Only if we didnt take a note yet. (Esto se controla desde ese script). Cuando reiniciemos el nivel, todo irá a false.
    // Comando para poner en el dialogo: <<take NombreObjeto noteNumber>>
    [Yarn.Unity.YarnCommand("take")]
    public void TakeNote(string _note)
    {
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].dialogue.talkToNode = notes[i].n_OTHERMissTaken;
            if (notes[i].noteNumber == _note)
            {
                notes[i].dialogue.talkToNode = notes[i].n_TakenMission;
                notes[i].ourObject.SetNoteTaken();
                if (notes[i].textDiary != null)
                    notes[i].textDiary.enabled = true;
            }
        }
    }

    // Todos vuelven a sus respectivos diálogos, pero si ya han sido finalizados, se indica. Antes que este, va el comando "takeCollectable" que llama a SetCollected
    [Yarn.Unity.YarnCommand("resetMissions")]
    public void ResetMissions()
    {
        int counter = 0;
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].dialogue.talkToNode = notes[i].n_NotTakenMission;
            if (notes[i].ourObject.GetCollected())
            {
                notes[i].dialogue.talkToNode = notes[i].n_FinishedMission;
                counter++;
            }
        }
        if(counter >= notes.Length)
        {
            Debug.Log("Cambio de estación!");
            Debug.Log(notes.Length + " cantidad de misiones completadas");
            SeasonalSwitch.instance.ChangeSeason();
        }
    }
}
