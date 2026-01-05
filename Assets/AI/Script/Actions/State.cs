using System;
using Unity.Behavior;

[BlackboardEnum]
public enum State
{
    Walking,
	MoveToAttackPos,
	SuppressPos,
	Shooting,
	Flee,
	Suppressed
}
