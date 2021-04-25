using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public Vector2 targetting;
    public Vector2 direction;
    public float radius = 0.5f;

    public LineRenderer targetLine;
    //public Rigidbody2D body;

    public float speed;
    public LayerMask terrainMask;

    public AnimationCurve movementCurve;
    public bool moving = false;
    public bool paused = false;
    public Vector2 start;
    public Vector2 destination;
    public float movingTimer;
    public float movingDuration;

    private Collider2D attachedTo;

    public void Reset( Vector2 position )
    {
        moving = false;
        transform.position = position;
        targetLine.SetPosition(0, position);
    }

    public void Pause()
    {
        paused = true;
        targetLine.enabled = false;
    }

    public void Ready()
    {
        paused = false;
        targetLine.enabled = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( !GameManager.instance.paused && !paused )
        {
            if ( !moving )
            {
                targetting = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                direction = targetting - (Vector2)transform.position;

                RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, direction, 100f, terrainMask.value);

                if ( hit && hit.distance < direction.magnitude )
                {
                    targetLine.SetPosition(1, hit.point);
                }
                else
                {
                    targetLine.SetPosition(1, targetting);
                }

                // If Left Click
                if ( Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() )
                {
                    //hit = Physics2D.CircleCast(transform.position, radius, direction, 100f, terrainMask.value);
                    if ( hit )
                    {
                        // Launch Player
                        targetLine.enabled = false;
                        moving = true;
                        start = transform.position;
                        destination = hit.point + radius * hit.normal;
                        movingTimer = movingDuration = hit.distance / speed;
                        attachedTo = hit.collider;
                    }
                    else
                    {
                        Debug.Log("Error, no target found!");
                    }
                }
            }
            else
            {
                movingTimer -= Time.deltaTime;
                if ( movingTimer < 0 )
                {
                    moving = false;
                    transform.position = destination;
                    targetLine.SetPosition(0, transform.position);
                    targetLine.enabled = true;
                }
                else
                {
                    float lerp = movementCurve.Evaluate(movingTimer / movingDuration);
                    transform.position = Vector2.Lerp(destination, start, lerp);
                }
                //body.MovePosition(newPosition);
            }
        }
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.CompareTag("Exit") )
        {
            GameManager.instance.ExitRoom();
        }
        else if ( collision.CompareTag("Entrance") )
        {

        }
        else if ( collision.CompareTag("Enemy") )
        {
            GameManager.instance.HitEnemy(collision.GetComponent<Enemy>());
        }
        else
        {
            Debug.Log($"Entered Trigger Zone {collision.name}");
        }
    }
}
