using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendTextures : MonoBehaviour
{
    /// <summary>
    /// All seasons textures
    /// </summary>
    [SerializeField] Texture summer;
    [SerializeField] Texture autumn;
    [SerializeField] Texture winter;
    [SerializeField] Texture spring;

    /// <summary>
    /// Last and current texture
    /// </summary>
    Texture lastText;

    /// <summary>
    /// Next texture
    /// </summary>
    Texture newText;

    // Delegates! SeasonsManager and this script. SeasonsManager will send a message about the next season.


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
