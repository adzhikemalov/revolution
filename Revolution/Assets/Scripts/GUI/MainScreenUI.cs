using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainScreenUI : MonoBehaviour
{
    public Text CurrentTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    CurrentTime.text = Game.TimeManager.CurrentWorldTime;
	}
}
