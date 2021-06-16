// Autor: Jorge Aranda y Silvia Osoro
// jaranlopz@gmail.com
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Esta clase la tendr�n los botones de interfaz que queramos que suenen con cierto comportamiento del usuario.
/// </summary>
public class AudioButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    /// <summary>
    /// Esta funci�n se activa cuando hacemos click.
    /// </summary>
    public void buttonSfxClick()
    {       
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click", GetComponent<Transform>().position);
    }

    /// <summary>
    /// Esta funci�n se activa cuando hacemos hover sobre el objeto. (Hereda de IPointerEnterHandler)
    /// </summary>
    /// <param name="ped"></param>
    public void OnPointerEnter(PointerEventData ped)
    {
        Debug.Log("ButtonHover");
        
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enviroment/Nature/Amb_Ocean", GetComponent<Transform>().position);
    }

    /// <summary>
    /// Esta funci�n se activa cuando un bot�n est� seleccionado. (Hereda de ISelectHandler)
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("ButtonSelected");
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_OnSelect", GetComponent<Transform>().position);
    }

    /// <summary>
    /// Esta funci�n es espec�fica para el bot�n de "volver".
    /// </summary>
    public void backButtonSfxClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Click", GetComponent<Transform>().position);
        
    }

    /// <summary>
    /// Esta funci�n es para el bot�n de pausa.
    /// </summary>
    public void pauseButtonSfxClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Pause", GetComponent<Transform>().position);
    }
}
