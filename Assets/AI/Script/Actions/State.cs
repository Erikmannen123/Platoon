using System;
using Unity.Behavior;

[BlackboardEnum]
public enum State
{
    Walking,
	MoveToAttackPos,
	Shooting,
	Flee,
	Suppressed
}
