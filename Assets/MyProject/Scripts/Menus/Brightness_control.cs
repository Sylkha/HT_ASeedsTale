// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// En esta clase controlamos un panel que está siempre activo y lo hacemos más o menos oscuro para imitar que bajamos el brillo.
/// Dejamos las variables guardadas en PlayerPrefs, así cuando cambiemos de escena se queda guardado, e igual si volvemos a abrir el juego.
/// </summary>
public class Brightness_control : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float sliderValue;
    [SerializeField] Image panelBrightness;

    // Start is called before the first frame update
    void Start()
    {
        // Esto sirve para que, cuando volvamos a iniciar el juego, esté guardado. Si no se guardó ningún valor tendrá 0.5f por defecto.
        slider.value = PlayerPrefs.GetFloat("brillo", 0.5f);

        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.g, panelBrightness.color.b, slider.value);
    }

    /// <summary>
    /// En esta función se le pasa el valor del slider y así guardarlo y aplicarlo al panel.
    /// </summary>
    /// <param name="valor"></param>
    public void ChangeSlider(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("brillo", sliderValue);
        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.g, panelBrightness.color.b, slider.value); 
    }
}
