using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private Animator _animator;

    [HideInInspector]
    public bool IsOpen;
    [HideInInspector]
    public bool IsOpening;

    public event ParameterHandler<Door> DoorOpened;
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
        if (DoorOpened != null) DoorOpened(this);
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
