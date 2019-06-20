using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ara;

public class TrailManager : MonoBehaviour
{
    [SerializeField]
    private AraTrail _trailRenderer;
    [SerializeField]
    private float maxThickness;
    [SerializeField]
    private float slopeMultiplier;
    [SerializeField]
    private float slopeStep;

    private float currentThickness;
    public bool isFiring;

    void Update() =>
        _trailRenderer.initialThickness = currentThickness;

    void LateUpdate()
    {
        _trailRenderer.emit = isFiring;
        if (isFiring)
            currentThickness.MixedInterpolate(maxThickness, slopeMultiplier, slopeStep);
        else
            currentThickness = 0;
        isFiring = false;
    }
}
