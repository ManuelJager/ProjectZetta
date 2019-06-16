using UnityEngine;
public interface IThruster
{
    void SetThrusterFlame(bool value, float strength = 0f);
    float thrust { get; set; }
    TrailRenderer trailRenderer { get; }
}
