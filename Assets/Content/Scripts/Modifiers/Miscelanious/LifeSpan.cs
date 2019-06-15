using UnityEngine;
//attached to any gameobject that should have a defined linespan in seconds
public class LifeSpan : MonoBehaviour
{
    [SerializeField]
    private float life;
    void Update()
    {
        life -= Time.deltaTime;
        if (life < 0f)
            gameObject.Destroy();
    }
}
