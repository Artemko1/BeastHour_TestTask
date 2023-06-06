using Entitas;
using UnityEngine;

[Game]
public sealed class CharacterControllerComponent : IComponent
{
    public CharacterController Value;
}

[Game]
public sealed class AnimatorComponent : IComponent
{
    public Animator Value;
}