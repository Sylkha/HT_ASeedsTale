using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class AudioButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    FMOD.Studio.EventInstance ClickSound;

    private void Start()
    {
        
    }

    public void buttonSfxClick()
    {
        
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click", GetComponent<Transform>().position);
    }
    public void OnPointerEnter(PointerEventData ped)
    {
        Debug.Log("ButtonHover");
        
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enviroment/Nature/Amb_Ocean", GetComponent<Transform>().position);
    }

    /*public void OnPointerDown(PointerEventData ped)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_OnSelect");
    }*/

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("ButtonSelected");
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_OnSelect", GetComponent<Transform>().position);
    }

    public void backButtonSfxClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click", GetComponent<Transform>().position);
        
    }

    public void pauseButtonSfxClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Pause", GetComponent<Transform>().position);
    }
}
