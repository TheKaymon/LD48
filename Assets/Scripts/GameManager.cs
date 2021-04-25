using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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

    public Canvas ui;
    public Player player;
    public List<Room> rooms = new List<Room>();
    public Transform cam;
    public bool paused;

    private int currentRoom = 0;
    private Vector2 lastRoomPos;
    private Vector2 lastPlayerPos;
    private readonly float roomTransitionDuration = 1f;
    private float roomTransitionTimer;

    private List<Light2D> roomLights = new List<Light2D>();

    // Start is called before the first frame update
    void Start()
    {
        ui.gameObject.SetActive(true);
        cam.transform.position = rooms[currentRoom].transform.position;
        roomLights = rooms[currentRoom].lights;
        roomTransitionTimer = -1f;
        RestartRoom();
    }

    // Update is called once per frame
    void Update()
    {
        // Transitioning Between Rooms
        if( roomTransitionTimer > 0 )
        {
            roomTransitionTimer -= Time.deltaTime;

            // Standard Transition
            if ( !rooms[currentRoom].requiresTransition )
            {
                if ( roomTransitionTimer < 0 )
                {
                    // Disable Last Room
                    rooms[currentRoom - 1].gameObject.SetActive(false);
                    // Start Next Room
                    cam.position = rooms[currentRoom].transform.position;
                    RestartRoom();
                    player.Ready();
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
        // Not Transitioning Between Rooms
        else
        {
            // Determine if Player is too Lit Up
            for ( int i = 0; i < roomLights.Count; i++ )
            {
                float radius = roomLights[i].pointLightOuterRadius;
            }
        }
    }

    public void PlayerVisible( Enemy viewer )
    {
        Debug.Log("Uh oh, you've been spotted!");
        paused = true;
        // Show Dialog
    }

    public void RestartRoom()
    {
        rooms[currentRoom].StartRoom();
        paused = false;
    }

    public void ExitRoom()
    {
        // More Rooms to Go
        if( currentRoom < rooms.Count - 1 )
        {
            // Pause Current Room - Enemies
            rooms[currentRoom].PauseRoom();
            player.Pause();

            // Pan Camera
            lastRoomPos = rooms[currentRoom].transform.position;
            lastPlayerPos = player.transform.position;
            roomTransitionTimer = roomTransitionDuration;

            // Enable and Pause Next Room
            currentRoom++;
            rooms[currentRoom].gameObject.SetActive(true);
            rooms[currentRoom].PauseRoom();
            roomLights = rooms[currentRoom].lights;
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

    public void QuitToMenu()
    {

    }

    // Private Functions //

    private void WinGame()
    {
        Debug.Log("You won the game!");
    }
}
