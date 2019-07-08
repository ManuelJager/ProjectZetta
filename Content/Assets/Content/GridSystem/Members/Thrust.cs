using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NewThrust
{
    public class ThrustVector
    {
        public float thrust;
        public List<IThruster> thrusterMembers;
        public Vector2 forceDirection;
        public Vector2 thrustVector => forceDirection * thrust;
        private float _consumption;
        public float consumption => _consumption;

        public ThrustVector(Vector2 forceDirection)
        {
            thrusterMembers = new List<IThruster>();
            this.forceDirection = forceDirection;
        }
        public bool isFiring
        {
            set
            {
                thrusterMembers.ForEach(item => item.trailManager.isFiring = value);
            }
        }
        public void CalculateConsumption()
        {
            _consumption = thrusterMembers.Sum(x => x.powerConsumption);
        }
        public void AddToGroup(IThruster thruster)
        {
            thrusterMembers.Add(thruster);
            thrust += thruster.thrust;
            _consumption += thruster.powerConsumption;
        }
        public void RemoveFromGroup(IThruster thruster)
        {
            thrusterMembers.Remove(thruster);
            thrust -= thruster.thrust;
            _consumption -= thruster.powerConsumption;
        }
    }
    public struct MinMax
    {
        public float min;
        public float max;

        public MinMax(float centre, float offset)
        {
            min = centre.AddAngle(-90 + offset);
            max = centre.AddAngle(90 - offset);
        }
    }


    public Dictionary<Vector2Int, ThrustVector> thrustVectors;

    private Dictionary<Vector2Int, MinMax> thrusterRanges = new Dictionary<Vector2Int, MinMax>();

    private ShipGrid parentClass;

    private float offset = 2;

    public NewThrust(ShipGrid parentClass, List<IThruster> thrusters = null)
    {
        thrustVectors = new Dictionary<Vector2Int, ThrustVector>();
        this.parentClass = parentClass;
        thrustVectors.Add(Vector2Int.right, new ThrustVector(Vector2.right));
        thrustVectors.Add(Vector2Int.up, new ThrustVector(Vector2.up));
        thrustVectors.Add(Vector2Int.left, new ThrustVector(Vector2.left));
        thrustVectors.Add(Vector2Int.down, new ThrustVector(Vector2.down));
        //Initializes thruster ranges
        for (int i = 0; i < 4; i++)
            thrusterRanges[Extensions.GetOrientationByIndex(i)] = new MinMax(Extensions.GetOrientationByIndex(i).GetRotation(), offset);

        if (thrusters == null)
            return;
        thrusters.ForEach(thruster => Add(thruster));
    }
    public void Add(IThruster thruster)
    {
        var orientation = ((IBlock)thruster).blockBaseClass.orientation;
        var tempThrustVector = thrustVectors[orientation];
        tempThrustVector.AddToGroup(thruster);
        thrustVectors[orientation] = tempThrustVector;
    }
    public void Remove(IThruster thruster)
    {
        var orientation = ((IBlock)thruster).blockBaseClass.orientation;
        var tempThrustVector = thrustVectors[orientation];
        tempThrustVector.RemoveFromGroup(thruster);
        thrustVectors[orientation] = tempThrustVector;
    }
    public void FireThrustersInDirection(float zRotDirection = 0)
    {
        Vector2Int orientation0 = Vector2Int.zero;
        Vector2Int orientation1 = Vector2Int.zero;
        //loops through the thruster ranges and finds thruster groups that can thrust forward.
        foreach (var thrusterRange in thrusterRanges)
        {
            if (zRotDirection.AngleIsInRange(thrusterRange.Value))
            {
                if (orientation0 == Vector2Int.zero)
                    orientation0 = thrusterRange.Key;
                else
                    orientation1 = thrusterRange.Key;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            var orientation = Extensions.GetOrientationByIndex(i);
            thrustVectors[orientation].isFiring = false;
        }

        if (orientation0 != null)
        {
            FireThrusterGroup(orientation0);
            thrustVectors[orientation0].isFiring = true;
            parentClass.controller.currentConsumption += thrustVectors[orientation0].consumption;
        }

        if (orientation1 != null)
        {
            FireThrusterGroup(orientation1);
            thrustVectors[orientation1].isFiring = true;
            parentClass.controller.currentConsumption += thrustVectors[orientation1].consumption;
        }
    }
    private void FireThrusterGroup(Vector2Int orientation, float multiplier = 1)
    {
        var thrustVector = thrustVectors[orientation];
        multiplier = Mathf.Clamp01(multiplier);
        parentClass._rb2d.AddForce(RotationUtilities.RotateVector2(thrustVector.thrustVector * multiplier, parentClass.grid.rotation.eulerAngles.z));
    }
}
