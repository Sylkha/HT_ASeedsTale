using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSeasonSwitch : MonoBehaviour
{
    [SerializeField] GameObject prefabMask;
    [SerializeField] float growSpeed = 2;
    GameObject mask;
    bool grow = false;

    // Start is called before the first frame update
    void Start()
    {
        mask = Instantiate(prefabMask, transform);
        SeasonalSwitch.instance.SubDelegate(set_growTrue);
    }

    // Update is called once per frame
    void Update()
    {
        if(grow == true)
        {
            mask.transform.localScale += new Vector3(growSpeed * Time.deltaTime, growSpeed * Time.deltaTime, growSpeed * Time.deltaTime);
        }
        else
        {
            mask.transform.localScale = Vector3.zero;
        }
    }

    void set_growTrue(bool b) { grow = b; }
}
