using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPanelAnimator
{
    IEnumerator LerpPos(UIManager.panelPos to);
    IEnumerator LerpAlpha(float value);
    IEnumerator LerpAlphaOfText(float value);
    Vector2 targetPos { get; }
    Vector2 spawnPos { get; }
    Vector2 canvasPos { get; }
    void SetInteractable(bool value);
}
