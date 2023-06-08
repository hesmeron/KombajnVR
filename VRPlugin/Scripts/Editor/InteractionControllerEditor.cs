using System;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(InteractionController))]
public class InteractionControllerEditor : Editor
{
    private void OnSceneGUI()
    {
        InteractionController controller =  target as InteractionController;
        EditorGUI.BeginChangeCheck();
        Handles.color = new Color(0, 0, 1f, 1f);
        //gunBarrel.StartPivot = Handles.PositionHandle(gunBarrel.StartPivot, gunBarrel.transform.rotation);
        //gunBarrel.EndPivot = Handles.PositionHandle(gunBarrel.EndPivot, gunBarrel.transform.rotation);
        //Handles.DrawLine(gunBarrel.StartPivot, gunBarrel.EndPivot);
    }
}
