using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    public ShapeSettings settings;
    RigidNoiseFilter[] _rigidNoiseFilters;

    public ShapeGenerator(ShapeSettings settings)
    {
        this.settings = settings;
        _rigidNoiseFilters = new RigidNoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < _rigidNoiseFilters.Length; i++)
        {
            _rigidNoiseFilters[i] = new RigidNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (_rigidNoiseFilters.Length > 0)
        {
            firstLayerValue = _rigidNoiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < _rigidNoiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += _rigidNoiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }

        return pointOnUnitSphere * (settings.radius * (1 + elevation));
    }
}