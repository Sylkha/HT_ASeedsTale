namespace IL3DN
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Helping class to associate a color with a material
    /// </summary>
    [System.Serializable]
    public class ShaderProperties
    {
        public Color color;
        public Texture2D mainTex;
        public ShaderProperties(Color color, Texture2D mainTex)
        {
            this.color = color;
            this.mainTex = mainTex;
        }
    }

    /// <summary>
    /// Helping class to display multiple set of properties on the same material
    /// </summary>
    [System.Serializable]
    public class MaterialProperties
    {
        public Material meterial;
        public List<ShaderProperties> properties;
        public int selectedProperty;
        public int previousProperty;

        public MaterialProperties(Material material)
        {
            this.meterial = material;
            properties = new List<ShaderProperties>();
        }
    }

    /// <summary>
    /// Display a list of materials with a single color to customize
    /// </summary>
    [RequireComponent(typeof(IL3DN_ColorController))]
    public class IL3DN_ColorManagerTextures : MonoBehaviour
    {
        bool finished = false;
        float secondsChange;
        public List<MaterialProperties> materials = new List<MaterialProperties>();

        public void Refresh()
        {
            StartCoroutine(SetColorGradually());                      
        }

        public void SetMaterialColors(int slot)
        {
            finished = false;
            for (int i = 0; i < materials.Count; i++)
            {
                if (materials[i].properties.Count > slot - 1)
                {
                    materials[i].previousProperty = materials[i].selectedProperty;
                    materials[i].selectedProperty = slot - 1;
                }
            }
            Refresh();
        }

        private IEnumerator SetColorGradually()
        {
            Color[] colors = new Color[materials.Count];
            for (int i = 0; i < materials.Count; i++)
            {
                colors[i] = materials[i].properties[materials[i].previousProperty].color;
            }

            for (float t = 0.00f; t < secondsChange / 10; t += Time.deltaTime)
            {
                for (int i = 0; i < materials.Count; i++)
                {
                    materials[i].meterial.color = Color.Lerp(colors[i], materials[i].properties[materials[i].selectedProperty].color, t);
                    // materials[i].meterial.mainTexture = materials[i].properties[materials[i].selectedProperty].mainTex;
                }
                yield return null;
            }
            finished = true;
        }

        public bool get_finished() { return finished; }

        public void set_finished(bool b) { finished = b; }
        public void set_seconds(float s) { secondsChange = s; }
    }
}
