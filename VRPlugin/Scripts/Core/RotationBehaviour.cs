using UnityEngine;

public class RotationBehaviour : MonoBehaviour
{

    [SerializeField]
    private RotateGrabController.Data _data;
    [SerializeField] 
    private InteractionHandle _handle;
    [SerializeField] 
    private Transform _target;
    
    private RotateGrabController _rotateGrabController;
    private Matrix4x4 _matrix = new Matrix4x4();

    private void Start()
    {
        _handle.OnBeingGrabbed += HandleOnonBeingGrabbed;
        _rotateGrabController = new RotateGrabController(_data, Vector3.forward, Callback );
    }

    private void HandleOnonBeingGrabbed(TransformData[] args)
    {
        for (int index = 0; index < args.Length; index++)
        {
            TransformData data = args[index];
            args[index] =  data.TransformPoint(_matrix);
        }
        _matrix.SetTRS(_target.position, _target.rotation, Vector3.one);

        Vector3 startPosition = _matrix.MultiplyPoint(_target.transform.position);
        /*
        Quaternion startQuaternion = TargetLocalRotation();
        
        TransformData transformData = _currentGrabController.OnBeingGrabbed(args,
            startPosition,
            startQuaternion);
        Vector3 newGlobalPosition = _matrix.inverse.MultiplyPoint(transformData.Position);
        _lastPosition = transformData.Position;
        _target.transform.position = newGlobalPosition;
        _target.transform.rotation = transformData.Rotation * _matrix.inverse.rotation;
        */
    }

    private void Callback(float obj)
    {
        //throw new NotImplementedException();
    }
    
}
