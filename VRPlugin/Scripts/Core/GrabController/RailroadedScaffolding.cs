using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class RailroadedScaffolding : MonoBehaviour
{
    [OnValueChanged(nameof(ChangeCut))]
    [SerializeField] 
    private Vector3 _point;

    [SerializeField] 
    [OnValueChanged(nameof(ChangeCut))]
    private Transform _from, _to;
    [SerializeField]
    private Transform _indicator;

    private Vector3 _result;
    private Vector3 _perpendicular;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_from.transform.position, _to.transform.position);
        Gizmos.DrawLine(_point, _point + _perpendicular);
        Gizmos.DrawLine(_from.transform.position, _point);
        Gizmos.DrawLine(_to.transform.position, _point);
        Gizmos.DrawLine(_point, _point + _perpendicular);
        Gizmos.DrawSphere(_point, 0.1f);
        Gizmos.color = Color.red;
        _indicator.transform.position = _result;
        Gizmos.DrawSphere(_result, 0.1f);
        Gizmos.DrawLine(_point, _result);
        Gizmos.DrawWireCube(_point, new Vector3(10, 10, 0.01f));
    }

    private void ChangeCut()
    {
        //this is not a production code don't kill me
        Debug.Log(Trigonometry.GetCastPoint(_from.transform.position, _to.transform.position, _point, out _result));
    }

    private bool GetCastPoint(Vector3 from, Vector3 to, Vector3 pointToCast, out Vector3 result)
    {
        Vector3 direction = to - from;
        Vector3 normal = direction;
        Vector3 translation = to - from;
        Debug.Log(translation);
        float dot = Vector3.Dot(normal, translation);
        _perpendicular = normal;
        if (Mathf.Abs(dot) > Single.Epsilon)
        {
            Vector3 distance1 = from - pointToCast;
            float fac = -Vector3.Dot(normal, distance1) / dot;
            fac = Mathf.Clamp01(fac);
            
            translation *= fac;
            result = from + translation;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
