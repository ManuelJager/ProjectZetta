using UnityEngine;
using System.Linq;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    public BackgroundLayer[] backgroundLayerParameters;

    public GameObject quadPrefab, background;

    private ScrollUV[] layerUVs;

    private ScrollUV backgroundUV;

    [SerializeField]
    private int _layerSize;
    public int layerSize => _layerSize;

    public float globalScale
    {
        set
        {
            var count = layerUVs.Length;
            for (int i = 0; i < count; i++)
                layerUVs[i].sizeMultiplier = value;
            backgroundUV.sizeMultiplier = value;
        }
    }

    /// <summary>
    /// class to store a min and max variable 
    /// </summary>
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

    /// <summary>
    /// class to store background layer parameters
    /// </summary>
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
            size.ControlRange();
        }
    }


    public void SetUpBackground()
    {
        var count = backgroundLayerParameters.Length;
        layerUVs = new ScrollUV[count];

        // controls layer star size
        backgroundLayerParameters.ToList().ForEach(param => param.size.ControlRange());

        // loops through all of the layer parameters
        for (int i = 0; i < count; i++)
        {
            // creates layer object inside this hierarchy
            var layer = Instantiate(quadPrefab, new Vector3(0, 0, i), transform.rotation, transform);
            layer.name = $@"Layer {i + 1}";
            layerUVs[i] = layer.GetComponent<ScrollUV>();
            // sets texture of the cutout material and parallax multiplier of the quad
            layerUVs[i].texture = SetUpLayer(backgroundLayerParameters[i]);
            layerUVs[i].parallax = backgroundLayerParameters[i].parallaxEffect;
        }

        // instantiates static background 
        var tempBackground = Instantiate(background, new Vector3(0, 0, count + 1), transform.rotation, transform);
        tempBackground.name = "Background";
        backgroundUV = tempBackground.GetComponent<ScrollUV>();
        backgroundUV.parallax = 0f;
    }

    /// <summary>
    /// Generates a texture based on layer parameters
    /// </summary>
    public Texture2D SetUpLayer(BackgroundLayer layerParameter)
    {
        var texture = new Texture2D(layerSize, layerSize);
        var textureSize = layerSize * layerSize;
        var starCount = Mathf.RoundToInt(textureSize * layerParameter.density);
        texture.wrapMode = TextureWrapMode.Repeat;

        var emptyColor = new Color(0, 0, 0);
        emptyColor.a = 0;

        // makes complete texture transparent
        for (int x = 0; x < layerSize; x++)
            for (int y = 0; y < layerSize; y++)
                texture.SetPixel(x, y, emptyColor);


        // generates stars on texture
        for (int i = 0; i < starCount; i++)
        {
            var randomX = Mathf.RoundToInt(Random.Range(0f, layerSize));
            var randomY = Mathf.RoundToInt(Random.Range(0f, layerSize));

            var color = layerParameter.colorGradient.Evaluate(Random.Range(0f, 1f));
            color.a = 1;

            var starSize = layerParameter.size.randomValue;

            for (int x = 0; x < starSize; x++)
                for (int y = 0; y < starSize; y++)
                    texture.SetPixel(HandleOverflow(randomX + x, layerSize), HandleOverflow(randomY + y, layerSize), color);
        }

        // applies setPixel changes on texture
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

    public void Start() => SetUpBackground();
}
