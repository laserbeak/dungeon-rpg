using Godot;
using System;

public partial class PlayerAttackState : PlayerState
{
    [Export] private Timer comboTimerNode;
    private int comboCounter = 1;
    private int maxComboCount = 2;

    public override void _Ready()
    {
        base._Ready();
        comboTimerNode.Timeout += () => comboCounter = 1; //? Reset the combo counter when the combo timer times out
    }
    protected override void EnterState()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_ATTACK + comboCounter, -1, 2);
        characterNode.AnimPlayerNode.AnimationFinished += HandleAnimationFinished; //? Subscribe to the AnimationFinished signal
    }

    protected override void ExitState()
    {
        characterNode.AnimPlayerNode.AnimationFinished -= HandleAnimationFinished; //? Unsubscribe from the AnimationFinished signal
        comboTimerNode.Start(); //? Start the combo timer
    }

    private void HandleAnimationFinished(StringName animName)
    {
        comboCounter++;
        comboCounter = Mathf.Wrap(comboCounter, 1, maxComboCount+1); //? use +1 to wrap around to 1, otherwise combo 2 would never play.
        characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
    }

}
