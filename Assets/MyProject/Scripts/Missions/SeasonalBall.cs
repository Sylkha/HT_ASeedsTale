using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonalBall : MonoBehaviour
{
    SphereCollider col;

    float rad_initial = 0;
    [SerializeField] float rad_final = 5;

    LayerMask season_hide;
    LayerMask season_show;

    bool doIt = false;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        col.radius = rad_initial;

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (doIt == false) return;       
        SeasonSwitch();        
    }

    void SeasonSwitch()
    {
        this.gameObject.SetActive(true);

        col.radius = Mathf.Lerp(rad_initial, rad_final, 10f);        
    }

    public void setLayers(LayerMask _season_hide, LayerMask _season_show)
    {
        this.gameObject.SetActive(true);

        season_hide = _season_hide;
        season_show = _season_show;

        doIt = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(other.gameObject.layer, season_show))
        {
            other.gameObject.SetActive(true);
            Debug.Log("aparece");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsInLayerMask(other.gameObject.layer, season_hide))
        {
            other.gameObject.SetActive(false);
            Debug.Log("ocultao");
        }
    }

    // Esto luego lo llevamos a un script con más statics.
    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }
}
