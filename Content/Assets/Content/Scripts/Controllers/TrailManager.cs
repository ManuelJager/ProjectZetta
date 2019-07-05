#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _trailRenderer;

    public bool isFiring;

    public Color particleColor
    {
        set
        {
            var col = _trailRenderer.colorOverLifetime;
            col.enabled = true;

            var grad = new Gradient();

            var alphaKeys = col.color.gradient.alphaKeys;
            var colorKeys = new GradientColorKey[alphaKeys.Length];

            for (int i = 0; i < colorKeys.Length; i++)
                colorKeys[i] = new GradientColorKey(value, alphaKeys[i].time);

            grad.SetKeys(colorKeys, alphaKeys);
            col.color = grad;
        }
    }   

    private void Start()
    {
        _trailRenderer.Play();
    }

    void LateUpdate()
    {
        var eot = _trailRenderer.emission;
        eot.enabled = isFiring;
        isFiring = false;
    }
}
