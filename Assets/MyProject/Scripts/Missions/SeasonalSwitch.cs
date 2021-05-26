using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IL3DN;

/// <summary>
/// Creamos un delegado. Podemos hacer uno para el cambio de estación (más elegante)
/// </summary>
public delegate void Delegate(bool b);

// This script is contained by Altar
public enum Seasons { Summer = 1, Autumn, Winter, Spring };
[RequireComponent(typeof(Yarn.Unity.Example.NPC))]
public class SeasonalSwitch : MonoBehaviour
{
    public static SeasonalSwitch instance;

    [Header("Mission Part")]
    [SerializeField] MissionNotes mn;
    [SerializeField] string n_CompletedMission;
    [SerializeField] string n_NotCompletedMission;

    Yarn.Unity.Example.NPC dialogue;
    bool canSwitch = false;

    [System.Serializable]
    public struct SeasonStruct
    {
        public GameObject objParent;
        public Seasons season;
        [HideInInspector]
        public LayerMask mask;
    }
    [Header("Seasons")]
    [SerializeField] SeasonStruct [] seasons;

    [SerializeField] IL3DN_ColorManagerTextures cmTex;
    [SerializeField] IL3DN_ColorManagerEffects cmEff;
    [SerializeField] IL3DN_Snow snow;

    Seasons season;
    Seasons newSeason;

    int seasonNum = 1;

    /// <summary>
    /// Layers.
    /// VisibleMask es la máscara actual, se verá durante toda la estación.
    /// InvisibleMask es la máscara en la cual estarán el resto de objetos de las demás estaciones que no sean la actual.
    /// NewVisibleMask es la máscara en la cual mientras se esté haciendo el cambio, la visible se irá haciendo invisible por el stencil y la newVisible será la que se está haciendo visible.
    /// Cuando se retiren los stencils es cuando cambiamos los objetos de la visible a la invisible, y los de newVisible a la visible.
    /// </summary>
    [SerializeField] LayerMask visibleMask;
    [SerializeField] LayerMask invisibleMask;
    [SerializeField] LayerMask newInvisibleMask;
    [SerializeField] LayerMask newVisibleMask;

    [Header("Snow Variables")]
    [SerializeField] [Range(0, 1)] float SnowTerrain;
    [SerializeField] [Range(0, 20)] float SnowPines;
    [SerializeField] [Range(0, 20)] float SnowLeaves;
    [SerializeField] [Range(0, 20)] float SnowBranches;
    [SerializeField] [Range(0, 20)] float SnowRocks;
    [SerializeField] [Range(0, 20)] float SnowGrass;
    [SerializeField] [Range(0, 2.1f)] float CutoffLeaves;

    [SerializeField] float snowAppSpeed = 2;

    [SerializeField] float secondsChange = 4f;

    Delegate delegado;

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

    // Start is called before the first frame update
    void Start()
    {       
        cmTex.set_seconds(secondsChange);
        cmEff.set_seconds(secondsChange);

        dialogue = GetComponent<Yarn.Unity.Example.NPC>();
        dialogue.talkToNode = n_NotCompletedMission;

        season = Seasons.Summer;
        ChangeLayers(seasons[(int)season - 1].objParent, visibleMask);
    }

    private void Update()
    {
        if(cmTex.get_finished() == true)
        {
            FinishChange();
            cmTex.set_finished(false);
        }
    }

    public void ChangeSeason()
    {
        seasonNum += 2;
        if (seasonNum > 4)
            seasonNum = 1;

        newSeason = (Seasons)seasonNum;

        if (season == Seasons.Winter)
        {
            Snow(false);
        }

        switch ((Seasons)seasonNum)
        {
            case Seasons.Summer:
                Debug.Log("Summer");
                break;
            case Seasons.Autumn:
                Debug.Log("Autumn");
                break;
            case Seasons.Winter:
                Debug.Log("Winter");
                Snow(true);
                break;
            case Seasons.Spring:
                Debug.Log("Spring");                               
                break;
        }

        SeasonStart();
        SeasonProcesses((int)newSeason, newVisibleMask);
        SeasonProcesses((int)season, newInvisibleMask);
    }

    public void FinishChange()
    {
        // Cuando terminamos el cambio de estación, cambiamos de layer la new a la visible, la visible a la invisible y desactivamos las bolitas que recubren nuestros objetos.
        SeasonProcesses((int)season, invisibleMask);
        season = newSeason;
        SeasonProcesses((int)season, visibleMask);
        delegado(false);
        Debug.Log("pasa?");
    }
    void SeasonStart()
    {
        delegado(true);
        cmTex.SetMaterialColors((int)newSeason);
        cmEff.SetMaterialColors((int)newSeason);
    }

    void SeasonProcesses(int sNum, LayerMask mask)
    {        
        //seasons[sNum - 1].mask = mask;
        ChangeLayers(seasons[sNum - 1].objParent, mask);        
    }

    void ChangeLayers(GameObject seasonObjParent, LayerMask newLayer)
    {
        // Ya que las layers son códigos de bits, el primer valor sería 1, el siguiente 2, 4, 8, 16... y así,
        // vamos a contar cuántas veces hay que dividir entre 2 para obtener el número de la layer 
        int tempLayer = newLayer;
        int layerNumber = 0;

        do
        {
            layerNumber++;
            tempLayer /= 2;
        } while (tempLayer > 0);
        layerNumber--; // las layers empiezan desde el 0

        for(int i = 0; i < seasonObjParent.transform.childCount; i++)
        {
            seasonObjParent.transform.GetChild(i).gameObject.layer = layerNumber;
        }
    }

    void Snow(bool grow)
    {
        // Crece la nieve
        if (grow)
        {
            snow.SnowTerrain = 0;
            snow.SnowPines = 0;
            snow.SnowLeaves = 0;
            snow.SnowBranches = 0;
            snow.SnowRocks = 0;
            snow.SnowGrass = 0;
            snow.CutoffLeaves = 0;
            snow.Snow = true;

            StartCoroutine(snowGrow());
        }
        // Va desapareciendo la nieve
        else
        {
            StartCoroutine(snowDecrese());
        }
    }

    IEnumerator snowGrow()
    {
        Debug.Log("Nieve parribaa");
        for (float t = 0.0f; t < secondsChange / 10; t += Time.deltaTime)
        {
            snow.SnowTerrain = Mathf.Lerp(0, SnowTerrain, t);
            snow.SnowPines = Mathf.Lerp(0, SnowPines, t);
            snow.SnowLeaves = Mathf.Lerp(0, SnowLeaves, t);
            snow.SnowBranches = Mathf.Lerp(0, SnowBranches, t);
            snow.SnowRocks = Mathf.Lerp(0, SnowRocks, t);
            snow.SnowGrass = Mathf.Lerp(0, SnowGrass, t);
            snow.CutoffLeaves = Mathf.Lerp(0, CutoffLeaves, t);

            yield return null;
        }                  
    }

    IEnumerator snowDecrese()
    {
        Debug.Log("Nieve fueraa");
        for (float t = 0.00f; t < secondsChange / 10; t += Time.deltaTime)
        {
            snow.SnowTerrain = Mathf.Lerp(snow.SnowTerrain, 0, t);
            snow.SnowPines = Mathf.Lerp(snow.SnowPines, 0, t);
            snow.SnowLeaves = Mathf.Lerp(snow.SnowLeaves, 0, t);
            snow.SnowBranches = Mathf.Lerp(snow.SnowBranches, 0, t );
            snow.SnowRocks = Mathf.Lerp(snow.SnowRocks, 0, t);
            snow.SnowGrass = Mathf.Lerp(snow.SnowGrass, 0, t);
            snow.CutoffLeaves = Mathf.Lerp(snow.CutoffLeaves, 0, t);

            yield return null;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerCollisions>())
        {
            int missionsCompleted = 0;
            for(int i = 0; i < mn.Get_Notes().Length; i++)
            {
                if (mn.Get_Notes()[i].ourObject.GetCollected())
                    missionsCompleted++;
            }
            if(missionsCompleted == mn.Get_Notes().Length)  //Todas están completadas.
            {
                dialogue.talkToNode = n_CompletedMission;
                canSwitch = true;
                // Activamos el cambio de estación desde el dialogo
            }
        }
    }

    /// <summary>
    /// Para suscribirse
    /// </summary>
    /// <param name="d"></param>
    public void SubDelegate(Delegate d)
    {
        delegado += d;
    }

    /// <summary>
    /// Desuscribirse
    /// </summary>
    /// <param name="d"></param>
    public void DesubDelegate(Delegate d)
    {
        if (delegado == null) return;
        delegado -= d;
    }

}
