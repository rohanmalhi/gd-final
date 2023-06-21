using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sunLight;
    public float cycleDuration = 60.0f;
    public float intensityMultiplier = 1.0f;

    private void Update()
    {
        // Calculate the time factor based on the cycle duration
        float timeFactor = 2.0f * Mathf.PI / cycleDuration;

        // Calculate the intensity based on a sine function
        float intensity = Mathf.Sin(timeFactor * Time.time) * 0.5f + 0.5f;

        // Apply intensity multiplier
        intensity *= intensityMultiplier;

        // Update the light intensity
        sunLight.intensity = intensity;
    }
}
