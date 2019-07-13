using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelAnimator : MonoBehaviour, IPanelAnimator
{
    [SerializeField]
    private Vector2 _targetPos;
    public Vector2 targetPos => _targetPos;

    [SerializeField]
    private Vector2 _spawnPosOffset;

    private Vector2 _spawnPos;
    public Vector2 spawnPos => _spawnPos;

    public Vector2 canvasPos => _rectTransform.anchoredPosition;

    private RectTransform _rectTransform;

    public Button button;

    public Text text;

    public ColorBlock buttonColorBlock;

    private float colorMultiplier = 1f;

    public void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        text = transform.GetChild(0).GetComponent<Text>();
        buttonColorBlock = button.colors;

        _targetPos = _rectTransform.anchoredPosition;
        _spawnPos = _targetPos + _spawnPosOffset;
    }

    public IEnumerator LerpAlpha(float value)
    {
        colorMultiplier.MixedInterpolate(value, 0.02f, 0.02f);
        button.colors = Extensions.MultiplyColorBlockByAlpha(buttonColorBlock, colorMultiplier);

        yield return Time.deltaTime;

        if (colorMultiplier != value)
            StartCoroutine(LerpAlpha(value));
    }

    public IEnumerator LerpAlphaOfText(float to)
    {
        var color = text.color;
        color.a.MixedInterpolate(to, 0.02f, 0.02f);
        text.color = color;

        yield return Time.deltaTime;
        if (color.a != to)
            StartCoroutine(LerpAlphaOfText(to));
    }

    public IEnumerator LerpPos(UIManager.panelPos panelPos)
    {
        var targetPos = panelPos == UIManager.panelPos.spawn ? _spawnPos : _targetPos;
        var pos = _rectTransform.anchoredPosition;
        pos.MixedInterpolatev2(targetPos, 0.02f, 0.02f);
        _rectTransform.anchoredPosition = pos;

        yield return Time.deltaTime;

        if (pos != targetPos)
            StartCoroutine(LerpPos(panelPos));
    }

    public void SetInteractable(bool value)
    {
        button.interactable = value;
    }
}
