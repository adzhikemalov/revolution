using UnityEngine;
using System.Collections;

public class PlaneInput : MonoBehaviour {
    CharacterMovement _movementController;
    Vector3 _targetPosition;
    GameObject _player;

	void Start () {

        _player = GameObject.FindGameObjectWithTag("Player");
	}

    private void FixedUpdate()
    {
        var ball = GameObject.FindGameObjectWithTag("Ball");
        if (Input.GetMouseButton(0))
        {
            if (_movementController == null)
            {
                if (_player)
                {
                    _movementController = _player.GetComponent<CharacterMovement>();
                    if (_movementController == null)
                    {
                        Debug.Log("No character movement script attached");
                    }
                }
                else
                {
                    Debug.Log("Can't find Player");
                }
            }
            else
            {
                var layerMask = 1 << 8;
                layerMask = ~layerMask;
                RaycastHit rayHit = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMask))
                {
                    ball.transform.position = rayHit.point;
                    _targetPosition = rayHit.point;
                }
            }
        }

        if (_movementController != null)
        {
            _movementController.TargetPosition = _targetPosition;
        }
    }
}
