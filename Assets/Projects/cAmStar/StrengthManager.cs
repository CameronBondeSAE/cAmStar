using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BehaviourEntry
{
	public MonoBehaviour behaviour;
	public float strengthMultiplier;
}

public class StrengthManager : MonoBehaviour
{
	public List<BehaviourEntry> behaviours;
	
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
