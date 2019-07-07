using System.Collections.Generic;
using UnityEngine;
public static class ShipControllerUitlities
{
    public static void ApplyRB2DForce(Rigidbody2D rb2d, Transform transform, NewThrust newThrust, float multiplier, Vector2Int orientation)
    {
        rb2d.AddRelativeForce(new Vector2(orientation.x, orientation.y) * newThrust.thrustVectors[orientation].thrust * Time.deltaTime);
    }
}
