using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {
    float moveSpeedMultiplier = 1;
    public float stationaryTurnSpeed = 180;
    public float movingTurnSpeed = 360;

    Rigidbody rigBody;

    bool onGround;

    Animator anim;

    Vector3 moveInput;
    float turnAmount;
    float forwardAmount;
    Vector3 velocity;

    float jumpPower = 10;
    IComparer rayHitComparer;
    float targetTolerance = 1;
    Vector3 targetPosition;
    bool moveToTarget;

	// Use this for initialization
	void Start () {
        rigBody = GetComponent<Rigidbody>();
        SetupAnimator();
	}
    
    public void Move(Vector3 move)
    {
        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        
        moveInput = move;
        velocity = rigBody.velocity;
        ConvertMoveInput();
        ApplyTurnExtraRotation();
        GroundCheck();
        UpdateAnimator();
    }

    void SetupAnimator()
    {
        anim = GetComponent<Animator>();

        foreach(Animator childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != anim)
            {
                anim.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }

    void OnAnimatorMove()
    {
        if (onGround && Time.deltaTime > 0)
        {
            Vector3 v = (anim.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
            v.y = rigBody.velocity.y;
            rigBody.velocity = v;
        }
    }

    void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = Mathf.Atan2(localMove.x, localMove.z);
        forwardAmount = localMove.z;
    }

    void UpdateAnimator()
    {
        anim.applyRootMotion = true;

        anim.SetFloat("forward", forwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("turn", turnAmount, 0.1f, Time.deltaTime);
    }

    void ApplyTurnExtraRotation()
    {
        float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void GroundCheck()
    {
        Ray ray = new Ray(transform.position + Vector3.up * .5f, - Vector3.up);

        RaycastHit[] hits = Physics.RaycastAll(ray, .5f);
        rayHitComparer = new RayHitComparer();

        System.Array.Sort(hits, rayHitComparer);
        if (velocity.y < jumpPower*.5f)
        {
            //onGround = false;
            rigBody.useGravity = true;
            foreach(var hit in hits)
            {
                if (!hit.collider.isTrigger)
                {
                    if (velocity.y <= 0)
                    {
                        rigBody.position = Vector3.MoveTowards(rigBody.position, hit.point, Time.deltaTime * 5);
                    }

                    onGround = true;
                    rigBody.useGravity = false;

                    break;
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
