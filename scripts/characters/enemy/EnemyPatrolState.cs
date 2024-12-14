using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class EnemyPatrolState : EnemyState
{
    [Export] private Timer idleTimerNode;
    [Export(PropertyHint.Range, "0,20,.1")] private float maxIdleTime = 4.0f;
    private int pointIndex;
    
    public override void _Ready()
    {
        base._Ready();
    }

    protected override void EnterState()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_MOVE);
       
        pointIndex = 1;

        destination = GetPointGlobalPosition(pointIndex);
        characterNode.AgentNode.TargetPosition = destination;

        characterNode.AgentNode.NavigationFinished += HandleNavbigationFinished;
        idleTimerNode.Timeout += HandleTimeout;
        characterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;
    }

    protected override void ExitState()
    {
        characterNode.AgentNode.NavigationFinished -= HandleNavbigationFinished;
        idleTimerNode.Timeout -= HandleTimeout;
        characterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(!idleTimerNode.IsStopped())
        {
            return;
        }

        Move();
    }

    private void HandleNavbigationFinished()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_IDLE);
        
        RandomNumberGenerator rng = new();
        idleTimerNode.WaitTime = rng.RandfRange(0, maxIdleTime);

        idleTimerNode.Start();
    }

    private void HandleTimeout()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_MOVE);

        pointIndex = Mathf.Wrap(
            pointIndex + 1,
            0,
            characterNode.PathNode.Curve.PointCount
        );

        destination = GetPointGlobalPosition(pointIndex);
        characterNode.AgentNode.TargetPosition = destination;
    }
}