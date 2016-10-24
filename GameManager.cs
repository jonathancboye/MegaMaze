using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static int currentLevel;
    public static Maze mazeToLoad;
    public static bool gameOver;

    private static GameManager instance = null;
    GameObject player;
    GameObject maze;
    Vector3 playerStartLocation;
    Grid grid;
    

    public static GameManager Instance {
        get { return instance; }
    }

    public void Restart() {
        DestroyMaze();
        currentLevel = 0;
        gameOver = false;
        Start();
    }

    void Awake() {
        currentLevel = 0;
        gameOver = false;
        if( instance ) {
            DestroyImmediate( gameObject );
            return;
        }
        instance = this;
        DontDestroyOnLoad( gameObject );
    }

    void Update() {
        if( gameOver ) {
            if( Input.GetKeyDown( KeyCode.R ) ) {
                Restart();
            }
            if( Input.GetKeyDown( KeyCode.Escape ) ) {
                Application.Quit();
            }
        }
    }

    // Use this for initialization
    void Start() {
        SceneManager.LoadScene( currentLevel );
    }

    public static void LoadNextLevel() {
        currentLevel++;
        SceneManager.LoadScene( currentLevel );
    }

    void OnLevelWasLoaded( int level ) {
       
        if( level == 1 ) {
            createMaze( mazeToLoad, 15, 15 );
        }
    }

    void createMaze( Maze mazeType, int rows, int columns ) {
        
        //create maze
        maze = new GameObject();
        maze.name = "Maze";
        grid = maze.AddComponent<Grid>();
        grid.tag = "Grid";
        grid.rows = rows;
        grid.columns = columns;
        grid.init( mazeType );


        //create player
        player = Instantiate( Resources.Load( "_prefabs/player" ) ) as GameObject;
        player.name = "Player";
        player.tag = "Player";
        playerStartLocation = grid.getCell( grid.rows - 1, grid.columns - 1 ).transform.position;
        playerStartLocation.y += Cell.height;
        player.transform.position = playerStartLocation;

        //setup camera
        Camera.main.orthographicSize = 3;
     
    }

    void DestroyMaze() {
        Destroy( maze );
        Destroy( player );
    }
}
