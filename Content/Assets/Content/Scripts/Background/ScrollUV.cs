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

    private void Update()
    {
        Material mat = meshRenderer.material;

        Vector2 offset;

        offset.x = transform.position.x / transform.localScale.x * _parallax;
        offset.y = transform.position.y / transform.localScale.y * _parallax;

        mat.mainTextureOffset = offset;
    }
}
