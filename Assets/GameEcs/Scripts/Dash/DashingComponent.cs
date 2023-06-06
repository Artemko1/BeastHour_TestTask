using Entitas;
using UnityEngine;

    public sealed class DashingComponent : IComponent
    {
        public float RemainingTime;
        public Vector3 Direction;
    }
