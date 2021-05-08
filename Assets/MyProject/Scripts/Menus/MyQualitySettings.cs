using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyQualitySettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] int quality;

    // Start is called before the first frame update
    void Start()
    {
        quality = PlayerPrefs.GetInt("qualityNum", 3);
        dropdown.value = quality;
        SetQuality();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("qualityNum", dropdown.value);
        quality = dropdown.value;
    }
}
