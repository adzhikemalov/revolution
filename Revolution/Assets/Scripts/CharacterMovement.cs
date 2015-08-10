using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {
    public float MoveSpeedMultiplier = 1;
    public float StationaryTurnSpeed = 180;
    public float MovingTurnSpeed = 360;
    public float JumpPower = 10;
    float _turnAmount;
    float _forwardAmount;
    private Vector3 _moveInput;
    private Vector3 _velocity;
    private Rigidbody _rigBody;
    private Animator _anim;

    private IComparer _rayHitComparer;
    public Vector3 TargetPosition { get; set; }

    // Use this for initialization
	void Start () {
        _rigBody = GetComponent<Rigidbody>();
        SetupAnimator();
	    TargetPosition = transform.position;
	}

    void Update()
    {
        Move(TargetPosition - transform.position);
    }
    
    public void Move(Vector3 move)
    {
        if (Vector3.Distance(move, TargetPosition) < 1)
        {
            return;
        }
        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        
        _moveInput = move;
        _velocity = _rigBody.velocity;
        ConvertMoveInput();
        ApplyTurnExtraRotation();
        GroundCheck();
        UpdateAnimator();
    }

    void SetupAnimator()
    {
        _anim = GetComponent<Animator>();

        foreach(Animator childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != _anim)
            {
                _anim.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }

    void OnAnimatorMove()
    {
        if (Time.deltaTime > 0)
        {
            Vector3 v = (_anim.deltaPosition * MoveSpeedMultiplier) / Time.deltaTime;
            v.y = _rigBody.velocity.y;
            _rigBody.velocity = v;
        }
    }

    void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(_moveInput);
        _turnAmount = Mathf.Atan2(localMove.x, localMove.z);
        _forwardAmount = localMove.z;
    }

    void UpdateAnimator()
    {
        _anim.applyRootMotion = true;

        _anim.SetFloat("forward", _forwardAmount, 0.1f, Time.deltaTime);
        _anim.SetFloat("turn", _turnAmount, 0.1f, Time.deltaTime);
    }

    void ApplyTurnExtraRotation()
    {
        float turnSpeed = Mathf.Lerp(StationaryTurnSpeed, MovingTurnSpeed, _forwardAmount);
        transform.Rotate(0, _turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void GroundCheck()
    {
        Ray ray = new Ray(transform.position + Vector3.up * .5f, - Vector3.up);

        RaycastHit[] hits = Physics.RaycastAll(ray, .5f);
        _rayHitComparer = new RayHitComparer();

        System.Array.Sort(hits, _rayHitComparer);
        if (_velocity.y < JumpPower*.5f)
        {
            _rigBody.useGravity = true;
            foreach(var hit in hits)
            {
                if (!hit.collider.isTrigger)
                {
                    if (_velocity.y <= 0)
                    {
                        _rigBody.position = Vector3.MoveTowards(_rigBody.position, hit.point, Time.deltaTime * 5);
                    }

                    _rigBody.useGravity = false;
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
