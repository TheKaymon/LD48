using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public Transform sprite;
    public Transform tipPoint;
    //public Transform pivotPoint;

    public Vector2 targetting;
    public Vector2 direction;
    public float angleThreshold = 100f;
    public float radius = 0.5f;

    public LineRenderer targetLine;
    //public Rigidbody2D body;

    public float speed;
    public LayerMask terrainMask;

    public AnimationCurve movementCurve;
    public AnimationCurve moveTurnCurve;
    public bool moving = false;
    public bool paused = false;
    public Vector2 start;
    public Vector2 destination;
    private float startAngle;
    private float endAngle;
    public float movingTimer;
    public float movingDuration;

    private Collider2D attachedTo;
    private Vector2 attachNormal = Vector2.up;
    //private Vector2 tipPos;

    public void Reset( Vector2 position, Vector2 direction )
    {
        moving = false;
        transform.position = position;
        attachNormal = direction;
        targetLine.SetPosition(0, tipPoint.position);
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

    //private void FlipSprite()
    //{
    //    float angle = Mathf.Atan2(attachNormal.y, attachNormal.x) * Mathf.Rad2Deg - 90f;
    //}

    // Start is called before the first frame update
    //void Start()
    //{
    //    //sprite = GetComponent<SpriteRenderer>();
    //}

    // Update is called once per frame
    void Update()
    {
        if ( !GameManager.instance.paused && !paused )
        {
            if ( !moving )
            {
                targetting = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                direction = targetting - (Vector2) transform.position;
                float angle = Vector2.Angle(attachNormal, direction);

                if ( angle < angleThreshold )
                {
                    float pointAngle = Vector2.SignedAngle(Vector2.up, direction);
                    sprite.eulerAngles = new Vector3(0, 0, pointAngle + 45);
                    targetLine.SetPosition(0, tipPoint.position);

                    targetLine.enabled = true;
                    RaycastHit2D hit = Physics2D.CircleCast(tipPoint.position, radius, direction, 100f, terrainMask.value);

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
                            destination = hit.point; // + radius * hit.normal;
                            startAngle = sprite.eulerAngles.z;
                            // ToDo: Make Helper Function?
                            endAngle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 45f;
                            if ( endAngle < 0 )
                                endAngle += 360;
                            movingTimer = movingDuration = hit.distance / speed;
                            attachedTo = hit.collider;
                            attachNormal = hit.normal;
                            //Debug.Log($"Moving From Angle {startAngle} to {endAngle}");
                        }
                        else
                        {
                            Debug.Log("Error, no target found!");
                        }
                    }
                }
                else
                {
                    targetLine.enabled = false;
                }
            }
            else
            {
                movingTimer -= Time.deltaTime;
                if ( movingTimer < 0 )
                {
                    moving = false;
                    transform.position = destination;
                    targetLine.SetPosition(0, tipPoint.position);
                    targetLine.enabled = true;
                }
                else
                {
                    float lerp = movementCurve.Evaluate(movingTimer / movingDuration);
                    transform.position = Vector2.Lerp(destination, start, lerp);
                    lerp = moveTurnCurve.Evaluate(movingTimer / movingDuration);
                    float rotation = Mathf.LerpAngle(endAngle, startAngle, lerp);
                    sprite.eulerAngles = new Vector3(0, 0, rotation); // + 45);
                }
                //body.MovePosition(newPosition);
            }
        }
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.CompareTag("Exit") )
        {
            Debug.Log($"Entered Exit Zone {collision.name}");
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
