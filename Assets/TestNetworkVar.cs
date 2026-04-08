using System;
using UnityEngine;

public class TestNetworkVar : MonoBehaviour
{
	public StatInt health;
	public StatInt shield;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class StatInt
{
	public int maxValue;
	public int minValue;
	public int defaultValue;
	public int currentValue;
}
