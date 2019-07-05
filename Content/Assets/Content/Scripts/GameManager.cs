using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BackgroundLayer[] backgroundLayerParameters;

    public ScrollUV[] quads;

    private static int LayerSize = 2048;

    [System.Serializable]
    public class BackgroundLayer
    {
        public Gradient colorGradient;
        public float density;
        public float parallaxEffect;
        public int size;

        public BackgroundLayer(Gradient colorGradient, float density, float parallaxEffect, int size)
        {
            this.colorGradient = colorGradient;
            this.density = density;
            this.parallaxEffect = parallaxEffect;
            this.size = size;
        }
    }

    private void Awake()
    {
        Instance = this;
        SetUpBackground(backgroundLayerParameters);
    }

    public void SetUpBackground(BackgroundLayer[] layerParameters)
    {
        var count = layerParameters.Length;

        for (int i = 0; i < count; i++)
        {
            quads[i].texture = SetUpLayer(layerParameters[i]);
            quads[i].parallax = layerParameters[i].parallaxEffect;
        }
    }

    public Texture2D SetUpLayer(BackgroundLayer layerParameter)
    {
        var texture      = new Texture2D(LayerSize, LayerSize);
        var textureSize  = LayerSize * LayerSize;
        var starCount    = Mathf.RoundToInt(textureSize * layerParameter.density);
        var starSize     = layerParameter.size;
        texture.wrapMode = TextureWrapMode.Repeat;

        var emptyColor = new Color(0, 0, 0);
        emptyColor.a = 0;

        for (int x = 0; x < LayerSize; x++)
            for (int y = 0; y < LayerSize; y++)
                texture.SetPixel(x, y, emptyColor);

        for (int i = 0; i < starCount; i++)
        {
            var randomX = Mathf.RoundToInt(UnityEngine.Random.Range(0f, LayerSize));
            var randomY = Mathf.RoundToInt(UnityEngine.Random.Range(0f, LayerSize));

            var color = layerParameter.colorGradient.Evaluate(UnityEngine.Random.Range(0f, 1f));
            color.a = 1;

            for (int x = 0; x < starSize; x++)
                for (int y = 0; y < starSize; y++)
                    texture.SetPixel(HandleOverflow(randomX+x, LayerSize), HandleOverflow(randomY+y, LayerSize), color);
        }

        texture.Apply(false, true);

        return texture;
    }

    public int HandleOverflow(int value, int size)
    {
        if (value > size)
            value -= size;
        if (value < size)
            value += size;
        return value;
    }
}
