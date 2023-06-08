using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGrabScaffolding : MonoBehaviour
{
    [SerializeField] private Vector3 _point;

    [SerializeField] private Vector3 origin;

    [SerializeField] private Vector3 _normal;

    private Vector3 _result;

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        transform.up = _normal;
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.InverseTransformPoint(origin), new Vector3(10, 0.01f, 10));
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawSphere(_point, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_result, 0.3f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, _point);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + _normal);

    }

    // Update is called once per frame
    void Update()
    {
        _normal.Normalize();
        _result = CastPoint(_normal, origin, _point);
    }
    
    private Vector3 CastPoint(Vector3 normal, Vector3 origin, Vector3 point)
    {

        return CastPoint(normal, point - origin) + origin;
    }
    
    private Vector3 CastPoint(Vector3 normal, Vector3 point)
    {
        float normalDotPoint = Vector3.Dot(normal, point);
        float normalDotNormal = Vector3.Dot(normal, normal);
        float t = normalDotPoint / normalDotNormal;
        return point + (-t * normal);
    }
}
