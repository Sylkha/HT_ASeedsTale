using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brightness_control : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float sliderValue;
    [SerializeField] Image panelBrightness;

    // Start is called before the first frame update
    void Start()
    {
        // Esto sirve para que, cuando volvamos a iniciar el juego, esté guardado. Si no se guardó ningún valor tendrá 0.5f.
        slider.value = PlayerPrefs.GetFloat("brillo", 0.5f);

        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.g, panelBrightness.color.b, slider.value);
    }

    public void ChangeSlider(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("brillo", sliderValue);
        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.g, panelBrightness.color.b, slider.value); 
    }
}
