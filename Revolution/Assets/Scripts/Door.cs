using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private Animator _animator;
    private OffMeshLink _link;
    public bool IsOpen;
    public bool IsOpening;

    // Use this for initialization
	void Start ()
	{
	    IsOpen = false;
	    IsOpening = false;
	    _animator = GetComponent<Animator>();
	}

    public void Open()
    {
        _animator.SetTrigger("Open");
        IsOpening = true;
        Debug.Log("Open");
    }

    public void Close()
    {
        _animator.SetTrigger("Close");
    }
}
