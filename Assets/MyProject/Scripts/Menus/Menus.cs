using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is contained by MenuManager
public class Menus : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] string sceneName;

    bool show = false;

    // Start is called before the first frame update
    void Start()
    {
        if(canvas != null)
            canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas == null) return;
        if (Input.GetButton("Cancel")) // Esc
        {
            if(show == false)
            {
                canvas.enabled = true;
                show = true;
            }
            else
            {
                ResumeButton();
            }
            Debug.Log("Menu");
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
        Debug.Log("Save");
    }

    public void SaveNExitButton()
    {
        // Guardamos
        Debug.Log("Save and Exit");
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
