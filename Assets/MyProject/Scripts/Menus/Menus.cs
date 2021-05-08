using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
       /* if(canvas != null)
            canvas.enabled = false; */     
    }

    private void Update()
    {
        CUpdate();
    }

    void CUpdate()
    {        
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // Esc
            {
                if (show == false)
                {
                    panelMain.SetActive(true);
                    show = true;
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
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            panelMain.SetActive(true);
            panelOptions.SetActive(false);
        }
    }

    public void ResumeButton()
    {
        // Ocultamos el menú
        panelMain.SetActive(false);
        panelOptions.SetActive(false);
        show = false;
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
