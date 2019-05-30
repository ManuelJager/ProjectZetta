using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosiveProjectile : IProjectile
{
    float radius { get; set; }
}
