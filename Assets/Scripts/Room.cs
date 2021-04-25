using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Room : MonoBehaviour
{
    public Transform enemyParent;
    public Transform lightParent;
    public bool requiresTransition = false;
    public Transform entrance;
    public Transform exit;

    [HideInInspector]
    public List<Enemy> enemies = new List<Enemy>();
    [HideInInspector]
    public List<Light2D> lights = new List<Light2D>();

    public void StartRoom()
    {
        // Reset Enemies
        if ( enemies.Count > 0 )
        {
            for ( int i = 0; i < enemies.Count; i++ )
            {
                enemies[i].Reset();
                enemies[i].frozen = false;
            }
        }
        // Move Player
        GameManager.instance.player.Reset( entrance.position, entrance.up );
    }

    public void PauseRoom()
    {
        // Pause Enemies
        if ( enemies.Count > 0 )
        {
            for ( int i = 0; i < enemies.Count; i++ )
            {
                enemies[i].frozen = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyParent.GetComponentsInChildren(enemies);
        lightParent.GetComponentsInChildren(lights);
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
}
