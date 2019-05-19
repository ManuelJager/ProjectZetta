using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dampening
{
    public static void Dampen(Vector2 input, Rigidbody2D rb2d, float zAngle, GameObject target, ShipController.Thrust thrust)
    {
        var absoluteVelocity = rb2d.velocity;
        var relativeVelocity = RotationUtilities.RotateVector2(absoluteVelocity, zAngle);
        var targetRelativeVelocity = relativeVelocity;
        Vector2 targetAbsoluteVelocity;
        //if there is not horizontal but there is vertical input, then we will dampen the ship horizontally
        if (input.x == 0 && input.y == 0)
        {
            targetAbsoluteVelocity = new Vector2();
        }
        else if (input.x == 0)
        {
            targetRelativeVelocity.y = 0f;
            targetAbsoluteVelocity = RotationUtilities.RotateVector2(targetRelativeVelocity, 360 - zAngle);
        }
        //if there is not horizontal but there is vertical input, then we will dampen the ship vertically
        else if (input.y == 0)
        {
            targetRelativeVelocity.x = 0f;
            targetAbsoluteVelocity = RotationUtilities.RotateVector2(targetRelativeVelocity, 360 - zAngle);
        }
        else
        {
            targetAbsoluteVelocity = rb2d.velocity;
        }
        Common.ClearLog();
        Debug.Log("Relative vel is : " + relativeVelocity);
        Debug.Log("TargetRelative vel is : " + targetRelativeVelocity);
        Debug.Log("TargetAbsolute vel is : " + targetAbsoluteVelocity);
        Debug.Log("Absolute vel is : " + rb2d.velocity);
    }
}
