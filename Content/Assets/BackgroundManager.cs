using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void GenerateBackground()
    {

    }


    public BackgroundLayer[] backgroundLayerParameters;

    private ScrollUV[] quads;

    public GameObject quadPrefab, background;

    [SerializeField]
    private int _layerSize;
    public int layerSize => _layerSize;

    [System.Serializable]
    public class IntMinMax
    {
        public int min = 1;
        public int max = 1;

        public int randomValue
        {
            get
            {
                if (min == max)
                    return min;
                return Mathf.RoundToInt(Random.Range(min, max));
            }
        }

        public IntMinMax(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public void ControlRange()
        {
            if (min > max)
                Debug.LogError("Max cannot be larger than min");
        }
    }

    [System.Serializable]
    public class BackgroundLayer
    {
        public Gradient colorGradient;
        public float density;
        public float parallaxEffect;
        public IntMinMax size;

        public BackgroundLayer(Gradient colorGradient, float density, float parallaxEffect, IntMinMax size)
        {
            this.colorGradient = colorGradient;
            this.density = density;
            this.parallaxEffect = parallaxEffect;
            this.size = size;
        }
    }

    public void SetUpBackground()
    {
        var count = backgroundLayerParameters.Length;
        quads = new ScrollUV[count];

        for (int i = 0; i < count; i++)
        {
            quads[i] = Instantiate(quadPrefab, new Vector3(0, 0, i), transform.rotation, transform).GetComponent<ScrollUV>();
            quads[i].texture = SetUpLayer(backgroundLayerParameters[i]);
            quads[i].parallax = backgroundLayerParameters[i].parallaxEffect;
        }

        Instantiate(background, new Vector3(0, 0, count + 1), transform.rotation, transform);
    }

    public Texture2D SetUpLayer(BackgroundLayer layerParameter)
    {
        var texture = new Texture2D(layerSize, layerSize);
        var textureSize = layerSize * layerSize;
        var starCount = Mathf.RoundToInt(textureSize * layerParameter.density);
        texture.wrapMode = TextureWrapMode.Repeat;

        var emptyColor = new Color(0, 0, 0);
        emptyColor.a = 0;

        for (int x = 0; x < layerSize; x++)
            for (int y = 0; y < layerSize; y++)
                texture.SetPixel(x, y, emptyColor);

        for (int i = 0; i < starCount; i++)
        {
            var randomX = Mathf.RoundToInt(UnityEngine.Random.Range(0f, layerSize));
            var randomY = Mathf.RoundToInt(UnityEngine.Random.Range(0f, layerSize));

            var color = layerParameter.colorGradient.Evaluate(UnityEngine.Random.Range(0f, 1f));
            color.a = 1;

            var starSize = layerParameter.size.randomValue;

            for (int x = 0; x < starSize; x++)
                for (int y = 0; y < starSize; y++)
                    texture.SetPixel(HandleOverflow(randomX + x, layerSize), HandleOverflow(randomY + y, layerSize), color);
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
