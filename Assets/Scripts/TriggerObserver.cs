using System;
using UnityEngine;

public class TriggerObserver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) =>
        TriggerEnter?.Invoke(other);

    private void OnTriggerExit(Collider other) =>
        TriggerExit?.Invoke(other);

    public event Action<Collider> TriggerEnter;
    public event Action<Collider> TriggerExit;
}