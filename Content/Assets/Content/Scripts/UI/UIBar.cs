#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
public class UIBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform _bar;
    [SerializeField]
    private Text _energyUsageText;
    [SerializeField]
    private float _maxVal = 1;
    [SerializeField]
    private float _val = 1;
    public float val
    {
        set
        {
            if (value.IsPositive())
            {
                if (value > _maxVal)
                {
                    Debug.LogWarning("Bar component value of " + gameObject.name + " should not be higher than the max value");
                    _val = _maxVal;
                }
                else
                    _val = value;
            }
            else
                Debug.LogWarning("val value must be positive");
        }
    }
    public float maxVal
    {
        set
        {
            if (value.IsPositive())
            {
                _maxVal = value;
            }
            else
                Debug.LogWarning("maxVal value must be positive");
        }
    }
    public void Update()
    {
        var val = Mathf.Clamp01(_val / _maxVal);
        var xSize = _bar.localScale.x;
        xSize.MixedInterpolate(val, 0.01f, 0.005f);
        _bar.localScale = new Vector2(xSize, 1);
        _energyUsageText.text = ((-val * 100) + 100).ToString() + "%";
    }
}
