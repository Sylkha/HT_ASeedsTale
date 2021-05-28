using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCollisions : MonoBehaviour
{
    [SerializeField] GameObject prefabImageDialogue;
    GameObject imageDialogue;

    // Start is called before the first frame update
    void Start()
    {
        imageDialogue = Instantiate(prefabImageDialogue, transform);
        imageDialogue.SetActive(false);
    }
    private void Update()
    {
        imageDialogue.transform.LookAt(Camera.main.transform.position, -Vector3.up);
    }
    private void OnTriggerEnter(Collider other)
    {
        imageDialogue.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        imageDialogue.SetActive(false);
    }
}
