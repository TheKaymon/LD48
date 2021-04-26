using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FieldOfView : MonoBehaviour
{
    public Light2D vision;
    [HideInInspector]
    public float viewRadius;
    [HideInInspector] // If > 180 View Texture size needs to be increased
    public float viewAngle;
    //public float meshResolution;
    public Vector2 Position => transform.position;
    //public float updateInterval = .25f;

    //public LayerMask targetMask;
    public LayerMask terrainMask;

    //private MeshFilter viewMeshFilter;
    //private Mesh viewMesh;

    //private float updateTimer;

    //private SpriteRenderer spriteRenderer;
    //private Sprite sprite;

    private void Start()
    {
        viewRadius = vision.pointLightOuterRadius;
        viewAngle = vision.pointLightOuterAngle;
        //viewMeshFilter = GetComponent<MeshFilter>();
        //viewMesh = new Mesh
        //{
        //    name = "View Mesh"
        //};
        //viewMeshFilter.mesh = viewMesh;
        //spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        // Create a blank Texture and Sprite to override later on.
        //int textureSize = Mathf.CeilToInt(viewRadius * 50) + 16;
        //var texture2D = new Texture2D(4 * textureSize, 4 * textureSize);
        //float x = Position.x - ( texture2D.width / 2 );
        //spriteRenderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.1f), 100);
        //sprite = spriteRenderer.sprite;
        //spriteR.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(-texture2D.width/2, texture2D.height/2), 1);
        //updateTimer = updateInterval;
    }

    public bool CheckVisible( Vector2 position )
    {
        Vector2 direction = position - Position;
        if ( Vector2.Angle(transform.up, direction) < viewAngle / 2 && direction.magnitude < viewRadius )
        {
            RaycastHit2D hit = Physics2D.Raycast(Position, direction.normalized, viewRadius, terrainMask.value);
            if( !hit )
            {
                return true;
            }
        }

        return false;
    }

    //private void Update()
    //{
    //    if ( !GameManager.instance.paused )
    //    {
    //        //updateTimer += Time.deltaTime;
    //        //if ( updateTimer > updateInterval )
    //        //{
    //        //    DrawFieldOfView();
    //        //    updateTimer = 0f;
    //        //}
    //    }
    //}

    //private void DrawFieldOfView()
    //{
    //    int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
    //    float stepAngleSize = viewAngle / stepCount;
    //    float angle = -transform.eulerAngles.z - viewAngle / 2;
    //    List<Vector3> viewPoints = new List<Vector3>();
    //    Vector2 point = Vector2.zero;

    //    for ( int i = 0; i <= stepCount; i++ )
    //    {
    //        RaycastHit2D hit = ViewCast(angle);
    //        if ( hit )
    //        {
    //            point = hit.point;
    //        }
    //        else
    //        {
    //            point = Position + DirFromAngle(angle) * viewRadius;
    //        }
    //        viewPoints.Add(point);
    //        //Debug.Log($"Vertex {i}: {viewPoints[i]}");

    //        //Debug.DrawLine(Position, Position + DirFromAngle(angle) * viewRadius, Color.red);
    //        angle += stepAngleSize;
    //    }

    //    int vertexCount = viewPoints.Count + 1;
    //    Vector3[] vertices = new Vector3[vertexCount];
    //    int[] triangles = new int[( vertexCount - 2 ) * 3];

    //    // First Vertex
    //    vertices[0] = Vector3.zero;

    //    for ( int i = 0; i < vertexCount - 1; i++ )
    //    {
    //        vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
    //        vertices[i + 1].z = -1f;

    //        //if( i < viewPoints.Count - 1 )
    //            //Debug.DrawLine(viewPoints[i], viewPoints[i + 1], Color.yellow);
    //        if ( i < vertexCount - 2 )
    //        {
    //            triangles[i * 3] = 0;
    //            triangles[i * 3 + 1] = ( i + 1 );
    //            triangles[i * 3 + 2] = ( i + 2 );
    //        }
    //    }

    //    viewMesh.Clear();
    //    viewMesh.vertices = vertices;
    //    viewMesh.triangles = triangles;
    //    viewMesh.RecalculateNormals();
    //}

    //private void DrawSpriteFieldOfView()
    //{
    //    int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
    //    float stepAngleSize = viewAngle / stepCount;
    //    float angle = -transform.eulerAngles.z - viewAngle / 2;
    //    List<Vector2> viewPoints = new List<Vector2>();
    //    Vector2 point = Vector2.zero;

    //    for ( int i = 0; i < stepCount; i++ )
    //    {
    //        RaycastHit2D hit = ViewCast(angle);
    //        if ( hit )
    //        {
    //            point = hit.point;
    //        }
    //        else
    //        {
    //            point = Position + DirFromAngle(angle) * viewRadius;
    //        }
    //        viewPoints.Add(point);
    //        //Debug.Log($"Vertex {i}: {viewPoints[i]}");

    //        //Debug.DrawLine(Position, Position + DirFromAngle(angle) * viewRadius, Color.red);
    //        angle += stepAngleSize;
    //    }

    //    int vertexCount = viewPoints.Count + 1;
    //    Vector2[] vertices = new Vector2[vertexCount];
    //    ushort[] triangles = new ushort[( vertexCount - 2 ) * 3];

    //    // First Vertex
    //    vertices[0] = new Vector2(0.5f, 0.1f);

    //    for ( int i = 0; i < vertexCount-1; i++ )
    //    {
    //        //vertices[i + 1].x = viewPoints[i].x / sprite.bounds.size.x;
    //        vertices[i + 1].x = Mathf.Clamp(
    //                ( viewPoints[i].x - sprite.bounds.center.x -
    //                ( sprite.textureRectOffset.x / sprite.texture.width ) + sprite.bounds.extents.x ) /
    //                ( 2.0f * sprite.bounds.extents.x ) * sprite.rect.width,
    //            0.0f, sprite.rect.width);

    //        //vertices[i + 1].y = viewPoints[i].y / sprite.bounds.size.y;
    //        vertices[i + 1].y = Mathf.Clamp(
    //            ( viewPoints[i].y - sprite.bounds.center.y -
    //            ( sprite.textureRectOffset.y / sprite.texture.width ) + sprite.bounds.extents.y ) /
    //            ( 2.0f * sprite.bounds.extents.y ) * sprite.rect.width,
    //        0.0f, sprite.rect.width);

    //        //Debug.DrawLine(viewPoints[i], viewPoints[i + 1], Color.yellow);
    //        Debug.DrawLine(vertices[i], vertices[i + 1], Color.yellow);
    //        if ( i < vertexCount - 2 )
    //        {
    //            triangles[i * 3] = 0;
    //            triangles[i * 3 + 1] = ( ushort) (i + 1);
    //            triangles[i * 3 + 2] = (ushort) (i + 2);
    //        }
    //    }

    //    spriteRenderer.sprite.OverrideGeometry(vertices, triangles);
    //}

    //private RaycastHit2D ViewCast( float globalAngle )
    //{
    //    Vector2 direction = DirFromAngle(globalAngle);

    //    RaycastHit2D hit = Physics2D.Raycast(Position, direction, viewRadius, terrainMask.value);

    //    return hit;
    //}

    public Vector2 DirFromAngle( float angleInDegrees, bool angleIsGlobal = true )
    {
        if ( !angleIsGlobal )
            angleInDegrees -= transform.eulerAngles.z;
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    //private void OnDrawGizmos()
    //{
    //    //Gizmos.color = Color.white;
    //    //Gizmos.DrawWireSphere(Position, viewRadius);
    //    //Vector2 viewAngleA = DirFromAngle(-viewAngle / 2, false);
    //    //Vector2 viewAngleB = DirFromAngle(viewAngle / 2, false);

    //    //Gizmos.DrawLine(Position, Position + viewAngleA * viewRadius);
    //    //Gizmos.DrawLine(Position, Position + viewAngleB * viewRadius);
    //}
}
