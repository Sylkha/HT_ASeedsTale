using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is contained by the Impulse Platform
public class JumpImpulse : MonoBehaviour
{
    [SerializeField] private float impulse = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Movement>())
        {
            other.GetComponent<Movement>().Jump(impulse);
            Debug.Log("oye");
        }
    }

}
