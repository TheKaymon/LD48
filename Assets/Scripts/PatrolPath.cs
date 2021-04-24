using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolPath : MonoBehaviour
{
    [SerializeField]
    public List<Vector2> pathPoints = new List<Vector2>();
    public int NumPoints => pathPoints.Count;
    public int LastIndex => pathPoints.Count - 1;
    public bool loopPath = false;

    private List<float> distances = new List<float>();

    // Public Functions
    public Vector2 this[int i]
    {
        get
        {
            return pathPoints[i];
        }
        set
        {
            pathPoints[i] = value;
        }
    }

    public float GetDistance( int firstIndex, int secondIndex )
    {
        if( loopPath && ((firstIndex == 0 && secondIndex == LastIndex ) || ( secondIndex == 0 && firstIndex == LastIndex )) )
        {
            return distances[LastIndex];
        }
        return distances[Mathf.Min(firstIndex, secondIndex)];
    }

    // Start is called before the first frame update
    void Start()
    {
        // Cache Distances between points
        if( NumPoints > 1 )
        {
            float dist = 0;
            for ( int i = 0; i < LastIndex; i++ )
            {
                dist = Vector2.Distance(pathPoints[i], pathPoints[i + 1]);
                distances.Add(dist);
            }
            if( loopPath )
            {
                dist = Vector2.Distance(pathPoints[0], pathPoints[LastIndex]);
                distances.Add(dist);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
