using UnityEngine;
using System.Collections;

public class PlaneInput : MonoBehaviour {
    CharacterMovement movementController;
    Vector3 targetPosition;
    NavMeshAgent agent;
    GameObject player;
	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        agent = player.GetComponent<NavMeshAgent>();
	}

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            var i = 1;
            var previousCorner = agent.transform.position;
            while (i < agent.path.corners.Length)
            {
                Vector3 currentCorner = agent.path.corners[i];
                Gizmos.DrawLine(previousCorner, currentCorner);
                previousCorner = currentCorner;
                i++;
            }
        }

    }
	// Update is called once per frame
	void FixedUpdate () 
    {

        var ball = GameObject.FindGameObjectWithTag("Ball");
	    if (Input.GetMouseButton(0))
        {
            if (movementController == null)
            {
                if (player)
                {
                    movementController = player.GetComponent<CharacterMovement>();
                    if (movementController == null)
                    {
                        Debug.Log("No character movement script attached");
                    }
                }
                else
                {
                    Debug.Log("Can't fin Player");
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
                    targetPosition = rayHit.point;
                }
            }
        }

        agent.SetDestination(ball.transform.position);
	    if (FacingClosedDoor)
	    {
	        TryOpenTheDoor();
	    }
	    else
	    {
//            agent.transform.position = player.transform.position;

            if (movementController != null)
            {
                if ((targetPosition - movementController.gameObject.transform.position).magnitude > .1)
                {
                    movementController.Move(agent.desiredVelocity);
                }
                else
                {
                    movementController.Move(Vector3.zero);
                }
            }
	    }
	}

    private void TryOpenTheDoor()
    {
//        agent.Stop();
//        movementController.Move(Vector3.zero);
        var door = agent.currentOffMeshLinkData.offMeshLink.gameObject.GetComponent<Door>();
        if (!door.IsOpening)
            door.Open();
    }

    private bool FacingClosedDoor
    {
        get
        {
            var result = false;
            if (agent.isOnOffMeshLink)
            {
                var door = agent.currentOffMeshLinkData.offMeshLink.gameObject.GetComponent<Door>();
                if (door)
                {
                    if (door.IsOpen)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }

                }
                else
                {
                    result = false;
                }
                result = true;
            }
            return result;
        }
    }
}
