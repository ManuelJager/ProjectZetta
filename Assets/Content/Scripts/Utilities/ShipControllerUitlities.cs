using System.Collections.Generic;
using UnityEngine;
public static class ShipControllerUitlities
{
    public static void ApplyRB2DForce(Rigidbody2D rb2d, Transform gameObject, ShipGrid.Thrust thrust, Common.Orientation orientation)
    {
        switch (orientation)
        {
            case Common.Orientation.forward:
                rb2d.AddForce(gameObject.right * thrust.forwardThrust * Time.deltaTime);
                break;
            case Common.Orientation.backward:
                rb2d.AddForce(gameObject.right * thrust.backwardsThrust * -1f * Time.deltaTime);
                break;
            case Common.Orientation.left:
                rb2d.AddForce(gameObject.up * thrust.leftThrust * Time.deltaTime);
                break;
            case Common.Orientation.right:
                rb2d.AddForce(gameObject.up * thrust.rightThrust * -1f * Time.deltaTime);
                break;
        }
    }
    public static void SetThrusterGroupFlame(List<IThruster> thrusters, bool value) 
    {
        if (thrusters != null)
        {
            foreach (var thruster in thrusters)
            {
                thruster.SetThrusterFlame(value);
            }
        }
    }
    public static float[] CalculateThrustVectors(List<IThruster>[] thrusterGroups)
    {
        var thrustVectors = new float[4];
        for (int i = 0; i < 4; i++)
        {
            foreach (var item in thrusterGroups[i])
            {
                thrustVectors[i] += item.thrust;
            }
        }
        return thrustVectors;
        
    }
   
}
