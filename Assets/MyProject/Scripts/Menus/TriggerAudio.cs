// Autor: Jorge Aranda
// jaranlopz@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase activa el audio en el inicio.
/// </summary>
public class TriggerAudio : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string Event;
    public bool PlayOnAwake;
    

    public void PlayOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(Event, gameObject);
    }

    private void Start()
    {
        if (PlayOnAwake)
            PlayOneShot();
    }
}
