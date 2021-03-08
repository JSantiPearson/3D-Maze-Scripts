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

/**
* Generates a 2D bool array that contains the data of the maze
**/
public class MazeDataGenerator
{
  // global maze 2D array
  bool[,] maze;

  // Return the value of a given point in the maze
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

  // returns a list of adjacent valid points that are walls when given a point on the maze
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

  // returns a list of adjacent valid points when given a point on the maze
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

  // takes a given list of walls and pushes each wall to another list.
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

  // checks the adjacent points for passageways. If there are more than one, return true.
  private bool tooSparse(Point choice){
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

  // Takes the end point's neighbors and some point, and if there is a match then the point connects to the end.
  private bool connectsEnd(Point end, Point choice){
    List<Point> endPoints = getAdjacent(end);
    foreach (Point point in endPoints){
      if (choice.x == point.x && choice.y == point.y){
        return true;
      }
    }
    return false;
  }

  // Takes a start point and an end point, then creates pathways in the maze using Prim's algoritm
  private void createPath(Point start, Point end){
    List<Point> walls = new List<Point>();

    //walls adjacent to start are added to walls list
    walls = addWalls(walls, getAdjacentWalls(start));
    Point current = start;

    //while there are still walls to examine, fill out the maze
    while (walls.Count > 0){
      //pick a random wall in our list
      int choiceIndex = (Random.Range(0, walls.Count));
      Point choice = walls[choiceIndex];
      //if the maze isn't too sparse or the wall connects to the end and wouldn't cause a 2-wide
      if (!tooSparse(choice) || (!willBeWide(choice) && connectsEnd(end, choice))){
        // turn the wall into a passage and add its adjacent walls to the list
        maze[choice.x, choice.y] = false;
        walls = addWalls(walls, getAdjacentWalls(choice));
      }
      // change the current point on the maze to our choice and remove the choice from the wall list
      current = choice;
      walls.RemoveAt(choiceIndex);
    }
  }

  // takes the maze and creates a border of walls around it except at the entrance and exit.
  private bool[,] fenceMaze(){
    int size = maze.GetLength(0);
    size += 2;
    bool[,] completeMaze = new bool[size, size];

    for (int i = 0; i < size; i++){
      for (int j = 0; j < size; j++){
        if (i != 0 && j != 0 && i != size-1 && j != size-1){
          completeMaze[i, j] = maze[i-1, j-1];
        }
        else {
          completeMaze[i, j] = true;
        }
      }
    }

    completeMaze[0, 1] = false;
    completeMaze[size-1, size-2] = false;

    return completeMaze;
  }

  // takes the size of the maze and generates it
  public bool[,] FromDimensions(int size)
  {
    // creates solid block of wall with length and width equal to size
    maze = new bool[size, size];
    for (int i = 0; i < size; i++){
      for (int j = 0; j < size; j++){
        maze[i,j] = true;
      }
    }
    // choose start and end points
    Point start = new Point(0, 0);
    Point end = new Point(size-1, size-1);

    // start and end are passages
    maze[start.x, start.y] = false;
    maze[end.x, end.y] = false;

    //create a path from the start, then a path from the end. Very rarely causes start to be blocked...
    createPath(end, start);
    createPath(start, end);

    return fenceMaze();
  }
}
