using System;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    [SerializeField]
    public float TimeMultiplier;
    public static Game Instance { get; set; }
    public static TimeManager TimeManager { get; set; }
    public static OfficeManager OfficeManager { get; set; }
    // Use this for initialization
    private void Awake()
    {
        Instance = this;
        TimeManager = new TimeManager();
        OfficeManager = new OfficeManager();
    }


	// Update is called once per frame
	void FixedUpdate ()
	{
	    if (Math.Abs(TimeManager.GameTimeMultiplier - TimeMultiplier) > 0)
	        TimeManager.GameTimeMultiplier = TimeMultiplier;
	    TimeManager.IncrementTime(Time.deltaTime);
	}
}
