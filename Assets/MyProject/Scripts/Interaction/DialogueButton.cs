// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Esta clase nos ayuda a que, cuando estemos en un diálogo, nos seleccione un botón en concreto del diálogo
/// para los jugadores con mando o que quieran usar solo el teclado.
/// Esta clase la contiene el Dialogue, padre de los botones de diálogo.
/// </summary>
public class DialogueButton : MonoBehaviour
{
    /// <summary>
    /// Botón de continuar de Yarn Spinner
    /// </summary>
    [SerializeField] GameObject continueButton;

    /// <summary>
    /// Botón de opción 1 de Yarn Spinner
    /// </summary>
    [SerializeField] GameObject option1Button;

    /// <summary>
    /// El selected es para que podamos elegir otra opción, y no esté todo el rato en la primera opción, tan solo en el principio, 
    /// ya que le decimos que en cuanto esté activa la primera opción, que la seleccione.
    /// </summary>
    bool selected = false;

    /// <summary>
    /// Ya que el código de YarnSpinner no dejaba ver bien qué cogía en cada momento (tiene comandos externos también),
    /// pensé en cuándo necesitaba que esos botones de diálogo apareciesen seleccionados: cuando están activos.
    /// </summary>
    void Update()
    {
        if (PlayerCollisions.instance.is_chating())
        {
            // Botón de continuar.
            if(continueButton.activeSelf == true)
            {
                // limpiamos el objeto seleccionado
                EventSystem.current.SetSelectedGameObject(null);
                // le metemos el objeto que queremos seleccionar
                EventSystem.current.SetSelectedGameObject(continueButton);
            }

            // Botón de opción
            if (option1Button.activeSelf == true && selected == false)
            {
                // limpiamos el objeto seleccionado
                EventSystem.current.SetSelectedGameObject(null);
                // le metemos el objeto que queremos seleccionar
                EventSystem.current.SetSelectedGameObject(option1Button);
                selected = true;
            }

            // Cuando esté desactivado el botón de opción tras seleccionarlo
            else if(option1Button.activeSelf == false && selected == true)
            {
                selected = false;
            }
        }
    }
}

