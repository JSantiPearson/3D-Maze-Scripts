using System.Collections.Generic;
using UnityEngine;

struct Point
{
  public int x, y;
  public bool visited;
  public Point(int px, int py)
     {
         x = px;
         y = py;
         visited = false;
     }
}

public class MazeDataGenerator
{
  HashSet<Point> visited;

  private bool[,] recurSearch(bool[,] maze, HashSet<Point> visited, Point point){
    //mark the current point as visited
    visited.Add(point);
    maze[point.x, point.y] = false;

    List<Point> pathChoices = new List<Point>();

    Point left = new Point(point.x-2, point.y);
    Point up = new Point(point.x, point.y-2);
    Point right = new Point(point.x+2, point.y);
    Point down = new Point(point.x, point.y+2);

    //if a point adjacent to the current point is valid and unvisted, add it to choices
    if (point.x - 2 >= 0 && !visited.Contains(left)){
      pathChoices.Add(left);
    }
    if (point.y - 2 >= 0 && !visited.Contains(up)){
      pathChoices.Add(up);
    }
    if (point.x + 2 < maze.GetLength(0)-1 && !visited.Contains(right)){
      pathChoices.Add(right);
    }
    if (point.y + 2 < maze.GetLength(0)-1 && !visited.Contains(down)){
      pathChoices.Add(down);
    }
    //if there are no paths to choose from, return
    if (pathChoices.Count == 0){
      return maze;
    }

    Point choice = pathChoices[(Random.Range(0, pathChoices.Count))]; //choose a random number between 0 and list size.
    Point wall;
    //chosen point is left of current point
    if (point.x > choice.x){
      //visit and remove adjacent wall
      wall = new Point(point.x - 1, point.y);
    }
    //chosen point is right of current point
    else if(point.x < choice.x){
      wall = new Point(point.x + 1, point.y);
    }
    //chosen point is below current point
    else if(point.y < choice.y){
      wall = new Point(point.x, point.y + 1);
    }
    //chosen point is below current point
    else {
      wall = new Point(point.x, point.y - 1);
    }
    visited.Add(wall);
    maze[wall.x, wall.y] = false;
    return recurSearch(maze, visited, choice);
  }

  private bool[,] createPath(bool[,] maze, HashSet<Point> visited, Point start){
    maze = recurSearch(maze, visited, start);
    return maze;
  }

  public bool[,] FromDimensions(int size) //takes the size of the maze and generates the placement of walls
  {
    bool[,] maze = new bool[size, size];
    for (int i = 0; i < size; i++){
      for (int j = 0; j < size; j++){
        maze[i,j] = true;
      }
    }

    HashSet<Point> visited = new HashSet<Point>();

    Point start = new Point(0, 0);
    Point end = new Point(size-1, size-1);
    visited = new HashSet<Point>();
    maze = createPath(maze, visited, start);
    Debug.Log(visited.Count);
    maze = createPath(maze, visited, end);
    return maze;
  }
}
