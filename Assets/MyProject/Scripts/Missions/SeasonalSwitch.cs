using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IL3DN;

// This script is contained by Altar
public enum Seasons { Summer = 1, Autumn, Winter, Spring };
[RequireComponent(typeof(Yarn.Unity.Example.NPC))]
public class SeasonalSwitch : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(seasonNum - 1);
        cmTex.set_seconds(secondsChange);
        cmEff.set_seconds(secondsChange);

        dialogue = GetComponent<Yarn.Unity.Example.NPC>();
        dialogue.talkToNode = n_NotCompletedMission;

        season = Seasons.Summer;
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
        
        SeasonProcesses(seasonNum, newVisibleMask);       
    }

    public void FinishChange()
    {
        // Cuando terminamos el cambio de estación, cambiamos de layer la new a la visible, la visible a la invisible y desactivamos las bolitas que recubren nuestros objetos.
        SeasonProcesses((int)season, invisibleMask);
        season = newSeason;
        SeasonProcesses((int)season, visibleMask);
        Debug.Log("pasa?");
    }

    void SeasonProcesses(int sNum, LayerMask mask)
    {
        cmTex.SetMaterialColors(sNum);
        cmEff.SetMaterialColors(sNum);
        seasons[sNum - 1].mask = mask;
        ChangeLayers(seasons[sNum - 1].objParent, mask);

        
    }

    void ChangeLayers(GameObject seasonObjParent, LayerMask newLayer)
    {
        for(int i = 0; i < seasonObjParent.transform.childCount; i++)
        {
            seasonObjParent.transform.GetChild(i).gameObject.layer = newLayer;
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
        for (float t = 0.01f; t < 10; t += 0.1f)
        {
            snow.SnowTerrain = Mathf.Lerp(0, SnowTerrain, t / 10);
            snow.SnowPines = Mathf.Lerp(0, SnowPines, t / 10);
            snow.SnowLeaves = Mathf.Lerp(0, SnowLeaves, t / 10);
            snow.SnowBranches = Mathf.Lerp(0, SnowBranches, t / 10);
            snow.SnowRocks = Mathf.Lerp(0, SnowRocks, t / 10);
            snow.SnowGrass = Mathf.Lerp(0, SnowGrass, t / 10);
            snow.CutoffLeaves = Mathf.Lerp(0, CutoffLeaves, t / 10);

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

}
