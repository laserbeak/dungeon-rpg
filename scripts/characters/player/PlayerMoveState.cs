using Godot;
using System;

public partial class PlayerMoveState : PlayerState
{
    [Export(PropertyHint.Range, "0, 10, .1")] private float speed = 5;
    public override void _PhysicsProcess(double delta)
    {
        if(characterNode.direction == Vector2.Zero)
        {
            characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
            return;
        }

        characterNode.Velocity = new(characterNode.direction.X, 0, characterNode.direction.Y);
        characterNode.Velocity *= 5;

        CheckFloor();
        characterNode.MoveAndSlide();

        characterNode._Flip();
    }

    public override void _Input(InputEvent @event)
    {
        CheckForAttackInput();
        
        if(Input.IsActionJustPressed(GameConstants.INPUT_DASH))
        {
            characterNode.StateMachineNode.SwitchState<PlayerDashState>();
        }
    }

    protected override void EnterState()
    {
        base.EnterState();

        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_MOVE);
    }

    protected void CheckFloor()
    {
        if(!characterNode.IsOnFloor())
        {
            Vector3 velocity = characterNode.Velocity;
            velocity.Y -= 9.8f;
            characterNode.Velocity = velocity;
        }
    }
}
