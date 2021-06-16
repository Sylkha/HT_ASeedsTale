// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Esta clase nos ayuda a que, cuando estemos en un di�logo, nos seleccione un bot�n en concreto del di�logo
/// para los jugadores con mando o que quieran usar solo el teclado.
/// Esta clase la contiene el Dialogue, padre de los botones de di�logo.
/// </summary>
public class DialogueButton : MonoBehaviour
{
    /// <summary>
    /// Bot�n de continuar de Yarn Spinner
    /// </summary>
    [SerializeField] GameObject continueButton;

    /// <summary>
    /// Bot�n de opci�n 1 de Yarn Spinner
    /// </summary>
    [SerializeField] GameObject option1Button;

    /// <summary>
    /// El selected es para que podamos elegir otra opci�n, y no est� todo el rato en la primera opci�n, tan solo en el principio, 
    /// ya que le decimos que en cuanto est� activa la primera opci�n, que la seleccione.
    /// </summary>
    bool selected = false;

    /// <summary>
    /// Ya que el c�digo de YarnSpinner no dejaba ver bien qu� cog�a en cada momento (tiene comandos externos tambi�n),
    /// pens� en cu�ndo necesitaba que esos botones de di�logo apareciesen seleccionados: cuando est�n activos.
    /// </summary>
    void Update()
    {
        if (PlayerCollisions.instance.is_chating())
        {
            // Bot�n de continuar.
            if(continueButton.activeSelf == true)
            {
                // limpiamos el objeto seleccionado
                EventSystem.current.SetSelectedGameObject(null);
                // le metemos el objeto que queremos seleccionar
                EventSystem.current.SetSelectedGameObject(continueButton);
            }

            // Bot�n de opci�n
            if (option1Button.activeSelf == true && selected == false)
            {
                // limpiamos el objeto seleccionado
                EventSystem.current.SetSelectedGameObject(null);
                // le metemos el objeto que queremos seleccionar
                EventSystem.current.SetSelectedGameObject(option1Button);
                selected = true;
            }

            // Cuando est� desactivado el bot�n de opci�n tras seleccionarlo
            else if(option1Button.activeSelf == false && selected == true)
            {
                selected = false;
            }
        }
    }
}

