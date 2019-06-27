#pragma warning disable 649
using UnityEngine;
using UnityEngine.UI;
public class UIBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform _bar;
    [SerializeField]
    private Image _barImage;
    [SerializeField]
    private Text _energyUsageText;
    [SerializeField]
    private float _maxVal = 1;
    [SerializeField]
    private float _val = 1;
    [SerializeField]
    private Color _normalColor;
    [SerializeField]
    private Color _maxColor;
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
    public void UIUpdate()
    {
        var val = Mathf.Clamp01(_val / _maxVal);
        var xSize = _bar.localScale.x;
        xSize.MixedInterpolate(val, 0.02f, 0.02f);
        _bar.localScale = new Vector2(xSize, 1);
        _energyUsageText.text = Mathf.RoundToInt(xSize * 100).ToString() + "%";
        if (xSize == 1f)
        {
            _barImage.color = _maxColor;
            _energyUsageText.color = _maxColor;
        }
        else
        {
            _barImage.color = _normalColor;
            _energyUsageText.color = _normalColor;
        }
    }
    public void OnValidate()
    {
        var val = Mathf.Clamp01(_val / _maxVal);
        _bar.localScale = new Vector2(val, 1);
        _energyUsageText.text = Mathf.RoundToInt(val * 100).ToString() + "%";

    }
}
