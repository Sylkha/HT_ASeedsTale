using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bindings;
public class Diary : MonoBehaviour
{
    [SerializeField] GameObject diaryMenu;
    bool diaryOpen = false;

    MyPlayerActions actions;

    private void Start()
    {
        actions = Controls.instance.get_actions();
        diaryMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (actions.Diary)
        {
            if(diaryOpen == false) //SFX SE ABRE 
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Diary-Notes/Notes_Open");

            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Diary-Notes/Notes_Close");
                //SFE SE CIERRA 
            }

            diaryOpen = !diaryOpen;
            diaryMenu.SetActive(diaryOpen);
        }
    }
}
