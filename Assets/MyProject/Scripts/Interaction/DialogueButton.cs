using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueButton : MonoBehaviour
{
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject option1Button;

    bool selected = false;

    // Update is called once per frame
    void Update()
    {
        if (PlayerCollisions.instance.is_chating())
        {
            if(continueButton.activeSelf == true)
            {
                // clear the selected object
                EventSystem.current.SetSelectedGameObject(null);
                // set new selected object
                EventSystem.current.SetSelectedGameObject(continueButton);
            }

            if (option1Button.activeSelf == true && selected == false)
            {
                // clear the selected object
                EventSystem.current.SetSelectedGameObject(null);
                // set new selected object
                EventSystem.current.SetSelectedGameObject(option1Button);
                selected = true;
            }
            else if(option1Button.activeSelf == false && selected == true)
            {
                selected = false;
            }
        }
    }
}

