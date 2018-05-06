using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGizmo : MonoBehaviour
{
    [SerializeField] private Color _color = Color.red;
    [SerializeField] private Vector3 _size = new Vector3(1, 1, 1);

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawWireCube(transform.position, _size);
    }
}
