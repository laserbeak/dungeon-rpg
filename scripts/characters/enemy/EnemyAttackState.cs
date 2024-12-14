using Godot;
using System;

public partial class EnemyAttackState : EnemyState
{
    protected override void EnterState()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_ATTACK);
    }

    protected override void ExitState()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_IDLE);
    }
}
