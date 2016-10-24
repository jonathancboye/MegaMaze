using UnityEngine;
using System.Collections;

public class button_binary : MonoBehaviour
{
    public void Clicked() {
        GameManager.mazeToLoad = Maze.BINARYTREE;
        GameManager.LoadNextLevel();
    }

}
