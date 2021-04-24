using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Enemy> enemies;
    public bool requiresTransition = false;
    public Transform entrance;
    public Transform exit;

    public void StartRoom()
    {
        // Reset Enemies
        for ( int i = 0; i < enemies.Count; i++ )
        {
            enemies[i].Reset();
            enemies[i].frozen = false;
        }
        // Move Player
        GameManager.instance.player.Reset( entrance.position );
    }

    public void PauseRoom()
    {
        // Pause Enemies
        for ( int i = 0; i < enemies.Count; i++ )
        {
            enemies[i].frozen = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
