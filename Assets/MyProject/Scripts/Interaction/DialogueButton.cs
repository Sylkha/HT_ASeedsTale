using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueButton : MonoBehaviour
{
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject option1Button;

    // Update is called once per frame
    void Update()
    {
        if(continueButton.activeSelf == true)
        {
            // clear the selected object
            EventSystem.current.SetSelectedGameObject(null);
            // set new selected object
            EventSystem.current.SetSelectedGameObject(continueButton);
        }

        if (option1Button.activeSelf == true)
        {
            // clear the selected object
            EventSystem.current.SetSelectedGameObject(null);
            // set new selected object
            EventSystem.current.SetSelectedGameObject(option1Button);
        }
    }
}

