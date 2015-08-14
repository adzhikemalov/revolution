using HutongGames.PlayMaker.Actions;
using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{

    float moveSpeedMultiplier = 1;
    float stationaryTurnSpeed = 180;
    float movingTurnSpeed = 360;

    public bool onGround;

    Animator animator;

    Vector3 moveInput;
    float turnAmount;
    float forwardAmount;
    Vector3 velocity;
    float jumpPower = 10;

    private bool _wait;

    IComparer rayHitComparer;
    Rigidbody rigidBody;
    private NavMeshAgent agent;
    public Vector3 TargetPosition { get; set; }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        SetUpAnimator();

        rigidBody = GetComponent<Rigidbody>();

        TargetPosition = transform.position;

        agent = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider collider)
    {
        var door = collider.gameObject.GetComponent<Door>();
        if (door)
        {
            if (door && (!door.IsOpen || door.IsOpening))
            {
                _wait = true;
                door.DoorOpened += DoorOpenedHandler;
                if (!door.IsOpening)
                    door.Open();
            }
        }
    }

    private void DoorOpenedHandler()
    {
        _wait = false;
    }

    void FixedUpdate()
    {
        if (agent != null)
        {
            if (_wait)
            {
                agent.SetDestination(transform.position);
            }
            else if ((TargetPosition - transform.position).magnitude > 1)
            {
                agent.SetDestination(TargetPosition);
            }
            agent.transform.position = transform.position;
            Move(agent.desiredVelocity);
        }
        else 
            Move(TargetPosition - transform.position);
    }

    void SetUpAnimator()
    {
        animator = GetComponent<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != animator)
            {
                animator.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }

    void OnAnimatorMove()
    {
        if (onGround && Time.deltaTime > 0)
        {
            Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
            v.y = rigidBody.velocity.y;
            rigidBody.velocity = v;
        }
    }

    public void Move(Vector3 move)
    {
        if (move.magnitude > 1)
            move.Normalize();

        this.moveInput = move;

        velocity = GetComponent<Rigidbody>().velocity;

        ConvertMoveInput();
        ApplyExtraTurnRotation();
        GroundCheck();
        UpdateAnimator();

    }

    void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);

        turnAmount = Mathf.Atan2(localMove.x, localMove.z);

        forwardAmount = localMove.z;
    }

    void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void UpdateAnimator()
    {
        animator.applyRootMotion = true;
        animator.SetFloat("forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("turn", turnAmount, 0.1f, Time.deltaTime);
    }

    void GroundCheck()
    {
        Ray ray = new Ray(transform.position + Vector3.up * .1f, -Vector3.up);

        RaycastHit[] hits = Physics.RaycastAll(ray, .1f);
        rayHitComparer = new RayHitComparer();
        System.Array.Sort(hits, rayHitComparer);

        if (velocity.y < jumpPower * .5f)
        {
            onGround = false;
            rigidBody.useGravity = true;

            foreach (var hit in hits)
            {
                if (!hit.collider.isTrigger)
                {
                    if (velocity.y <= 0)
                    {
                        rigidBody.position = Vector3.MoveTowards(rigidBody.position, hit.point, Time.deltaTime * 100);
                    }

                    onGround = true;
                    rigidBody.useGravity = false;
                }
            }
        }
    }

    class RayHitComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
        }
    }

}