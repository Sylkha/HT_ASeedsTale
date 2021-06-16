// Autor: Silvia Osoro
// silwia.o.g@gmail.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase para mostrar el trail del vuelo (aún no implementado)
/// </summary>
namespace Movements.Fly
{
    public class FlyingTrail : MonoBehaviour
    {
        [SerializeField] GameObject trail;
        public void ActivateTrails()
        {
            if(trail != null)
            {
                trail.SetActive(true);
            }
        }

        public void DeactivateTrails()
        {
            if (trail != null)
            {
                trail.SetActive(false);
            }
        }

        public void ClearTrails()
        {
            if(trail != null)
            {
                TrailRenderer[] componentsInChildren = trail.GetComponentsInChildren<TrailRenderer>();
                foreach(TrailRenderer trailRenderer in componentsInChildren)
                {
                    trailRenderer.Clear();
                }
            }
        }
    }
}
