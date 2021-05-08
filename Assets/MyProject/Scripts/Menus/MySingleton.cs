using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
