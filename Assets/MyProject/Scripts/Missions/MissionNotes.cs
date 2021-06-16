// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Este script lo tiene MissionNotes
/// </summary>
public class MissionNotes : MonoBehaviour
{
    /// <summary>
    /// Struct de los datos necesarios por cada nota:
    /// </summary>
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
        public GameObject textDiary;
    }

    [Header("Notes")]
    [SerializeField] Notes[] notes = default;

    /// <summary>
    /// Dejamos las notas a "0" (como tendrían que estar al inicio de la partida (no guardada))
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].dialogue.talkToNode = notes[i].n_NotTakenMission;
            if(notes[i].textDiary != null)
                notes[i].textDiary.SetActive(false);
        }
    }

    public Notes[] Get_Notes()
    {
        return notes;
    }


    /// <summary>
    /// Esta clase indica que hemos cogido la nota acompañado de el número de nota.
    /// Comando para poner en el dialogo: <</ take NombreObjeto noteNumber>>
    /// </summary>
    /// <param name="_note"></param>
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
                    notes[i].textDiary.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Esta función se hace cada vez que se termina una misión:
    /// Todos vuelven a sus respectivos diálogos, pero si ya han sido finalizados, se indica. Si están todas completas, se cambia de estación
    /// Antes que este, va el comando "takeCollectable" que llama a SetCollected
    /// </summary>
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
