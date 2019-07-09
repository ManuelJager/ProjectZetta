using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;

    public Texture2D texture
    {
        set
        {
            var material = new Material(meshRenderer.material);
            material.mainTexture = value;
            material.name += " (custom)";
            meshRenderer.material = material;
        }
    }

    private float _parallax;

    public float parallax
    {
        set
        {
            _parallax = value;
        }
    }

    public float sizeMultiplier
    {
        get
        {
            return transform.localScale.x / _startSize.x;
        }
        set
        {
            var multiplier = value + _parallax;
            transform.localScale = _startSize * multiplier;
        }
    }

    private Vector3 _startSize;

    private void Awake()
    {
        
        _startSize = transform.localScale;
    }

    private void Update()
    {
        var mat = meshRenderer.material;

        Vector2 offset;

        offset.x = transform.position.x / _startSize.x * _parallax;
        offset.y = transform.position.y / _startSize.y * _parallax;

        offset.x %= 1f;
        offset.y %= 1f;

        mat.mainTextureOffset = offset;
    }
}
