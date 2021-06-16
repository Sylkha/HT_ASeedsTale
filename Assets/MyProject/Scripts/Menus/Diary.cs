// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bindings;
/// <summary>
/// Esta clase se encarga de abrir y cerrar el diario.
/// </summary>
public class Diary : MonoBehaviour
{
    [SerializeField] GameObject diaryMenu;
    bool diaryOpen = false;

    /// <summary>
    /// Referencia a nuestro mapeado de acciones.
    /// </summary>
    MyPlayerActions actions;

    private void Start()
    {
        actions = Controls.instance.get_actions();
        diaryMenu.SetActive(false);
    }

    /// <summary>
    /// Abrimos y cerramos el diario.
    /// </summary>
    void Update()
    {
        if (actions.Diary.WasPressed)
        {
            if(diaryOpen == false) 
            {
                //SFX SE ABRE 
                FMODUnity.RuntimeManager.PlayOneShot("event:/Diary-Notes/Notes_Open");
            }
            else
            {
                //SFX SE CIERRA 
                FMODUnity.RuntimeManager.PlayOneShot("event:/Diary-Notes/Notes_Close");
            }

            diaryOpen = !diaryOpen;
            diaryMenu.SetActive(diaryOpen);
        }
    }
}
