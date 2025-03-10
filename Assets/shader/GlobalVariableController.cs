using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariableController : MonoBehaviour
{

    public Vector3 WindVector;
    public float LightIntensity;
    public Color ShadeColor;
    // Start is called before the first frame update
    void Start()
    {
        SetGlobalColor(ShadeColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetGlobalColor(Color color)
    {
        Shader.SetGlobalColor("_ShadeColor", color);

    }
    public void SetWind(Vector3 Wind)
    {
        Shader.SetGlobalVector("_WindDirection", Wind);
    }
    public void SetLightIntensity(float value)
    {
        Shader.SetGlobalFloat("_ColorIntensity", value);
    }
}
