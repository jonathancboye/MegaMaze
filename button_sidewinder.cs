using UnityEngine;
using System.Collections;

public class button_sidewinder : MonoBehaviour
{

    public void Clicked() {
        GameManager.mazeToLoad = Maze.SIDEWINDER;
        GameManager.LoadNextLevel();
    }
}
