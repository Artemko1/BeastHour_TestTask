using Entitas;
using UnityEngine;

    public sealed class BlinkingComponent : IComponent
    {
        public float RemainingTime;
        public Vector3 Direction;
    }
