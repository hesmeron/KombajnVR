using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

class CommonInteractionBoundsEditor
{
    private BoxBoundsHandle _boxBoundsHandle = new BoxBoundsHandle();

    public void EditBounds(InteractionBounds bounds)
    {
        Handles.matrix = bounds.transform.localToWorldMatrix;

        Handles.color = new Color(0, 0, 1f, 1f);
        switch (bounds.CurrentBoundType)
        {
            case InteractionBounds.BoundType.Box:
                EditBoxBounds(bounds);
                break;
            case InteractionBounds.BoundType.Sphere:
                EditSphereBound(bounds);
                break;
            case InteractionBounds.BoundType.Frustum:
                EditFrustumBound(bounds);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void EditSphereBound(InteractionBounds bounds)
    {
        EditorGUI.BeginChangeCheck();
        float areaOfEffect = Handles.RadiusHandle(Quaternion.identity, Vector3.zero, bounds.Radius);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(bounds, "Changed Area Of Effect");
            bounds.Radius = areaOfEffect;
        }
    }
    
    private void EditFrustumBound(InteractionBounds bounds)
    {
        EditorGUI.BeginChangeCheck();
        Handles.DrawLine(Vector3.zero, new Vector3(0,0, bounds.MaxDistance));

        int pointCount = 5;
        float distance = bounds.MaxDistance * bounds.ZMultiplier;
        for (int i=0; i < pointCount; i++)
        {
            float radians = (i / (float) pointCount) * 2f*Mathf.PI;
            float x = Mathf.Sin(radians);
            float y = Mathf.Cos(radians);
            Vector2 direction = new Vector2(x, y);
            direction = direction.normalized * distance;
            Handles.DrawLine(Vector3.zero, new Vector3(direction.x,direction.y, bounds.MaxDistance));
        }
    }

    private void EditBoxBounds(InteractionBounds bounds)
    {
        _boxBoundsHandle.size = bounds.Bounds.size;
        _boxBoundsHandle.center = bounds.Bounds.center;
        EditorGUI.BeginChangeCheck();
        _boxBoundsHandle.DrawHandle();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(bounds, "Changed Area Of Effect");
            bounds.EditBounds(_boxBoundsHandle.size, _boxBoundsHandle.center);
        }
    }
}

[CustomEditor(typeof(InteractionBounds))]
public class InteractionBoundsEditor : Editor
{
    private CommonInteractionBoundsEditor _interactionHandleBoundsEditor = new CommonInteractionBoundsEditor();
    public void OnSceneGUI()
    {
        InteractionBounds bounds = target as InteractionBounds;
        _interactionHandleBoundsEditor.EditBounds(bounds);
    }
}
