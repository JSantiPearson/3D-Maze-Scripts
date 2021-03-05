using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    //Create variable for easy debugging
    public bool showDebug;

    //materials for maze meshes
    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;

    //contains maze data (walls and passages)
    public int[,] data
    {
        get; private set;
    }

    //Initializes the initial maze
    void Awake()
    {
        // default to walls surrounding a single empty cell for testing purposes
        data = new int[,]
        {
            {true, true, true},
            {true, false, true},
            {true, true, true}
        };
    }

    public void GenerateNewMaze(int size)
    {
        
    }
}
