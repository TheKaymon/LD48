using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Public Variables
    public float speed;
    public AnimationCurve movementCurve;

    // Private Variables
    private FieldOfView fov;
    private PatrolPath path;
    private int pathIndex = 0;
    private bool pathReversed;

    // ToDo: Create Class? Shared with Player
    private Vector2 start;
    private Vector2 destination;
    private float movingTimer = -1f;
    private float movingDuration;

    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
        path = GetComponent<PatrolPath>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if Player is Visible
        if( fov.CheckVisible(GameManager.instance.player.transform.position) )
        {
            GameManager.instance.PlayerVisible(this);
        }
        // Otherwise Move Along Patrol
        else
        {
            movingTimer -= Time.deltaTime;
            if( movingTimer < 0 )
            {
                int previousIndex = pathIndex;

                if ( pathReversed )
                {
                    pathIndex--;
                    // First Index Reached
                    if ( pathIndex < 0 )
                    {
                        pathReversed = false;
                        pathIndex = 1;
                    }
                }
                else
                {
                    pathIndex++;
                    // Last Index Reached
                    if ( pathIndex > path.LastIndex )
                    {
                        if ( path.loopPath )
                        {
                            pathIndex = 0;
                        }
                        else
                        {
                            pathReversed = true;
                            pathIndex = path.LastIndex - 1;
                        }
                    }
                }

                // Calculate Next Position
                //Debug.Log($"Moving from index {previousIndex} to {pathIndex}");
                float distance = path.GetDistance(previousIndex, pathIndex);
                start = transform.position;
                destination = path[pathIndex];
                movingTimer = movingDuration = distance / speed;
            }
            else
            {
                float lerp = movementCurve.Evaluate(movingTimer / movingDuration);
                transform.position = Vector2.Lerp(destination, start, lerp);
            }
        }
    }
}
