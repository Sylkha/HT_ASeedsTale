﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

// This script is contained by MenuManager
public class Menus : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] string sceneName;

    [SerializeField] bool needSave;
    [ShowIf("needSave")] [SerializeField] DataManager dm;
    
    bool show = false;

    // Start is called before the first frame update
    void Start()
    {
        if(canvas != null)
            canvas.enabled = false;

        StartCoroutine(CUpdate());
    }

    IEnumerator CUpdate()
    {
        while (true)
        {
            if (canvas == null) yield return null;
            if (Input.GetButton("Cancel")) // Esc
            {
                if (show == false)
                {
                    canvas.enabled = true;
                    show = true;
                }
                else
                {
                    ResumeButton();
                }
                Debug.Log("Menu");
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
    }

    public void ResumeButton()
    {
        // Ocultamos el menú
        canvas.enabled = false;
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
        SceneManager.LoadScene(sceneName);
    }

}
