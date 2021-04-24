using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PatrolPath))]
public class PatrolPathEditor : Editor
{
    private void OnSceneGUI()
    {
        PatrolPath path = (PatrolPath)target;

        Handles.color = Color.yellow;
        if( path.NumPoints > 0 )
        {
            for ( int i = 0; i < path.NumPoints; i++ )
            {
                Vector2 newPos = Handles.FreeMoveHandle(path.pathPoints[i], Quaternion.identity, 0.25f, Vector2.zero, Handles.CylinderHandleCap);
                if( path[i] != newPos )
                {
                    Undo.RecordObject(path, "Move Path Point");
                    path[i] = newPos;
                }
            }
        }


        if( path.NumPoints > 1 )
        {
            Handles.color = Color.green;
            for ( int i = 1; i < path.NumPoints; i++ )
            {
                Handles.DrawLine(path[i - 1], path[i]);
            }

            if( path.loopPath )
            {
                Handles.DrawLine(path[path.NumPoints - 1], path[0]);
            }
        }
    }
}
