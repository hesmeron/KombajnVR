using System;
using UnityEngine;
using UnityEngine.Assertions;

public class RotationWheel : MonoBehaviour
{
        
    [System.Serializable]
    public class Data
    {
        [SerializeField]
        private Vector3 _axisOrigin;
        [SerializeField]
        private Vector3 _axisNormal;
        [SerializeField] 
        private float _maxTurn = 360f;

        public Vector3 AxisOrigin => _axisOrigin;

        public Vector3 AxisNormal => _axisNormal;

        public float MaxTurn => _maxTurn;

        public Data(Vector3 axisOrigin, Vector3 axisNormal)
        {
            _axisOrigin = axisOrigin;
            _axisNormal = axisNormal;
        }
    }
    public Action<float> OnValueChanged;

    [SerializeField] 
    private InteractionHandle _interactionHandle;

    [SerializeField] 
    private Transform _movingElement;
    
    [SerializeField]
    private Data _data;
    
    private float _previousAngle = 0f;
    private float _compoundAngle = 0f;
    private float _startAngle = 0f;
    private Vector3 _previousForward;
    private Vector3 _castPoint;
    private Vector3 _rawPoint;

    public void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_data.AxisOrigin, _rawPoint);
        Gizmos.DrawCube(_rawPoint, Vector3.one / 20f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(_data.AxisOrigin, _castPoint);
        Gizmos.DrawCube(_castPoint, Vector3.one / 20f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_data.AxisOrigin, _data.AxisOrigin + _data.AxisNormal);
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetTRS(Gizmos.matrix.GetPosition(), Quaternion.LookRotation(_data.AxisNormal) * Gizmos.matrix.rotation, Vector3.one);
        Gizmos.DrawWireCube(_data.AxisOrigin, new Vector3(2, 0, 2));
    }

    private void Awake()
    {
        _interactionHandle.OnGrabbed += InteractionHandleOnGrabbed;
        _interactionHandle.OnBeingGrabbed += InteractionHandleOnBeingGrabbed;
    }

    private void InteractionHandleOnBeingGrabbed(TransformData[] args)
    {
        Vector3 position = transform.InverseTransformPoint(args[0].Position);
        float angle = GetNewAngle(position);
        _compoundAngle += angle;
        _compoundAngle = Mathf.Clamp(_compoundAngle, 0, _data.MaxTurn);
        Quaternion resultRotation = Quaternion.AngleAxis(_compoundAngle + _startAngle, _data.AxisNormal);
        OnValueChanged?.Invoke(_compoundAngle / _data.MaxTurn);
        _movingElement.transform.rotation =  transform.rotation * resultRotation;
    }

    private void InteractionHandleOnGrabbed(Interactor obj)
    {
        _startAngle = -GetNewAngle(obj.GetPosition());
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
    
    private float GetNewAngle(Vector3 position)
    {
        Assert.IsNotNull(_data);
        Vector3 axisNormal = _data.AxisNormal;
        Vector3 newForward = GetVectorFromPoint(position);
        float angle = Vector3.SignedAngle( _previousForward,newForward, axisNormal);
        _previousForward = newForward;
        return angle;
    }

    private Vector3 GetVectorFromPoint(Vector3 position)
    {
        Vector3 axisOrigin = _data.AxisOrigin;
        Vector3 axisNormal = _data.AxisNormal;
        Vector3 raw = position - axisOrigin;
        Vector3 castPoint = CastPoint(axisNormal, axisOrigin, raw);
        _castPoint = castPoint;
        return  castPoint - axisOrigin;
    }
}
