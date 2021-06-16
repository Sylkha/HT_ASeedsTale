// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase se encarga de recibir cuándo comienza el cambio de estación para hacer crecer la mascara que le ocultará o hará aparecer.
/// </summary>
public class ObjSeasonSwitch : MonoBehaviour
{
    [SerializeField] GameObject prefabMask;
    [SerializeField] float growSpeed = 2;
    GameObject mask;
    bool grow = false;

    /// <summary>
    /// Creamos la máscara y nos suscribimos pasa que nos indiquen desde el controlador del cambio de estación cuándo tiene que crecer nuestra máscara.
    /// </summary>
    void Start()
    {
        mask = Instantiate(prefabMask, transform);
        SeasonalSwitch.instance.SubDelegate(set_growTrue);
    }

    /// <summary>
    /// Haremos crecer el objeto en el cambio de estación. El resto del tiempo lo mantendremos a escala 0.
    /// </summary>
    void Update()
    {
        if(grow == true)
        {
            mask.transform.localScale = new Vector3(mask.transform.localScale.x + growSpeed * Time.deltaTime,
                mask.transform.localScale.y + growSpeed * Time.deltaTime, mask.transform.localScale.z + growSpeed * Time.deltaTime);
        }
        else
        {
            mask.transform.localScale = Vector3.zero;
        }
    }

    void set_growTrue(bool b) { grow = b; }
}
