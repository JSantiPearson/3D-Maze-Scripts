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

  private List<Point> getAdjacentWalls(Point point){
    List<Point> adjacent = new List<Point>();

    Point left = new Point(point.x-1, point.y);
    Point up = new Point(point.x, point.y-1);
    Point right = new Point(point.x+1, point.y);
    Point down = new Point(point.x, point.y+1);

    if (withinMaze(left) && isWall(left)){
      adjacent.Add(left);
    }
    if (withinMaze(up) && isWall(up)){
      adjacent.Add(up);
    }
    if (withinMaze(right) && isWall(right)){
      adjacent.Add(right);
    }
    if (withinMaze(down) && isWall(down)){
      adjacent.Add(down);
    }
    return adjacent;
  }

  private List<Point> getAdjacent(Point point){
    List<Point> adjacent = new List<Point>();

    Point left = new Point(point.x-1, point.y);
    Point up = new Point(point.x, point.y-1);
    Point right = new Point(point.x+1, point.y);
    Point down = new Point(point.x, point.y+1);

    if (withinMaze(left)){
      adjacent.Add(left);
    }
    if (withinMaze(up)){
      adjacent.Add(up);
    }
    if (withinMaze(right)){
      adjacent.Add(right);
    }
    if (withinMaze(down)){
      adjacent.Add(down);
    }
    return adjacent;
  }

  private List<Point> addWalls(List<Point> walls, List<Point> newWalls){
    foreach (Point wall in newWalls){
      walls.Add(wall);
    }
    return walls;
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

  private bool tooSparse(Point choice){
    //choice is left or right of current
    List<Point> adjacent = getAdjacent(choice);
    int numPassages = 0;
    foreach (Point point in adjacent){
      if (!isWall(point)){
        numPassages++;
      }
    }
    if (numPassages > 1){
      return true;
    }
    else {
      return false;
    }
  }

  private bool connectsEnd(List<Point> endWalls, Point choice){
    if (endWalls.Contains(choice)){
      return true;
    }
    else {
      return false;
    }
  }

  private void createPath(Point start, Point end){
    List<Point> walls = new List<Point>();
    List<Point> endWalls = getAdjacentWalls(end);
    walls = addWalls(walls, getAdjacentWalls(start));
    Point current = start;
    while (walls.Count > 0){
      int choiceIndex = (Random.Range(0, walls.Count));
      Point choice = walls[choiceIndex];
      if (!tooSparse(choice) || (!willBeWide(choice) && connectsEnd(endWalls, choice))){
        maze[choice.x, choice.y] = false;
        walls = addWalls(walls, getAdjacentWalls(choice));
      }
      current = choice;
      walls.RemoveAt(choiceIndex);
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
    createPath(end, start);

    bool[,] completeMaze = new bool[size+1, size+1];
    for (int i = 0; i <= size; i++){
      for (int j = 0; j <= size; j++){
        if (i == 0 || j == 0 || i == size || j == size){
          completeMaze[i, j] = true;
        }
        else {
          completeMaze[i,j] = maze[i-1, j-1];
        }
      }
    }
    completeMaze[0, 1] = false;
    completeMaze[size-1, size-0] = false;
    return completeMaze;
  }
}
