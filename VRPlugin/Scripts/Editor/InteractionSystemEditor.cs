using System.Drawing.Drawing2D;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(InteractionSystem))]
public class InteractionSystemEditor : Editor
{
    public void OnSceneGUI()
    {
        //InteractionHandleBoundsEditor _interactionHandleBoundsEditor = new InteractionHandleBoundsEditor();
        InteractionSystem system = target as InteractionSystem;
        foreach (Interactor interactor in system.Interactors)
        {
            HandleInteractor(interactor);
        }
        foreach (InteractionHandle handle in system.Handles)
        {
            //_interactionHandleBoundsEditor.EditBounds(handle);
        }
    }

    private void HandleInteractor(Interactor interactor)
    {
        EditorGUI.BeginChangeCheck();
        Handles.color = new Color(0, 1, 0, 1f);
        Vector3 transformPosition = interactor.transform.position;
        float radius = 0.2f;

        EditorGUI.BeginChangeCheck();
        Vector3 grabPosition;

        grabPosition =transformPosition;
        Handles.matrix = Matrix4x4.identity;
        Handles.color = new Color(0, 1f, 0f, 0.25f);
        Vector3 position = Handles.FreeMoveHandle(grabPosition, 
            Quaternion.identity,radius, 
            Vector3.one * radius, 
            Handles.SphereHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            interactor.transform.position = position;
        }
    }

}
