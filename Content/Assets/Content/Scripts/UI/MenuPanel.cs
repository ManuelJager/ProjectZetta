using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class MenuPanel : MonoBehaviour, IPanel
{
    [SerializeField]
    private Vector2 defaultPos;
    [SerializeField]
    private Vector2 spawnPos;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image image;
    [SerializeField]
    private float targetAlpha;
    [SerializeField]
    public Text[] buttonTextComponents;
    [SerializeField]
    public Button[] buttonButtonComponents;

    public bool rawEnabled => gameObject.activeInHierarchy;

    private bool _enabled;
    public bool enabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            if (value == _enabled)
                return;
            if (value)
                Enable();
            else
                Disable();
        }
    }

    private void SetEnabled(bool value) => _enabled = value;

    IEnumerator LerpPos(Vector2 to, float initialDelay = 0f, Action<bool> endAction = null, bool value = false)
    {
        if (initialDelay != 0f)
            yield return new WaitForSeconds(initialDelay);

        var pos = rectTransform.anchoredPosition;
        pos.MixedInterpolatev2(to, 0.04f, 0.02f);
        rectTransform.anchoredPosition = pos;

        yield return Time.deltaTime;
        if (pos != to)
            StartCoroutine(LerpPos(to: to, endAction: endAction, value: value));
        else endAction?.Invoke(value);
    }

    IEnumerator LerpAlpha(float to, float initialDelay = 0f, Action<bool> endAction = null, bool value = false)
    {
        if (initialDelay != 0f)
            yield return initialDelay;

        var color = image.color;
        color.a.MixedInterpolate(to, 0.01f, 0.01f);
        image.color = color;

        yield return Time.deltaTime;
        if (color.a != to)
            StartCoroutine(LerpAlpha(to: to, endAction: endAction, value: value));
        else endAction?.Invoke(value);
    }

    IEnumerator LerpTimeScale(float to)
    {
        var value = Time.timeScale;
        value.MixedInterpolate(to, 0.01f, 0.01f);
        Time.timeScale = value;

        yield return Time.deltaTime;
        if (value != to)
            StartCoroutine(LerpTimeScale(to));
    }

    IEnumerator LerpPosOfText(RectTransform text, Vector2 to)
    {
        var pos = text.anchoredPosition;
        pos.MixedInterpolatev2(to, 0.02f, 0.02f);
        text.anchoredPosition = pos;

        yield return Time.deltaTime;
        if (pos != to)
            StartCoroutine(LerpPosOfText(text, to));
    }

    IEnumerator LerpAlphaOfText(Text text, float to)
    {
        var color = text.color;
        color.a.MixedInterpolate(to, 0.01f, 0.01f);
        text.color = color;

        yield return Time.deltaTime;
        if (color.a != to)
            StartCoroutine(LerpAlphaOfText(text, to));
    }

    IEnumerator LerpTextComponents(float initialDelay, Vector2 relativePos, float targetAlpha, float delayBetweenComponenets, bool orderTopToBottom)
    {
        yield return new WaitForSeconds(initialDelay);

        var list = buttonTextComponents.ToList();

        if (!orderTopToBottom)
            list.Reverse();

        for (int i = 0; i < list.Count; i++)
        {
            var component = list[i];
            StartCoroutine(LerpPosOfText(component.gameObject.GetComponent<RectTransform>(), relativePos));
            StartCoroutine(LerpAlphaOfText(component, targetAlpha));
            yield return new WaitForSeconds(delayBetweenComponenets);
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();

        var action = new Action<bool>(SetEnabled);
        rectTransform.anchoredPosition = spawnPos;
        StartCoroutine(LerpPos(to: defaultPos, endAction: action, value: true));

        var color = image.color;
        color.a = 0f;
        image.color = color;
        StartCoroutine(LerpAlpha(targetAlpha));

        StartCoroutine(LerpTimeScale(0f));

        StartCoroutine(LerpTextComponents(initialDelay: 0.2f, relativePos: new Vector2(-15f, 0f), targetAlpha: 1f, delayBetweenComponenets: 0.02f, orderTopToBottom: true));
    }

    public void Disable()
    {
        _enabled = false;
        StopAllCoroutines();

        var action = new Action<bool>(gameObject.SetActive);
        StartCoroutine(LerpPos(spawnPos, 0.2f, action, false));

        StartCoroutine(LerpAlpha(0f));

        StartCoroutine(LerpTimeScale(1f));

        StartCoroutine(LerpTextComponents(initialDelay: 0.0f, relativePos: new Vector2(-115f, 0f), targetAlpha: 0f, delayBetweenComponenets: 0.02f, orderTopToBottom: false));
    }

    public void ImmideateEnable()
    {
        StopAllCoroutines();
        rectTransform.anchoredPosition = defaultPos;

        var color = image.color;
        color.a = targetAlpha;
        image.color = color;

        gameObject.SetActive(true);
    }

    public void ImmedeateDisable()
    {
        StopAllCoroutines();
        rectTransform.anchoredPosition = spawnPos;

        var color = image.color;
        color.a = 0f;
        image.color = color;

        gameObject.SetActive(false);
    }

    private void SetButtonComponentActive() => buttonButtonComponents.ToList().ForEach(component => component.gameObject.SetActive(true));

    private void SetButtonComponentInactive() => buttonButtonComponents.ToList().ForEach(component => component.gameObject.SetActive(false));
}
