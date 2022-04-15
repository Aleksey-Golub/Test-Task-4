using UnityEngine;

public class OrderPoint : MonoBehaviour
{
    public Vector3 Position => transform.position;
    public bool IsOccupied { get; set; }
}
