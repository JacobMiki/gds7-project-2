using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class TouchableBehaviour : MonoBehaviour
{
    public abstract void OnTouched();

    public bool Enabled { get; set; } = true;
}
