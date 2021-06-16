// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Desde esta clase, apretando el Toggle del menú, activamos o desactivamos la opción de pantalla completa.
/// También guardaremos esta decisión.
/// </summary>
public class FullScreen : MonoBehaviour
{
    [SerializeField] Toggle toggle;

    [SerializeField] TMP_Dropdown resolDropDown;
    Resolution[] resolutions;

    void Start()
    {
        toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("fullscreen", 1));
        if (toggle.isOn == true)
        {
            Screen.fullScreen = true;            
        }
        else
        {
            Screen.fullScreen = false;
        }

        CheckResolution();
    }

    /// <summary>
    /// Activamos o desactivamos pantalla completa desde el toggle
    /// </summary>
    /// <param name="fullScreen"></param>
    public void ActivateFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        PlayerPrefs.SetInt("fullscreen", Convert.ToInt32(fullScreen));
    }

    /// <summary>
    /// Cogemos todas las resoluciones de nuestra pantalla, eliminamos las que tenía el Dropdown y le ponemos las nuestras
    /// </summary>
    void CheckResolution()
    {        
        resolutions = Screen.resolutions;
        resolDropDown.ClearOptions();
        List<string> options = new List<string>();
        int currentRes = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // Para poner el tamaño actual de nuestra pantalla
            if(Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
            }
        }

        // Agrega las opciones guardadas en el DropDown, pone la resolución actual y actualiza la lista guardada
        resolDropDown.AddOptions(options);
        resolDropDown.value = currentRes;
        resolDropDown.RefreshShownValue();

        resolDropDown.value = PlayerPrefs.GetInt("resolutionNum", currentRes);
    }

    /// <summary>
    /// Cambiamos de resolución desde el dropdown
    /// </summary>
    /// <param name="idResolution"></param>
    public void ChangeResolution(int idResolution)
    {
        PlayerPrefs.SetInt("resolutionNum", resolDropDown.value);

        Resolution resolution = resolutions[idResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
