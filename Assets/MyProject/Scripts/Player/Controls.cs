using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bindings;
public class Controls : MonoBehaviour
{
    public static Controls instance;

    MyPlayerActions actions;
    string saveActionsData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    #region InControl
    void OnEnable()
    {
        // See PlayerActions.cs for this setup.
        actions = MyPlayerActions.CreateWithDefaultBindings();
        //playerActions.Move.OnLastInputTypeChanged += ( lastInputType ) => Debug.Log( lastInputType );

        LoadBindings();
    }
    void OnDisable()
    {
        // This properly disposes of the action set and unsubscribes it from
        // update events so that it doesn't do additional processing unnecessarily.
        actions.Destroy();
    }
    void LoadBindings()
    {
        if (PlayerPrefs.HasKey("Bindings"))
        {
            saveActionsData = PlayerPrefs.GetString("Bindings");
            actions.Load(saveActionsData);
        }
    }
    void SaveBindings()
    {
        saveActionsData = actions.Save();
        PlayerPrefs.SetString("Bindings", saveActionsData);
    }
    #endregion InControl

    public MyPlayerActions get_actions() { return actions; }
}
