using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    //Create variable for easy debugging
    public bool showDebug;

    // private generator for maze data
    private MazeDataGenerator dataGenerator;

    //materials for maze meshes
    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;

    //contains maze data (walls and passages)
    public bool[,] data
    {
        get; private set;
    }

    //Initializes the initial maze
    void Awake()
    {
      dataGenerator = new MazeDataGenerator();
    }

    // Displays maze data when debugging is on
    void OnGUI()
    {
      //return if debug is false
      if (!showDebug)
      {
          return;
      }

      //get the size of the sides of the maze (they will always be equal)
      bool[,] maze = data;
      int size = maze.GetUpperBound(0);

      string msg = "";

      //check the stored value of each element in the array and print a visual to display the maze
      for (int i = 0; i <= size; i++)
      {
          for (int j = 0; j <= size; j++)
          {
              if (maze[i, j] == false)
              {
                  msg += "....";
              }
              else
              {
                  msg += "==";
              }
          }
          msg += "\n";
      }

      //prints the string
      GUI.Label(new Rect(200, 200, 5000, 5000), msg);
    }

    public void GenerateNewMaze(int size)
    {
      data = dataGenerator.FromDimensions(size);
    }
}
