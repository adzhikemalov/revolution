using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private Animator _animator;
    private OffMeshLink _link;

    [HideInInspector]
    public bool IsOpen;
    [HideInInspector]
    public bool IsOpening;

    public event EmptyHandler DoorOpened;
    public event EmptyHandler DoorClosed;



    // Use this for initialization
	void Start ()
	{
	    IsOpen = false;
	    IsOpening = false;
	    _animator = GetComponent<Animator>();
	}

    public void DoorOpenedAnimation()
    {
        IsOpen = true;
        IsOpening = false;
        if (DoorOpened != null) DoorOpened();
    }

    public void DoorClosedAnimation()
    {
        IsOpen = false;
        IsOpening = false;
        if (DoorClosed != null) DoorClosed();
    }
    public void Open()
    {
        _animator.SetTrigger("Open");
        IsOpening = true;
    }

    public void Close()
    {
        _animator.SetTrigger("Close");
    }
}
