using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    //public LayerMask targetMask;
    public LayerMask terrainMask;

    public bool CheckVisible( Vector2 position )
    {
        Vector2 direction = position - (Vector2) transform.position;
        if ( Vector2.Angle(transform.up, direction) < viewAngle / 2 && direction.magnitude < viewRadius )
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, viewRadius, terrainMask.value);
            if( !hit )
            {
                return true;
            }
        }

        return false;
    }
    
    public Vector2 DirFromAngle( float angleInDegrees, bool angleIsGlobal = true )
    {
        if ( !angleIsGlobal )
            angleInDegrees -= transform.eulerAngles.z;
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
