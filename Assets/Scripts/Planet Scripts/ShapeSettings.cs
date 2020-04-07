using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public float planetRadius = 1;

    public NoiseLayer[] noiseLayers; //NoiseSettings noiseSettings;


    [System.Serializable]
    /// <summary>
    /// Class for layered noise settings
    /// </summary>
    public class NoiseLayer
    {
        /// <summary>
        /// Toggle visibility for a single noise layer
        /// </summary>
        public bool enabled = true;
        public bool useFirstLayerAsMask;
        public NoiseSettings noiseSettings;
    }
}
