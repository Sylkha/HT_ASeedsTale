namespace IL3DN
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Helping class to associate a name with a color
    /// </summary>
    [System.Serializable]
    public class ColorProperty
    {
        public Color color;
        public string name;

        public ColorProperty(Color color, string name)
        {
            this.color = color;
            this.name = name;
        }
    }

    /// <summary>
    /// Helping class to associate multiple colors to the same material
    /// </summary>
    [System.Serializable]
    public class MaterialColors
    {
        public List<ColorProperty> colors;

        public MaterialColors(Material material)
        {
            colors = new List<ColorProperty>();
#if UNITY_EDITOR
            Shader shader = material.shader;
            int nrOfProperties = UnityEditor.ShaderUtil.GetPropertyCount(shader);
            for (int i = 0; i < nrOfProperties; i++)
            {
                if (UnityEditor.ShaderUtil.GetPropertyType(shader, i) == UnityEditor.ShaderUtil.ShaderPropertyType.Color)
                {
                    string propertyName = UnityEditor.ShaderUtil.GetPropertyName(shader, i);
                    Color color = material.GetColor(propertyName);
                    Debug.Log(propertyName + " " + color);
                    colors.Add(new ColorProperty(color, propertyName));
                }
            }
#endif
        }
    }

    /// <summary>
    /// Helping class to display multiple colors for a material
    /// </summary>
    [System.Serializable]
    public class MultipleColorProperties
    {
        public Material meterial;
        public List<MaterialColors> properties;
        public int selectedProperty;
        public int previousProperty;

        public MultipleColorProperties(Material material)
        {
            this.meterial = material;
            properties = new List<MaterialColors>();
            properties.Add(new MaterialColors(material));
        }
    }

    /// <summary>
    /// Displays a list of materials with multiple colors for customization 
    /// </summary>
    [RequireComponent(typeof(IL3DN_ColorController))]
    public class IL3DN_ColorManagerEffects : MonoBehaviour
    {
        float secondsChange;
        public List<MultipleColorProperties> materials = new List<MultipleColorProperties>();

        public void Refresh()
        {
            StartCoroutine(SetColorGradually());
        }

        public void SetMaterialColors(int slot)
        {
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
            Color[,] colors = new Color[materials.Count, 5];
            for (int i = 0; i < materials.Count; i++)
            {
                for (int k = 0; k < materials[i].properties[materials[i].previousProperty].colors.Count; k++)
                {
                    colors[i, k] = (materials[i].properties[materials[i].previousProperty].colors[k].color);
                }
            }

            for (float t = 0.00f; t < 10; t += 0.1f)
            {
                for (int i = 0; i < materials.Count; i++)
                {
                    for (int k = 0; k < materials[i].properties[materials[i].selectedProperty].colors.Count; k++)
                    {
                        //Color materialColor = Color.Lerp(colors[i, k], materials[i].properties[materials[i].selectedProperty].colors[k].color, t / 10);

                        string propertyName = materials[i].properties[materials[i].selectedProperty].colors[k].name;
                        materials[i].meterial.SetColor(propertyName, Color.Lerp(colors[i, k], materials[i].properties[materials[i].selectedProperty].colors[k].color, t));

                       // materials[i].meterial.SetColor(propertyName, materialColor);
                    }
                }
                yield return null;
            }
        }

        public void set_seconds(float s) { secondsChange = s; }

    }
}