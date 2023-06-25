using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrabBehaviour))]
public class GrabBehaviourEditor : Editor
{
    public void OnSceneGUI()
    {
        GrabBehaviour grabBehaviour = target as GrabBehaviour;
        Handles.matrix = grabBehaviour.GetMatrix();
        Matrix4x4 matrix = new Matrix4x4();
        RotateGrabController.Data _data = grabBehaviour.GrabControlScheme.RotateData;
        matrix.SetTRS(Handles.matrix.GetPosition(), Quaternion.LookRotation(_data.AxisNormal) * Handles.matrix.rotation, Vector3.one);
        Handles.color = Color.red;
        Handles.DrawWireCube(_data.AxisOrigin, new Vector3(1, 0, 1));
    }
}
