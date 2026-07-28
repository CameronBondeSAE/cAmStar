using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ComboManager : MonoBehaviour
{
	public List<ComboSequence> allCombos;

	public Animation jump;
	
	public PlayableDirector playableDirector;

	private void Start()
	{
		jump.updateMode = AnimationUpdateMode.Fixed;
		
		playableDirector.time = Time.time;
	}
}

[Serializable]
public class ComboSequence
{
	public string            comboName;
	public List<ComboAction> comboSequence;
}

[Serializable]
public class ComboAction
{
	public int   moveDirection;
	public float minValidTime;
	public float startUpTime;
}
