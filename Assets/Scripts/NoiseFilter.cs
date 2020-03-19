using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
    NoiseSettings settings;
    Noise noise = new Noise();

    public NoiseFilter(NoiseSettings settings)
    {
        this.settings = settings;
    }

    // Point is where we evaluate and apply processing
    public float Evaluate(Vector3 point)
    {
        // noise.Evaluate(point) is in [-1, 1] so ad 1 for [0, 1]
        float noiseValue = 0;                                           // (noise.Evaluate(point * settings.roughness + settings.center) + 1) * 0.5f;
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        // Cycle through the layers
        for (int i = 0; i < settings.numLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }
        // Clamp value of noice to planet surface if it dips into planet
        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return noiseValue * settings.strength;
    }
}
