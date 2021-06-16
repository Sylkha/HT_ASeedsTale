// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

/// <summary>
/// Esta clase se encarga de comprobar cu�ndo ha terminado el video de la intro.
/// </summary>
public class VideoCheck : MonoBehaviour
{
    double time;
    double currentTime;

    VideoPlayer vp;

    [SerializeField] string sceneName;

    /// <summary>
    /// Vemos cu�nto dura el video
    /// </summary>
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        time = vp.clip.length;
    }

    /// <summary>
    /// Vamos comprobando si el video ya ha finalizado. 
    /// Por alguna raz�n, no coincide cu�ndo termina el video y cu�nto dura, por ello, le quito cierta diferencia al total.
    /// </summary>
    void Update()
    {
        currentTime = gameObject.GetComponent<VideoPlayer>().time;

        if (currentTime >= time - 0.5f)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
