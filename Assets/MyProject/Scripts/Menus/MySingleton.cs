// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simplemente un Singleton padre en el cual pondremos todo aquello que queramos llevarnos de entre escenas.
/// </summary>
public class MySingleton : MonoBehaviour
{
    #region Singleton management
    static MySingleton instance;

    static public MySingleton Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //Este gameObject persiste entre escenas
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    #endregion 
}
