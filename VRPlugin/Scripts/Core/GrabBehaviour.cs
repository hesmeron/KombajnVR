using System;
using UnityEngine;
using UnityEngine.Assertions;

public struct TransformData
{
    private Vector3 _position;
    private Quaternion _rotation;

    public Vector3 Position
    {
        get => _position;
        set => _position = value;
    }

    public Quaternion Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }

    public TransformData(Vector3 position, Quaternion rotation)
    {
        _position = position;
        _rotation = rotation;
    }

    public TransformData TransformPoint(Matrix4x4 matrix)
    {
        _position = matrix.inverse.MultiplyPoint(_position);
        return this;
    }
}

public class GrabBehaviour : MonoBehaviour
{
    public event Action<float> OnValueChanged; 
    
    [SerializeField] 
    private Transform _target;
    [SerializeField]
    private InteractionHandle _handle;
    [SerializeField]
    private GrabControlScheme _grabControlScheme;

#region PrivateFields
    private GrabController _currentGrabController;
    private GrabControllerType _currentGrabControllerType;
    private Matrix4x4 _matrix = new Matrix4x4();
    private TransformData _transformData;
    private Vector3 _lastPosition;
#endregion
    
#region Properites
    public Transform Target => _target;

    public GrabControlScheme GrabControlScheme
    {
        get => _grabControlScheme;
        set => _grabControlScheme = value;
    }
#endregion


#region UnityMethods
    private void OnEnable()
    {
        _handle.OnGrabbed += OnHandleGrabbed;
        _handle.OnReleased += HandleOnReleased;
        _handle.OnBeingGrabbed += OnBeingGrabbed;
    }

    private void OnDisable()
    {
        _handle.OnGrabbed -= OnHandleGrabbed;
        _handle.OnReleased -= HandleOnReleased;
        _handle.OnBeingGrabbed -= OnBeingGrabbed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        _matrix = GetMatrix();
        if (_handle.IsGrabbed)
        {
            Assert.IsNotNull(_currentGrabController);
            Vector3 direction = _transformData.Rotation * Vector3.forward;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, transform.position + direction);
            Quaternion corrected = _transformData.Rotation * _matrix.rotation;
            Vector3 correctedDirection = _matrix.rotation * direction;
            correctedDirection *= 100;
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, transform.position + correctedDirection);
            Gizmos.matrix = _matrix;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Vector3.zero,  direction);
            Gizmos.DrawSphere(_lastPosition, 0.2f);
            _currentGrabController.DrawGizmosWhenUsed();
        }
    }
#endregion
#region PublicMethods

    public void SetControlScheme(GrabControlScheme grabControlScheme)
    {
        _grabControlScheme = grabControlScheme;
        UpdateGrabController();
    }

    public Quaternion TargetLocalRotation()
    {
        return _target.transform.rotation * _matrix.rotation;
    }

    public void ReturnValue(float value)
    {
        OnValueChanged?.Invoke(value);
    }
    
    public Matrix4x4 GetMatrix()
    {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetTRS(transform.position, transform.rotation, Vector3.one);
        return matrix;
    }
#endregion
#region PrivateMethods 
    private void HandleOnReleased(Interactor interactor)
    {
        if (_currentGrabController != null)
        {
            _currentGrabController.OnReleased();
        }

        UpdateGrabController();
    }

    private void OnHandleGrabbed(Interactor interactor)
    {
        UpdateGrabController();
        _currentGrabController.OnGrabbed();
    }

    private void OnBeingGrabbed(TransformData[] args)
    {
        _matrix = GetMatrix();
        
        for (int index = 0; index < args.Length; index++)
        {
            TransformData data = args[index];
            args[index] =  data.TransformPoint(_matrix);
        }

        Vector3 startPosition = _matrix.MultiplyPoint(_target.transform.position);
        Quaternion startQuaternion = TargetLocalRotation();
        
        TransformData transformData = _currentGrabController.OnBeingGrabbed(args, startPosition, startQuaternion);
        Vector3 newGlobalPosition = _matrix.MultiplyPoint(transformData.Position);
        _lastPosition = _matrix.inverse.MultiplyPoint(transformData.Position);
        _target.transform.position = newGlobalPosition;
        _transformData = transformData;
        _target.transform.rotation = _matrix.rotation * transformData.Rotation;
    }

    private void UpdateGrabController()
    {
        _currentGrabController = _grabControlScheme.GetGrabController(_handle.InteractorsGrabbing.List, this);
        if (_currentGrabController != null)
        {
            _currentGrabController.OnValueChanged += CurrentGrabControllerOnValueChanged;
        }
    }

    private void CurrentGrabControllerOnValueChanged(float value)
    {
        OnValueChanged?.Invoke(value);
    }

#endregion
}
