using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactor))]
public class InteractorEditor : Editor
{
    public void OnSceneGUI()
    {
        Interactor interactor = target as Interactor;
        EditorGUI.BeginChangeCheck();
        Handles.color = new Color(0, 1, 0, 1f);
        Vector3 transformPosition = interactor.transform.position;
        float radius = 0.2f;

        EditorGUI.BeginChangeCheck();
        Vector3 grabPosition;

            grabPosition =transformPosition;
            Handles.color = new Color(0, 1f, 0f, 0.25f);
            var fmh_21_17_638233058009171024 = Quaternion.identity; Vector3 position = Handles.FreeMoveHandle(grabPosition,radius, 
                Vector3.one * radius, 
                Handles.SphereHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                interactor.transform.position = position;
            }
    }
}
