using System;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]               // Creates instance of MazeConstructor component

public class GameController : MonoBehaviour
{
    private MazeConstructor generator;

    void Start()
    {
        generator = GetComponent<MazeConstructor>();      // Creates a maze generator
        generator.GenerateNewMaze(17);
    }
}
