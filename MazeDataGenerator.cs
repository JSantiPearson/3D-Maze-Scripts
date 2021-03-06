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

  private void addSpot(Point point){
    maze[point.x, point.y] = false;
    createPath(point);
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

  private List<Point> addWalls(List<Point> walls, List<Point> newWalls){
    foreach (Point wall in newWalls){
      walls.Add(wall);
    }
    return walls;
  }

  private int numPassages(Point current, Point choice){
    //choice is left or right of current
    int numPassages = 0;
    Point up = new Point(choice.x, choice.y-1);
    Point down = new Point(choice.x, choice.y+1);
    Point left = new Point(choice.x-1, choice.y);
    Point right = new Point(choice.x+1, choice.y);
    if (withinMaze(up) && !isWall(up)){
      numPassages++;
    }
    if (withinMaze(down) && !isWall(down)){
      numPassages++;
    }
    if (withinMaze(left) && !isWall(left)){
      numPassages++;
    }
    if (withinMaze(right) && !isWall(right)){
      numPassages++;
    }
    return numPassages;
  }

  private void createPath(Point start){
    List<Point> walls = new List<Point>();
    walls = addWalls(walls, getAdjacentWalls(start));
    Point current = start;
    while (walls.Count > 0){
      int choiceIndex = (Random.Range(0, walls.Count));
      Point choice = walls[choiceIndex];
      if (numPassages(current, choice) < 2){
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
    createPath(end);
    return maze;
  }
}
