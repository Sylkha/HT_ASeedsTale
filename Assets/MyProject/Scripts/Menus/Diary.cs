using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diary : MonoBehaviour
{
    [SerializeField] GameObject diaryMenu;
    bool diaryOpen = false;

    private void Start()
    {
        diaryMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            diaryOpen = !diaryOpen;
            diaryMenu.SetActive(diaryOpen);
        }
        
    }

    /*
    void OpenClose()
    {
        if(diaryOpen == true)
        {
            diaryMenu.SetActive(false);
            diaryOpen = false;
        }
        else
        {
            diaryMenu.SetActive(true);
            diaryOpen = true;
        }
    }*/
}
