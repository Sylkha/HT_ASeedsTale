using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonsManager : MonoBehaviour
{
    #region Variables
    [SerializeField] SeasonalBall sb;
    [SerializeField] Camera camera_ref;

    [SerializeField] LayerMask everything;
    /// <summary>
    /// Character's layer
    /// </summary>
    [SerializeField] LayerMask character;

    /// <summary>
    /// Each season layer
    /// </summary>
    [SerializeField] LayerMask summer;
    [SerializeField] LayerMask winter;
    [SerializeField] LayerMask autumn;
    [SerializeField] LayerMask spring;

    /// <summary>
    /// This array contains each season layer. Sort in a real way (summer - autumn - winter - spring)
    /// </summary>
    LayerMask[] seasonsLayers;
    LayerMask[] hideLayers = new LayerMask [2];

    /// <summary>
    /// This variable is for last and current season
    /// </summary>
    LayerMask lastSeason;

    /// <summary>
    /// This variable is for the next season
    /// </summary>
    LayerMask newSeason;

    /// <summary>
    /// Variable to keep track of the seasons we've been throu. 
    /// After being in each one, we'll change it, so instead of going to the next season, the player will choose the next season
    /// </summary>
    byte count = 0;

    #endregion Variables

    void Start()
    {
        seasonsLayers = new LayerMask [] { summer, autumn, winter, spring};
        lastSeason = seasonsLayers[0];
        camera_ref.cullingMask = everything - winter - autumn - spring; 
        // Physics.IgnoreLayerCollision(character, lastSeason, false);       
    }

    void Update()
    {
        // Cuando el count sea menos que 4, enseñamos el next season, si no enseñamos solo lo de las demás estaciones
    }

    /// <summary>
    /// When count is < seasonsLayers, this function will be called when we finish talking with the tree after we complete missions
    /// When count is >= seasonsLayers, this function will be called in SelectSeason
    /// </summary>
    public void SwitchSeasons()
    {
        // While we're in the first stage (season non eligible) we'll now the next season, if not, next season will be chose in SelectSeason
        if(count < seasonsLayers.Length)
        {
            count++;
            newSeason = seasonsLayers[count];
        }
         
        // We wont render the other 2 seasons. We only need the last and new seasons.
        for(int i = 0, j = 0; i < seasonsLayers.Length; i++)
        {
            if(seasonsLayers[i] != newSeason && seasonsLayers[i] != lastSeason)
            {
                hideLayers[j] = seasonsLayers[i];
                j++;
            }
        }
        // Render Everything but the 2 seasons mentioned.
        camera_ref.cullingMask = everything - hideLayers[0] - hideLayers[1];
        Debug.Log(LayerMask.NameToLayer("Summer"));
        
        sb.setLayers(lastSeason.value, newSeason.value);

        // Last season is the current season.
        lastSeason = newSeason;
    }

    // Esto lo tendrán unos botones.
    public void SelectSeason(string season)
    {
        switch (season)
        {
            case "summer": newSeason = seasonsLayers[0];
                    break;

            case "autumn": newSeason = seasonsLayers[1];
                Debug.Log("autumn");
                break;

            case "winter": newSeason = seasonsLayers[2];
                break;

            case "spring": newSeason = seasonsLayers[3];
                break;

            default: Debug.Log("Asegurate de que hayas puesto el nombre de la season bien");
                break;
        }

        if (newSeason != lastSeason)
        {
            SwitchSeasons();
            // Debug.Log("distinta season, vamos a cambiar de estación.");
        }
        else;
            //Debug.Log("estas accediendo a la misma estación a la que estabas");
    }
}
