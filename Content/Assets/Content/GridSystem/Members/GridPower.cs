using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power
{
    private List<IPowerConsumer> powerConsumers;
    private List<IPowerGenerator> powerGenerators;

    public float totalConsumption;
    public float totalGeneration;
    public float totalSatisfactionRaw => totalConsumption / totalGeneration;
    public int totalSatisfactionPercentage => Mathf.RoundToInt(totalConsumption / totalGeneration * 100);

    public void Add(IPowerConsumer powerConsumer)
    {

    }

    public void Add(IPowerGenerator powerGenerator)
    {

    }
}
