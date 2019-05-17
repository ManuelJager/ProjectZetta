﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    void SetThrustVectors(float[] thrustVectors);
    float[] GetThrustVectors();
    void SetTurningRateVectors(float [] turningRateVectors);
    float[] GetTurningRateVectors();
}
