using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoCheck : MonoBehaviour
{
    double time;
    double currentTime;

    VideoPlayer vp;

    [SerializeField] string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        time = vp.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = gameObject.GetComponent<VideoPlayer>().time;

        if (currentTime >= time - 0.5f)
        {
            Debug.Log("cambio!");
            SceneManager.LoadScene(sceneName);
        }
    }
}
