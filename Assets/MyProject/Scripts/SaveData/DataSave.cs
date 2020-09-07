using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[Serializable]
public class DataSave: MonoBehaviour
{
    // Todo int y ya lo juntaremos 
    public float x_position;
    public float y_position;
    public float z_position;
    
    public float x_rotation;
    public float y_rotation;
    public float z_rotation;

    public bool ship;

    public void save_Variables(Transform _player, InMemoryVariableStorage _memoryVariable)
    {
        x_position = _player.position.x;

        x_rotation = _player.rotation.eulerAngles.x; // comprobar si con eulerAngles o sin eulerAngles

        ship = _memoryVariable.GetValue("$ship_something").AsBool;
        
        Debug.Log(x_position);
    }

    public void load_Variables(Transform _player, InMemoryVariableStorage _memoryVariable)
    {
        _player.GetComponent<CharacterController>().Move(-_player.transform.position + new Vector3(x_position, y_position, z_position));
        _player.rotation = Quaternion.Euler(x_rotation, y_rotation, z_rotation);

        _memoryVariable.SetValue("$ship_something", ship);

        //Debug.Log(_player.position);
        Debug.Log(_memoryVariable.GetValue("$ship_something"));
    }

}
