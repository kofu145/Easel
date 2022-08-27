using System;
using System.Numerics;
using BulletSharp;
using Easel.Utilities;

namespace Easel.Entities.Components;

public class Trigger : Component
{
    public event OnCollision Collision;
    
    private CollisionShape _iShape;
    
    private GhostObject _ghost;
    
    public Trigger(CollisionShape shape)
    {
        _iShape = shape;
    }

    protected internal override void Initialize()
    {
        base.Initialize();
        
        Matrix4x4 transform = Matrix4x4.CreateFromQuaternion(Transform.Rotation) *
                              Matrix4x4.CreateTranslation(Transform.Position);

        _iShape.LocalScaling = Transform.Scale;
        _ghost = Physics.AddTrigger(_iShape, transform);
    }

    protected internal override void Update()
    {
        base.Update();

        for (int i = 0; i < _ghost.NumOverlappingObjects; i++)
        {
            Collision?.Invoke(_ghost.OverlappingPairs[i]);
        }
    }

    public delegate void OnCollision(CollisionObject obj);
}