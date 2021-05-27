using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Bindings;
// This script is contained by MenuManager
public class Menus : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject panelMain; 
    [SerializeField] GameObject panelOptions;  
    [SerializeField] string sceneName;

    [SerializeField] bool needSave;
    [ShowIf("needSave")] [SerializeField] DataManager dm;
    
    bool show = false;

    [SerializeField] GameObject menuFirstButton;
    [SerializeField] GameObject menuInGameFirstButton;
    [SerializeField] GameObject optionsFirstButton;
    [SerializeField] GameObject optionsCloseButton;
    [SerializeField] GameObject optionsInGameCloseButton;

    MyPlayerActions actions;

    // Start is called before the first frame update
    void Start()
    {
        actions = Controls.instance.get_actions();

        // clear the selected object
        EventSystem.current.SetSelectedGameObject(null);
        // set new selected object
        EventSystem.current.SetSelectedGameObject(menuFirstButton);

        /* if(canvas != null)
             canvas.enabled = false; */
    }

    private void Update()
    {
        CUpdate();
        Debug.Log(EventSystem.current);
    }

    void CUpdate()
    {        
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            if (actions.Exit.WasPressed) // Esc
            {
                if (show == false)
                {
                    Movement.instance.canMove = false;
                    panelMain.SetActive(true);
                    show = true;

                    // clear the selected object
                    EventSystem.current.SetSelectedGameObject(null);
                    // set new selected object
                    EventSystem.current.SetSelectedGameObject(menuInGameFirstButton);                    
                }
                else
                {                   
                    ResumeButton();
                }
                Debug.Log("Menu");
            }
        }
    }

    public void ReturnButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            panelMain.SetActive(true);
            panelOptions.SetActive(false);

            // clear the selected object
            EventSystem.current.SetSelectedGameObject(optionsInGameCloseButton);
        }
        else
            EventSystem.current.SetSelectedGameObject(optionsCloseButton);
    }

    public void OptionsButton()
    {
        // clear the selected object
        EventSystem.current.SetSelectedGameObject(null);
        // set new selected object
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void ResumeButton()
    {
        // Ocultamos el menú
        panelMain.SetActive(false);
        panelOptions.SetActive(false);
        show = false;
        if (SceneManager.GetActiveScene().name == sceneName)
            Movement.instance.canMove = true;

        Debug.Log("Resume");
    }

    public void SaveButton()
    {
        // Guardamos
        dm.Save();
        Debug.Log("Save");
    }

    public void SaveNExitButton()
    {
        // Guardamos
        Debug.Log("Save and Exit");
        dm.Save();
        Application.Quit();
    }

    public void ExitGameButton()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void ChangeSceneButton()
    {
        // Ocultamos el menú
        panelMain.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }



}
