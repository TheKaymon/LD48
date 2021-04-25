using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Public Variables
    public float speed = 1f;
    public float turnSpeed = 30f; // Speed in Degrees per Second
    public float idleTime = 1f;
    public AnimationCurve movementCurve;

    public bool frozen = true;

    // Private Variables
    private readonly static int IDLE = 0;
    private readonly static int MOVING = 1;
    private readonly static int TURNING = 2;

    private FieldOfView fov;
    private PatrolPath path;
    private int previousIndex = 0;
    private int pathIndex = 1;
    private bool pathReversed;
    private int state;

    // ToDo: Create Class? Shared with Player
    private Vector2 initialPosition;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float initialAngle;
    private float startAngle;
    private float endAngle;
    private float actionTimer;
    private float actionDuration;

    public void Reset()
    {
        frozen = true;
        // Reset Pathing
        previousIndex = 0;
        pathIndex = 1;
        // Reset Position
        transform.position = initialPosition;
        // Reset Rotation
        transform.rotation = Quaternion.Euler(0, 0, initialAngle);
        // ToDo: Function? Reused in Movement
        startPosition = transform.position;
        endPosition = path[pathIndex];
        startAngle = transform.eulerAngles.z;
        // ToDo: Make Helper Function?
        endAngle = Mathf.Atan2(endPosition.y - startPosition.y, endPosition.x - startPosition.x) * Mathf.Rad2Deg - 90f;
        if ( endAngle < 0 )
            endAngle += 360;
        transform.eulerAngles = new Vector3(0, 0, endAngle);

        // Setup Action
        actionTimer = actionDuration = idleTime;
        //Debug.Log($"Idle for {actionTimer}");
        state = IDLE;
    }

    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
        path = GetComponent<PatrolPath>();

        initialPosition = transform.position;
        initialAngle = transform.eulerAngles.z;

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if ( !GameManager.instance.paused && !frozen )
        {
            // Check if Player is Visible
            if ( fov.CheckVisible(GameManager.instance.player.transform.position) )
            {
                GameManager.instance.PlayerVisible(this);
            }
            // Otherwise Move Along Patrol
            else
            {
                if ( state == IDLE )
                {
                    Idle();
                }
                else if ( state == TURNING )
                {
                    Turn();
                }
                else if ( state == MOVING )
                {
                    Move();
                }
                else
                {
                    Debug.Log($"Error: Enemy {name} has invalid state.");
                    state = IDLE;
                }
            }
        }
    }

    private void Idle()
    {
        actionTimer -= Time.deltaTime;
        if ( actionTimer < 0 )
        {
            float distance = path.GetDistance(previousIndex, pathIndex);
            actionTimer = actionDuration = distance / speed;
            state = MOVING;
        }
    }

    private void Turn()
    {
        actionTimer -= Time.deltaTime;
        if ( actionTimer < 0 )
        {
            transform.eulerAngles = new Vector3(0, 0, endAngle);
            // Idle Time
            if ( ( path.loopPath && pathIndex == 0 ) || pathIndex == path.LastIndex )
            {
                actionTimer = actionDuration = idleTime;
                state = IDLE;
            }
            // Movement Time
            else
            {
                float distance = path.GetDistance(previousIndex, pathIndex);
                actionTimer = actionDuration = distance / speed;
                state = MOVING;
            }
        }
        else
        {
            float lerp = movementCurve.Evaluate(actionTimer / actionDuration);
            transform.eulerAngles = new Vector3( 0, 0, Mathf.LerpAngle(endAngle, startAngle, lerp));

        }
    }

    private void Move()
    {
        actionTimer -= Time.deltaTime;
        if ( actionTimer < 0 )
        {
            previousIndex = pathIndex;

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
            startPosition = transform.position;
            endPosition = path[pathIndex];
            startAngle = transform.eulerAngles.z;
            endAngle = Mathf.Atan2(endPosition.y-startPosition.y, endPosition.x - startPosition.x) * Mathf.Rad2Deg - 90f;
            if( endAngle < 0 )
                endAngle += 360;
            actionTimer = actionDuration = Mathf.Abs(endAngle - startAngle) / turnSpeed;
            state = TURNING;
            //Debug.Log($"Turning from {startAngle} to {endAngle} over {actionTimer}");
        }
        else
        {
            float lerp = movementCurve.Evaluate(actionTimer / actionDuration);
            transform.position = Vector2.Lerp(endPosition, startPosition, lerp);
        }
    }
}
