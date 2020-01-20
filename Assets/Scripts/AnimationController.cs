using UnityEngine;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour
{
    private Animator anim;

    private readonly string jump = "JumpTrigger";
    private readonly string grounded = "IsGrounded";
    private readonly string movingSpeed = "MovingSpeed";
    private readonly string turningSpeed = "TurningSpeed";
    private readonly string isMoving = "IsMoving";
    private readonly string isTurning = "IsTurning";
    
    public Rigidbody bodyRigidBody;
    private Quaternion originalBodyRotation;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = true;
        originalBodyRotation = bodyRigidBody.transform.localRotation;
    }

    private void EnableRagdoll(bool enable)
    {
        if (!enable)
        {
            bodyRigidBody.transform.localRotation = originalBodyRotation;
        }

        bodyRigidBody.isKinematic = !enable;

        // turn on/off animator
        anim.enabled = !enable;
    }

    public void Jump()
    {
        anim.SetTrigger(jump);
        // EnableRagdoll(true);
    }

    public void Landed()
    {
        //EnableRagdoll(false);
    }

    public void IsGrounded(bool isGrounded)
    {
        anim.SetBool(grounded, isGrounded);
    }

    public void SetVelocity(float vel)
    {
        anim.SetBool(isMoving, vel != 0);
        anim.SetFloat(movingSpeed, vel);
    }

    public void SetTurning(float turnVel)
    {
        anim.SetBool(isTurning, turnVel != 0);
        anim.SetFloat(turningSpeed, turnVel);
    }
}
