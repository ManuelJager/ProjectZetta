using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dampening
{
    static Vector2 relativeVelocity = new Vector2();
    static ShipController shipController;
    static Dampening()
    {
        shipController = GameObject.FindObjectOfType<ShipController>();
    }
    public static void Dampen(Vector2 input, Rigidbody2D rb2d, float zAngle, GameObject target, ShipGrid.Thrust thrust)
    {
        var absoluteVelocity = rb2d.velocity;
        relativeVelocity = RotationUtilities.RotateVector2(absoluteVelocity, zAngle);
        //if there is not horizontal but there is vertical input, then we will dampen the ship horizontally
        if (input.x == 0 && input.y == 0)
        {
            DampenVertically();
            DampenHorizontally();
        }
        else if (input.x == 0)
        {
            DampenVertically();
        }
        //if there is not horizontal but there is vertical input, then we will dampen the ship vertically
        else if (input.y == 0)
        {
            
            DampenHorizontally();
        }
        else
        {
        }
        Common.ClearLog();
        Debug.Log("Relative vel is : " + relativeVelocity);
        Debug.Log("Absolute vel is : " + rb2d.velocity);
    }

    public static void DampenVertically()
    {
        if (relativeVelocity.y < -0.05)
        {
            shipController.thrusterGroupFiring[3] = true;
        }
        if (relativeVelocity.y > 0.05)
        {
            shipController.thrusterGroupFiring[2] = true;
        }
    }

    public static void DampenHorizontally()
    {
        if (relativeVelocity.x < -0.05)
        {
            shipController.thrusterGroupFiring[0] = true;
        }
        if (relativeVelocity.x > 0.05)
        {
            shipController.thrusterGroupFiring[1] = true;
        }
    }
}
