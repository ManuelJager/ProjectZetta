using UnityEngine;
public interface IThruster : IPowerConsumer
{
    void SetThrusterFlame(bool value, float strength = 0f);
    float thrust { get; set; }
    TrailManager trailManager { get; }
}
