using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;

    private readonly string jump = "JumpTrigger";
    private readonly string grounded = "IsGrounded";
    private readonly string movingSpeed = "MovingSpeed";
    private readonly string turningSpeed = "TurningSpeed";
    private readonly string isMoving = "IsMoving";
    private readonly string isTurning = "IsTurning";

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Jump()
    {
        anim.SetTrigger(jump);
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
