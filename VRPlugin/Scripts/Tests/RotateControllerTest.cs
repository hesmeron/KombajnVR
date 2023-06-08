using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class RotateControllerTest : MonoBehaviour
{
    [Test]
    public void RotateControllerTestSimple()
    {
        RotateGrabController.Data data = new RotateGrabController.Data(Vector3.zero, Vector3.up);
        RotateGrabController rotateGrabController = new RotateGrabController(data, Vector3.left, Callback);
        TransformData[] args = new []{new TransformData(Vector3.right+ Vector3.up, Quaternion.identity)};
        Vector3 currentPosition = Vector3.zero;
        Quaternion currentRotation = Quaternion.identity;
        TransformData output = rotateGrabController.OnBeingGrabbed(args, currentPosition, currentRotation);
        Assert.IsTrue(currentPosition == output.Position);
        //Assert.IsTrue(output.Rotation == Quaternion.LookRotation(Vector3.right) *Quaternion.Inverse(startRotation));
    }
    
    [Test]
    public void RotateControllerRightAngleTest()
    {
        RotateGrabController.Data data = new RotateGrabController.Data(Vector3.zero, Vector3.up);
        RotateGrabController rotateGrabController = new RotateGrabController(data, Vector3.left, Callback);
        TransformData[] args = new []{new TransformData(Vector3.right+ Vector3.up, Quaternion.identity)};
        Vector3 currentPosition = Vector3.zero;
        Quaternion currentRotation = Quaternion.identity;
        TransformData output = rotateGrabController.OnBeingGrabbed(args, currentPosition, currentRotation);
        Assert.IsTrue(currentPosition == output.Position);
        //Assert.IsTrue(output.Rotation == Quaternion.LookRotation(Vector3.right) *Quaternion.Inverse(startRotation));
    }
    
    [Test]
    public void MatrixTest()
    {
        Matrix4x4 _matrix = new Matrix4x4();
        _matrix.SetTRS(Vector3.zero, Quaternion.Euler(90,0,0),Vector3.one );
        Vector3 directionToCheck = new Vector3(1, 1, 0);
        directionToCheck.Normalize();
        Quaternion rotation = Quaternion.LookRotation(directionToCheck);
        Quaternion correctedRotation = rotation * _matrix.rotation;
        UnityEngine.Assertions.Assert.IsTrue(correctedRotation == rotation * _matrix.rotation);
        Vector3 correctedDirection = correctedRotation *Vector3.forward;
        Debug.Log($"Rotations {correctedRotation} {Quaternion.LookRotation(correctedDirection)}");
        UnityEngine.Assertions.Assert.IsTrue(Quaternion.LookRotation(correctedDirection) == correctedRotation);
        Debug.Log("Directions " +_matrix.inverse.MultiplyPoint(directionToCheck)+" " +directionToCheck + " " + correctedDirection);
        UnityEngine.Assertions.Assert.IsTrue(_matrix.rotation * directionToCheck == correctedDirection);
    }
    
    [Test]
    public void MatrixTest2()
    {
        Matrix4x4 _matrix = new Matrix4x4();
        _matrix.SetTRS(Vector3.zero, Quaternion.Euler(90,0,0),Vector3.one );
        Vector3 directionToCheck = new Vector3(1, 1, 0);
        directionToCheck.Normalize();
        Quaternion rotation = Quaternion.LookRotation(directionToCheck);
        UnityEngine.Assertions.Assert.IsTrue(rotation * Quaternion.Euler(90,0,0) == rotation * _matrix.rotation);

    }
    
    [Test]
    public void MatrixTest3()
    {
        Matrix4x4 _matrix = new Matrix4x4();
        _matrix.SetTRS(Vector3.zero, Quaternion.Euler(90,0,0),Vector3.one );
        Vector3 directionToCheck = new Vector3(1, 1, 0);
        directionToCheck.Normalize();
        Quaternion rotation = Quaternion.LookRotation(directionToCheck);
        UnityEngine.Assertions.Assert.IsTrue(_matrix.rotation * rotation * Vector3.forward == _matrix.rotation * directionToCheck);
    }
    
    [Test]
    public void MatrixTest4()
    {
        Matrix4x4 _matrix = new Matrix4x4();
        _matrix.SetTRS(Vector3.zero, Quaternion.Euler(90,0,0),Vector3.one );
        Vector3 directionToCheck = new Vector3(1, 1, 0);
        directionToCheck.Normalize();
        Quaternion rotation = Quaternion.LookRotation(directionToCheck, Vector3.up);
        Quaternion correctedRotation = rotation * _matrix.rotation;
        Vector3 correctedDirection = correctedRotation * Vector3.forward;
        Quaternion retrivedRoation = Quaternion.LookRotation(correctedDirection);
        Debug.Log($"Rotations {correctedRotation} {retrivedRoation}");
        float angle = Quaternion.Angle(correctedRotation, retrivedRoation);
        Debug.Log("Angle " + angle);
        float angle2 = Quaternion.Angle(correctedRotation, Quaternion.Inverse(retrivedRoation));
        Debug.Log("Angle2 " + angle2);
        bool isEqual = angle < 3f;
        UnityEngine.Assertions.Assert.IsTrue(isEqual) ;
        UnityEngine.Assertions.Assert.IsTrue(_matrix.rotation * directionToCheck == correctedDirection);
    }
    
    [Test]
    public void MatrixTest5()
    {
        Vector3 directionToCheck = new Vector3(1, 1, 0);
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        Quaternion lookRotation = Quaternion.LookRotation(directionToCheck);
        Debug.Log( rotation* directionToCheck);
        Debug.Log( lookRotation +" " + lookRotation.eulerAngles);
        Debug.Log( "est" + lookRotation * Vector3.forward);
        Debug.Log( lookRotation * rotation);
        Debug.Log( (lookRotation * rotation * Vector3.forward));
        UnityEngine.Assertions.Assert.IsTrue(rotation* directionToCheck == lookRotation * rotation * Vector3.forward);
    }

    private void Callback(float obj)
    {
        //throw new System.NotImplementedException();
    }
}
