// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Bindings;

/// <summary>
/// Este script lo contiene el MenuManager
/// </summary>
public class Menus : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject panelInicio; 
    [SerializeField] GameObject panelMain; 
    [SerializeField] GameObject panelOptions;  
    [SerializeField] GameObject panelQuit;  
    [SerializeField] string sceneNameToChange;
    [SerializeField] string sceneName;

//    [SerializeField] bool needSave;
//    [ShowIf("needSave")] [SerializeField] DataManager dm;
    
    bool show = false;

    [SerializeField] GameObject menuFirstButton;
    [SerializeField] GameObject menuInGameFirstButton;
    [SerializeField] GameObject optionsFirstButton;
    [SerializeField] GameObject optionsCloseButton;
    [SerializeField] GameObject optionsInGameCloseButton;

    bool check = false;
    [SerializeField] GameObject buttonCheckQuit;
    [SerializeField] GameObject buttonCheckQuit_Menu;
    [SerializeField] GameObject buttonQuit;
    [SerializeField] GameObject buttonQuit_Menu;

    MyPlayerActions actions;

    void Start()
    {
        actions = Controls.instance.get_actions();

        // hay que limpiarlo primero
        EventSystem.current.SetSelectedGameObject(null);
        
        EventSystem.current.SetSelectedGameObject(menuFirstButton);
    }

    private void Update()
    {
        CUpdate();
    }

    /// <summary>
    /// Esta función se encarga de que, desde la escena Game, mostremos y ocultemos el menú.
    /// </summary>
    void CUpdate()
    {        
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            if (actions.Exit.WasPressed) // Esc
            {
                if (show == false)
                {
                    //SFX ABRIR MENU
                    Movement.instance.canMove = false;
                    panelMain.SetActive(true);
                    show = true;

             
                    EventSystem.current.SetSelectedGameObject(null);
                  
                    EventSystem.current.SetSelectedGameObject(menuInGameFirstButton);                    
                }
                else
                {       
                    //SFX SE CIERRA MENÚ
                    ResumeButton();
                }
                Debug.Log("Menu");
            }
        }
    }

    /// <summary>
    /// Esta función cierra el menú de opciones y vuelve al inicio del menú
    /// </summary>
    public void ReturnButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            panelMain.SetActive(true);
            panelOptions.SetActive(false);
            panelQuit.SetActive(false);


            EventSystem.current.SetSelectedGameObject(optionsInGameCloseButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(optionsCloseButton);
            panelInicio.SetActive(true);

        }
    }

    /// <summary>
    /// Función que, al abrir el botón de opciones, selecciona cierto botón.
    /// </summary>
    public void OptionsButton()
    {
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    /// <summary>
    /// Esta función oculta el menú
    /// </summary>
    public void ResumeButton()
    {
        panelMain.SetActive(false);
        panelOptions.SetActive(false);
        show = false;
        if (SceneManager.GetActiveScene().name == sceneName)
            Movement.instance.canMove = true;

        Debug.Log("Resume");
    }

    /// <summary>
    /// Función para cuando se pueda guardar el juego.
    /// </summary>
    public void SaveButton()
    {
        // Guardamos
        //dm.Save();
        Debug.Log("Save");
    }

    /// <summary>
    /// Función para cuando se pueda guardar el juego.
    /// </summary>
    public void SaveNExitButton()
    {
        // Guardamos
        Debug.Log("Save and Exit");
        //dm.Save();
        Application.Quit();
    }

    /// <summary>
    /// Función para salir del juego.
    /// </summary>
    public void ExitGameButton()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    /// <summary>
    /// Función para cambiar de escena.
    /// </summary>
    public void ChangeSceneButton()
    {
        // Ocultamos el menú
        panelMain.SetActive(false);
        SceneManager.LoadScene(sceneNameToChange);
    }

    /// <summary>
    /// Función para cuando el jugador quiere salir del juego: hacemos otra comprobación, y seleccionamos primero el botón "no".
    /// </summary>
    public void Quit_Check()
    {
        // Cuando se abre el panel que vuelve a preguntar al jugador.
        if(check == false)
        {
            check = true;

            // clear the selected object
            EventSystem.current.SetSelectedGameObject(null);
            // set new selected object
            if (SceneManager.GetActiveScene().name == sceneName)
                EventSystem.current.SetSelectedGameObject(buttonCheckQuit);
            else
                EventSystem.current.SetSelectedGameObject(buttonCheckQuit_Menu);
            Debug.Log(buttonCheckQuit);
        }
        // Cuando se cierra el panel
        else
        {
            check = false;

            // clear the selected object
            EventSystem.current.SetSelectedGameObject(null);
            // set new selected object
            if (SceneManager.GetActiveScene().name == sceneName)
                EventSystem.current.SetSelectedGameObject(buttonQuit);
            else
                EventSystem.current.SetSelectedGameObject(buttonQuit_Menu);

        }
    }



}
