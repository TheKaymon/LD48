using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using TMPro;

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

    public string nextSceneName;
    public Canvas ui;
    public Player player;
    public List<Room> rooms = new List<Room>();
    public Transform cam;
    public Light2D globalLight;
    public AudioSource BGM;
    //public CanvasGroup introScreen;
    //public AnimationCurve introCurve;
    //public float introDuration;
    public GameObject popupMenu;
    public TextMeshProUGUI popUpText;
    public bool paused = true;

    private int currentRoom = 0;
    private Vector2 lastRoomPos;
    private Vector2 lastPlayerPos;
    private readonly float roomTransitionDuration = 1f;
    private float roomTransitionTimer;
    private float playerStartAngle;
    private float playerEndAngle;

    private List<Light2D> roomLights = new List<Light2D>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager Loaded");
        for ( int i = 1; i < rooms.Count; i++ )
        {
            rooms[i].gameObject.SetActive(false);
        }
        ui.gameObject.SetActive(true);
        cam.transform.position = rooms[currentRoom].transform.position;
        player.Reset(rooms[currentRoom].entrance.position, rooms[currentRoom].entrance.up);
        roomLights = rooms[currentRoom].lights;
        //roomTransitionTimer = introDuration;
        //RestartRoom();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Escape ) )
        {
            if( !paused )
            {
                paused = true;
                popUpText.SetText("Game Paused");
                popupMenu.SetActive(true);
            }
        }
        // Transitioning Between Rooms
        if( roomTransitionTimer > 0 )
        {
            roomTransitionTimer -= Time.deltaTime;

            //// Standard Transition
            //if ( !rooms[currentRoom].requiresTransition )
            //{
            if ( roomTransitionTimer < 0 )
            {
                if ( currentRoom > 0 )
                {
                    // Disable Last Room
                    rooms[currentRoom - 1].gameObject.SetActive(false);
                }
                //else
                //{
                //    introScreen.gameObject.SetActive(false);
                //}
                // Start Next Room
                cam.position = rooms[currentRoom].transform.position;
                RestartRoom();
            }
            else
            {
                if ( currentRoom > 0 )
                {
                    float lerp = roomTransitionTimer / roomTransitionDuration;
                    cam.position = Vector2.Lerp(rooms[currentRoom].transform.position, lastRoomPos, lerp);
                    player.transform.position = Vector2.Lerp(rooms[currentRoom].entrance.position, lastPlayerPos, lerp);
                    float angle = Mathf.LerpAngle(playerEndAngle, playerStartAngle, lerp);
                    player.sprite.eulerAngles = new Vector3(0, 0, angle);
                }
                //else
                //{
                //    float lerp = 1f - roomTransitionTimer / introDuration;
                //    introScreen.alpha = introCurve.Evaluate(lerp);
                //}
            }
            //}
            // Fade Transition
        }
        // Not Transitioning Between Rooms
        else
        {
            // Determine if Player is too Lit Up
            //for ( int i = 0; i < roomLights.Count; i++ )
            //{
            //    float radius = roomLights[i].pointLightOuterRadius;
            //}
        }
    }

    public void PlayerVisible( Enemy viewer )
    {
        Debug.Log("Uh oh, you've been spotted!");
        paused = true;
        // Show Dialog
        popUpText.SetText("You've been spotted!");
        popupMenu.SetActive(true);
    }

    public void RestartRoom()
    {
        popupMenu.SetActive(false);
        rooms[currentRoom].StartRoom();
        player.Ready();
        if( !BGM.isPlaying )
        {
            BGM.Play();
        }
        paused = false;
    }

    public void ExitRoom()
    {
        Debug.Log($"Exiting from room {currentRoom}");
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

            playerStartAngle = player.sprite.eulerAngles.z;
            // ToDo: Make Helper Function?
            playerEndAngle = Mathf.Atan2(rooms[currentRoom].entrance.up.y, rooms[currentRoom].entrance.up.x) * Mathf.Rad2Deg - 45f;
            if ( playerEndAngle < 0 )
                playerEndAngle += 360;
        }
        // Reached the End
        else
        {
            if( nextSceneName != "Win" )
            {
                currentRoom = 0;
                BGM.Stop();
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                BGM.Stop();
                WinGame();
            }
        }
    }

    public void HitEnemy( Enemy e )
    {

    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Private Functions //

    private void WinGame()
    {
        paused = true;
        Debug.Log("You won the game!");
        popUpText.SetText("Victory!");
        popupMenu.SetActive(true);
    }
}
