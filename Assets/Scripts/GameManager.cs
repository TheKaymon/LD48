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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerVisible( Enemy viewer )
    {
        Debug.Log("Uh oh, you've been spotted!");
    }
}
