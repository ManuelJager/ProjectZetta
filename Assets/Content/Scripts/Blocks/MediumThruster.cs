#pragma warning disable 649
using UnityEngine;
public class MediumThruster : MonoBehaviour, IBlock, IThruster, IMultiSizeBlock
{
    [SerializeField]
    private float _thrust;
    public float thrust
    {
        get => _thrust;
        set => _thrust = value;
    }
    [SerializeField]
    private GameObject thruster;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private BlockBaseClass _blockBaseClass;
    public BlockBaseClass blockBaseClass
    {
        get => _blockBaseClass;
        set => _blockBaseClass = value;
    }
    [SerializeField]
    private MultiSizeBlockBaseClass _multiSizeBlockBaseClass;
    public MultiSizeBlockBaseClass multiSizeBlockBaseClass
    {
        get => _multiSizeBlockBaseClass;
        set => _multiSizeBlockBaseClass = value;
    }
    [SerializeField]
    private TrailRenderer _trailRenderer;
    public TrailRenderer trailRenderer => _trailRenderer;

    public void SubtractFromGridAndDestroy()
    {
        Destroy(gameObject);
    }
    public void SetThrusterFlame(bool value, float strength = 0f)
    {
        animator.SetBool("IsFiring", value);
    }
    public void DebugThis()
    {
        Debug.Log("Rood id of " + transform.root.name + " is : " + transform.GetRootGridID());
    }
}
