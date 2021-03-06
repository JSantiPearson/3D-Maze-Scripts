using System.Collections.Generic;
using UnityEngine;

struct Point
{
  public int x, y;
  public Point(int px, int py)
     {
         x = px;
         y = py;
     }
}

public class MazeDataGenerator
{

  bool[,] maze;

  private bool isWall(Point point){
    return maze[point.x, point.y];
  }

  // Returns true if the point is within the maze bounds.
  private bool withinMaze(Point point){
    int size = maze.GetLength(0);
    if (point.x < 0 || point.x >= size || point.y < 0 || point.y >= size){
      return false;
    }
    else {
      return true;
    }
  }

  // Returns true if the point would cause a wide hallway to occur.
  private bool willBeWide(Point point){
    Point top = new Point(point.x, point.y-1);
    Point topRight = new Point(point.x+1, point.y-1);
    Point right = new Point(point.x+1, point.y);
    Point downRight = new Point(point.x+1, point.y+1);
    Point down = new Point(point.x, point.y+1);
    Point downLeft = new Point(point.x-1, point.y+1);
    Point left = new Point(point.x-1, point.y);
    Point topLeft = new Point(point.x-1, point.y-1);

    // Check if the corners and their adjacent points are passages. If they are, it will create a 2-wide.
    if (withinMaze(top) && withinMaze(topRight) && withinMaze(right) &&
      !isWall(top) && !isWall(topRight) && !isWall(right)){
       return true;
    }

    if (withinMaze(right) && withinMaze(downRight) && withinMaze(down) &&
      !isWall(right) && !isWall(downRight) && !isWall(down)){
        return true;
    }

    if (withinMaze(down) && withinMaze(downLeft) && withinMaze(left) &&
      !isWall(down) && !isWall(downLeft) && !isWall(left)){
        return true;
    }

    if (withinMaze(left) && withinMaze(topLeft) && withinMaze(top) &&
      !isWall(left) && !isWall(topLeft) && !isWall(top)){
        return true;
    }
    return false;
  }

  // Returns true of the point is within the maze bounds and would not cause a wide hall.
  private bool isValidPath(Point point){
    if (!withinMaze(point)){
      return false;
    }
    if (willBeWide(point)){
      return false;
    }
    return true;
  }

  private void addSpot(Point point){
    maze[point.x, point.y] = false;
    createPath(point);
  }

  private void createPath(Point point){
    List<Point> pathChoices = new List<Point>();

    Point left = new Point(point.x-1, point.y);
    Point up = new Point(point.x, point.y-1);
    Point right = new Point(point.x+1, point.y);
    Point down = new Point(point.x, point.y+1);

    if (isValidPath(left) && isWall(left)){
      pathChoices.Add(left);
    }
    if (isValidPath(up) && isWall(up)){
      pathChoices.Add(up);
    }
    if (isValidPath(right) && isWall(right)){
      pathChoices.Add(right);
    }
    if (isValidPath(down) && isWall(down)){
      pathChoices.Add(down);
    }
    if (pathChoices.Count > 0){
      int choiceIndex = (Random.Range(0, pathChoices.Count));
      Point choice = pathChoices[choiceIndex]; //choose a random number between 0 and list size.
      addSpot(choice);
    }
  }

  public bool[,] FromDimensions(int size) //takes the size of the maze and generates the placement of walls
  {
    maze = new bool[size, size];
    for (int i = 0; i < size; i++){
      for (int j = 0; j < size; j++){
        maze[i,j] = true;
      }
    }
    Point start = new Point(0, 0);
    Point end = new Point(size-1, size-1);
    maze[0, 0] = false;
    maze[size-1, size-1] = false;
    createPath(start);
    createPath(end);
    return maze;
  }
}
