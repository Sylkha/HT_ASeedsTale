// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// En esta clase controlamos un panel que est� siempre activo y lo hacemos m�s o menos oscuro para imitar que bajamos el brillo.
/// Dejamos las variables guardadas en PlayerPrefs, as� cuando cambiemos de escena se queda guardado, e igual si volvemos a abrir el juego.
/// </summary>
public class Brightness_control : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float sliderValue;
    [SerializeField] Image panelBrightness;

    // Start is called before the first frame update
    void Start()
    {
        // Esto sirve para que, cuando volvamos a iniciar el juego, est� guardado. Si no se guard� ning�n valor tendr� 0.5f por defecto.
        slider.value = PlayerPrefs.GetFloat("brillo", 0.5f);

        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.g, panelBrightness.color.b, slider.value);
    }

    /// <summary>
    /// En esta funci�n se le pasa el valor del slider y as� guardarlo y aplicarlo al panel.
    /// </summary>
    /// <param name="valor"></param>
    public void ChangeSlider(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("brillo", sliderValue);
        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.g, panelBrightness.color.b, slider.value); 
    }
}
