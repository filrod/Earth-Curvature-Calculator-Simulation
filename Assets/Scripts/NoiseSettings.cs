using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    // Strength of the noise
    public float strength = 1;

    /// <summary> Number of Noise layers </summary>
    [Tooltip("Number of noise layers")] [Range(1, 8)] public int numLayers = 1;
    /// <summary> Base roughness of noise controls intensity of curvature for the base layer </summary>
    [Tooltip("Base roughness of noise controls intensity of curvature for the base layer")] public float baseRoughness = 1;
    /// <summary> Roughness of noise controls intensity of curvature </summary>
    [Tooltip("Roughness of noise controls intensity of curvature")] public float roughness = 2;
    /// <summary> Persistance changes the amplitude of roughness by layer </summary>
    [Tooltip(" Persistance changes the amplitude of roughness by layer")] public float persistence = 0.5f;
    // Centre of noise
    public Vector3 center;

    /// <summary> Responsible for mountain roughness by clamping the values of troughs to the planet's surface. </summary>
    [Tooltip("Responsible for mountain roughness by clamping the values of troughs to the planet's surface.")] public float minValue;

}
