using UnityEngine;
using System.Collections;

public class PlaneInput : MonoBehaviour {
    CharacterMovement movementController;
    Vector3 targetPosition;
    NavMeshAgent agent;
	// Use this for initialization
	void Start () {

        var player = GameObject.FindGameObjectWithTag("Player");
        agent = player.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {        
         var player = GameObject.FindGameObjectWithTag("Player");
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
                    var ball = GameObject.FindGameObjectWithTag("Ball");
                    ball.transform.position = rayHit.point;
                    targetPosition = rayHit.point;
                    agent.SetDestination(targetPosition);
                }
            }
        }

	    if (agent.isOnOffMeshLink)
	    {
            agent.Stop();
            movementController.Move(Vector3.zero);
	    }
	    else
	    {
            agent.transform.position = player.transform.position;

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
}
