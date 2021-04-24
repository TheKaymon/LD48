using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance;
    private void Awake()
    {
        if( instance != null )
        {
            Debug.Log("Error: Multiple GameManagers!");
        }
        instance = this;
    }

    public Player player;
    public List<Room> rooms = new List<Room>();
    public Transform cam;
    public bool paused;

    private int currentRoom = 0;
    private Vector2 lastRoomPos;
    private Vector2 lastPlayerPos;
    private readonly float roomTransitionDuration = 1f;
    private float roomTransitionTimer;


    // Start is called before the first frame update
    void Start()
    {
        cam.transform.position = rooms[currentRoom].transform.position;
        roomTransitionTimer = -1f;
        RestartRoom();
    }

    // Update is called once per frame
    void Update()
    {
        if( roomTransitionTimer > 0 )
        {
            roomTransitionTimer -= Time.deltaTime;

            // Standard Transition
            if ( !rooms[currentRoom].requiresTransition )
            {
                if ( roomTransitionTimer < 0 )
                {
                    // Start Next Room
                    cam.position = rooms[currentRoom].transform.position;
                    RestartRoom();
                }
                else
                {
                    float lerp = roomTransitionTimer / roomTransitionDuration;
                    cam.position = Vector2.Lerp(rooms[currentRoom].transform.position, lastRoomPos, lerp);
                    player.transform.position = Vector2.Lerp(rooms[currentRoom].entrance.position, lastPlayerPos, lerp);
                }
            }
            // Fade Transition
        }
    }

    public void PlayerVisible( Enemy viewer )
    {
        Debug.Log("Uh oh, you've been spotted!");
    }

    public void RestartRoom()
    {
        rooms[currentRoom].StartRoom();
    }

    public void ExitRoom()
    {
        // More Rooms to Go
        if( currentRoom < rooms.Count - 1 )
        {
            // Pause Current Room - Enemies
            rooms[currentRoom].PauseRoom();
            // Pan Camera
            lastRoomPos = rooms[currentRoom].transform.position;
            lastPlayerPos = player.transform.position;
            currentRoom++;

            roomTransitionTimer = roomTransitionDuration;
        }
        // Reached the End
        else
        {
            WinGame();
        }
    }

    public void HitEnemy( Enemy e )
    {

    }

    private void WinGame()
    {
        Debug.Log("You won the game!");
    }
}
