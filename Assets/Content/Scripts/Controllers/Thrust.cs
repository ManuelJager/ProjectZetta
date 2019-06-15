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
        public ThrustVector(Vector2 forceDirection)
        {
            thrusterMembers = new List<IThruster>();
            this.forceDirection = forceDirection;
        }
    }

    public class ThrusterGroup
    {
        private Dictionary<Common.Orientation, bool> orientationShouldFire = new Dictionary<Common.Orientation, bool>();

        public ThrusterGroup()
        {
            orientationShouldFire = new Dictionary<Common.Orientation, bool>()
            {
                { Common.Orientation.forward, false},
                { Common.Orientation.right,   false},
                { Common.Orientation.backward,false},
                { Common.Orientation.left,    false}
            };
        }
        public bool this [Common.Orientation index]
        {
            get => orientationShouldFire[index];
            set => orientationShouldFire[index] = value;
        }
        
    }

    public Dictionary<Common.Orientation, ThrustVector> thrustVectors;

    public NewThrust(List<IThruster> thrusters = null)
    {
        thrustVectors = new Dictionary<Common.Orientation, ThrustVector>();
        thrustVectors.Add(Common.Orientation.forward,  new ThrustVector(Vector2.up));
        thrustVectors.Add(Common.Orientation.left,     new ThrustVector(Vector2.left));
        thrustVectors.Add(Common.Orientation.backward, new ThrustVector(Vector2.down));
        thrustVectors.Add(Common.Orientation.right,    new ThrustVector(Vector2.right));
        if (thrusters == null)
            return;
        thrusters.ForEach(thruster => AddToThrusterGroup(thruster));
    }
    private void AddToThrusterGroup(IThruster thruster)
    {
        var orientation = ((IBlock)thruster).GetOrientation();
        var tempThrustVector = thrustVectors[orientation];
        tempThrustVector.thrust += thruster.thrust;
        tempThrustVector.thrusterMembers.Add(thruster);
        thrustVectors[orientation] = tempThrustVector;
    }
    private void RemoveFromThrusterGroup(IThruster thruster)
    {
        var orientation = ((IBlock)thruster).GetOrientation();
        var tempThrustVector = thrustVectors[orientation];
        tempThrustVector.thrust -= thruster.thrust;
        tempThrustVector.thrusterMembers.Remove(thruster);
        thrustVectors[orientation] = tempThrustVector;
    }
    public void FireThrusterInDirection(float rotation, float rotationOffset)
    {
        rotation.AddAngle(rotationOffset);
        FireThrustersInDirection(rotation);
    }
    public void FireThrustersInDirection(float zRotDirection = 0)
    {
        Common.Orientation orientation0 = new Common.Orientation();
        Common.Orientation orientation1 = new Common.Orientation();
        ThrusterGroup thrusterGroup = new ThrusterGroup();
        thrusterGroup[orientation0] = true;
        thrusterGroup[orientation1] = true;
        Debug.Log(zRotDirection);
    }
    private void GetResultantAndMagnitude()
    {

    }






}
