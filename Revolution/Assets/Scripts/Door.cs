using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{

    private OffMeshLink _link;
    public bool IsOpen;
	// Use this for initialization
	void Start ()
	{
	    _link = GetComponent<OffMeshLink>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (_link.occupied)
	    {

	    }
	}
}
