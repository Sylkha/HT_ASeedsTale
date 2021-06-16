// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Esta clase recoge las calidades que tenemos guardadas en nuestra Render Pipeline para mostrarlas y que el usuario elija la que necesite.
/// También guardamos estos cambios.
/// </summary>
public class MyQualitySettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] int quality;

    void Start()
    {
        quality = PlayerPrefs.GetInt("qualityNum", 3);
        dropdown.value = quality;
        SetQuality();
    }

    /// <summary>
    /// Selecciona y cambia la calidad
    /// </summary>
    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("qualityNum", dropdown.value);
        quality = dropdown.value;
    }
}
