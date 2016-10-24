using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Maze { BINARYTREE, SIDEWINDER }
public class Grid : MonoBehaviour
{
    GameObject[,] cells;
    GameObject entry, exit;
    Distances distances;

    int _rows, _columns;

    // Use this for initialization

    public int rows {
        get { return _rows; }
        set { _rows = value; }
    }

    public int columns {
        get { return _columns; }
        set { _columns = value; }
    }

    public void init( Maze mazeType ) {
        cells = new GameObject[ _rows, _columns ];
        prepareGrid();
        configure_cells();

        switch( mazeType ) {
            case Maze.BINARYTREE:
                MazeAlgo.BinaryMaze( this );
                break;
            case Maze.SIDEWINDER:
                MazeAlgo.SideWinderMaze( this );
                break;
            default:
                Debug.Log( "Don't know this type of Maze" );
                break;
        }

        cutMaze();    
        openMaze();
        buildMazeEdgeCollider();
        solveMaze();
    }

    void buildMazeEdgeCollider() {
        EdgeCollider2D leftEdgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        EdgeCollider2D rightEdgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        EdgeCollider2D top_left_boundry = gameObject.AddComponent<EdgeCollider2D>();
        EdgeCollider2D top_right_boundry = gameObject.AddComponent<EdgeCollider2D>();
        List<KeyValuePair<Vector2, Vector2>> edges = new List<KeyValuePair<Vector2, Vector2>>();
        GameObject start = cells[ _rows - 1, _columns - 1].GetComponent<Cell>().northEastWall;

        // Start points for right edge collider
        GameObject bottomRight_southEastWall = cells[ 0, _columns - 1 ].GetComponent<Cell>().southEastWall;
        GameObject bottomLeft_southWestWall = cells[ 0, 0 ].GetComponent<Cell>().southWestWall;
        Vector2 rightStartPoint = new Vector2( start.transform.position.x + 0.5f,
                    start.transform.position.y + 0.5f );
        Vector2 point1 = new Vector2( start.transform.position.x - 0.5f,
                   start.transform.position.y + 0.5f );
        Vector2 point2 = new Vector2( start.transform.position.x - 0.5f,
                    start.transform.position.y - 0.5f );
        Vector2 bottomRight = new Vector2( bottomRight_southEastWall.transform.position.x + 0.5f,
                    bottomRight_southEastWall.transform.position.y - 0.5f );
        Vector2 bottomLeft = new Vector2( bottomLeft_southWestWall.transform.position.x - 0.5f,
                    bottomLeft_southWestWall.transform.position.y - 0.5f );
        Vector2 bottomLeft_topLeftCorner = new Vector2( bottomLeft_southWestWall.transform.position.x - 0.5f,
                    bottomLeft_southWestWall.transform.position.y + 0.5f );
        Vector2 bottomLeft_topRightCorner = new Vector2( bottomLeft_southWestWall.transform.position.x + 0.5f,
                    bottomLeft_southWestWall.transform.position.y + 0.5f );
        edges.Add( new KeyValuePair<Vector2, Vector2>( rightStartPoint, point1 ) );
        edges.Add( new KeyValuePair<Vector2, Vector2>( point1, point2 ) );
        top_right_boundry.points = new Vector2[] { rightStartPoint, bottomRight, bottomLeft, bottomLeft_topLeftCorner,
                bottomLeft_topRightCorner};


        // Start points for left edge collider
        Vector2 leftStartPoint = new Vector2( start.transform.position.x - 1.5f,
            start.transform.position.y + 0.5f );
        GameObject topLeft_northWestWall = cells[ _rows - 1, 0 ].GetComponent<Cell>().northWestWall;
        GameObject bottomLeft_northWestWall = cells[ 0, 0 ].GetComponent<Cell>().northWestWall;
        point1 = new Vector2( start.transform.position.x - 1.5f,
                    start.transform.position.y - 0.5f );
        Vector2 topLeftPoint = new Vector2( topLeft_northWestWall.transform.position.x - 0.5f,
                    topLeft_northWestWall.transform.position.y + 0.5f );
        Vector2 bottomLeftPoint = new Vector2( bottomLeft_northWestWall.transform.position.x - 0.5f,
                    bottomLeft_northWestWall.transform.position.y - 0.5f );
        Vector2 bottomLeftPoint_rightCorner = new Vector2( bottomLeft_northWestWall.transform.position.x + 0.5f,
                    bottomLeft_northWestWall.transform.position.y - 0.5f );
        Vector2 bottomLeftPoint_topRightCorner = new Vector2( bottomLeft_northWestWall.transform.position.x + 0.5f,
                    bottomLeft_northWestWall.transform.position.y + 0.5f );
        edges.Add( new KeyValuePair<Vector2, Vector2>( leftStartPoint, point1 ) );
        top_left_boundry.points = new Vector2[] { leftStartPoint, topLeftPoint, bottomLeftPoint, bottomLeftPoint_rightCorner};

        // find rest of the edges in maze
        findEdges( edges );

        // create paths to build edge colliders
        leftEdgeCollider.points = createEdgePath( edges, leftStartPoint );
        rightEdgeCollider.points = createEdgePath( edges, rightStartPoint );
        
    }
    
    // Find all edges in maze
    void findEdges( List<KeyValuePair<Vector2, Vector2>> edges ) {

        Cell currentCell;
      
        for( int i = 0 ; i < _rows ; i++ ) {
            for( int j = 0 ; j < _columns ; j++ ) {
                currentCell = cells[ i, j ].GetComponent<Cell>();

                //find north edges
                if( currentCell.north && currentCell.islinked( currentCell.north ) ) {
                    Vector3 block_northEast = currentCell.gameObject.GetComponent<Cell>().northEastWall.transform.position;
                    addEdge( edges, block_northEast, -0.5f, 0.5f, -0.5f, -0.5f );
                    addEdge( edges, block_northEast, -1.5f, -0.5f, -1.5f, 0.5f );
                } else {
                    if( currentCell.gameObject != entry ) {
                        Vector3 block_north = currentCell.gameObject.GetComponent<Cell>().northWall.transform.position;
                        addEdge( edges, block_north, 0.5f, -0.5f, -0.5f, -0.5f );
                    }
                }

                //find south edges
                if( currentCell.south && currentCell.islinked( currentCell.south ) ) {
                    Vector3 block_southEast = currentCell.gameObject.GetComponent<Cell>().southEastWall.transform.position;
                    addEdge( edges, block_southEast, -0.5f, 0.5f, -0.5f, -0.5f );
                    addEdge( edges, block_southEast, -1.5f, 0.5f, -1.5f, -0.5f );
                } else {
                    Vector3 block_south = currentCell.gameObject.GetComponent<Cell>().southWall.transform.position;
                    addEdge( edges, block_south, -0.5f, 0.5f, 0.5f, 0.5f );
                }

                //find west edges
                if( currentCell.west && currentCell.islinked( currentCell.west ) ) {
                    Vector3 block_west = currentCell.gameObject.GetComponent<Cell>().westWall.transform.position;
                    addEdge( edges, block_west, 0.5f, 0.5f, -0.5f, 0.5f );
                    addEdge( edges, block_west, 0.5f, -0.5f, -0.5f, -0.5f );
                } else {
                    if( currentCell.gameObject != exit ) {
                        Vector3 block_west = currentCell.gameObject.GetComponent<Cell>().westWall.transform.position;
                        addEdge( edges, block_west, 0.5f, 0.5f, 0.5f, -0.5f );
                    }
                }

                //find east edges
                if( currentCell.east && currentCell.islinked( currentCell.east ) ) {
                    Vector3 block_east = currentCell.gameObject.GetComponent<Cell>().eastWall.transform.position;
                    addEdge( edges, block_east, -0.5f, 0.5f, 0.5f, 0.5f );
                    addEdge( edges, block_east, -0.5f, -0.5f, 0.5f, -0.5f );
                } else {
                    Vector3 block_east = currentCell.gameObject.GetComponent<Cell>().eastWall.transform.position;
                    addEdge( edges, block_east, -0.5f, 0.5f, -0.5f, -0.5f );
                }
            }
        }
    }

    void addEdge(List<KeyValuePair<Vector2, Vector2>> edges, Vector3 block, float xOffset_p1, float yOffset_p1, float xOffset_p2, float yOffset_p2 ) {
        Vector2 point1 = new Vector2( block.x + xOffset_p1, block.y + yOffset_p1 );
        Vector2 point2 = new Vector2( block.x + xOffset_p2, block.y + yOffset_p2 );
        edges.Add( new KeyValuePair<Vector2, Vector2>( point1, point2 ) );
    }

    Vector2[] createEdgePath(List<KeyValuePair<Vector2,Vector2>> edges, Vector2 startPoint) {
        List<Vector2> edgepath = new List<Vector2>();
        KeyValuePair<Vector2, Vector2> currentEdge = edges[0];
        KeyValuePair<Vector2, Vector2 > def = new KeyValuePair<Vector2, Vector2>(new Vector2( 0f, 0f), new Vector2(0f, 0f) );
        Vector2 nextPoint = currentEdge.Value;
        edgepath.Add( currentEdge.Key );
        edges.Remove( currentEdge );

        while(edges.Count >= 0) {
            edgepath.Add( nextPoint );
            if( !def.Equals( currentEdge = edges.Find( x => x.Key == nextPoint ) ) ) {
                nextPoint = currentEdge.Value;
            } else if( !def.Equals( currentEdge = edges.Find( x => x.Value == nextPoint ) ) ) {
                nextPoint = currentEdge.Key;
            } else {
                break;
            }
            edges.Remove( currentEdge );
        }

        return edgepath.ToArray();
    }

    void prepareGrid() {

        for( int i = 0 ; i < _rows ; i++ ) {
            for( int j = 0 ; j < _columns ; j++ ) {
                GameObject gameobject = Instantiate( Resources.Load( "_prefabs/cell" ) ) as GameObject;
                gameobject.transform.position = new Vector3( j * Cell.width, i * Cell.height, 0 );
                gameobject.transform.parent = transform;
                cells[ i, j ] = gameobject;
                gameobject.name = "cell(" + i + "," + j + ")";
            }
        }
    }

    void configure_cells() {

        for( int i = 0 ; i < _rows ; i++ ) {
            for( int j = 0 ; j < _columns ; j++ ) {
                Cell cell = cells[ i, j ].GetComponent<Cell>();
                cell.row = i;
                cell.column = j;
                cell.north = getCell( i + 1, j );
                cell.south = getCell( i - 1, j );
                cell.east = getCell( i, j + 1 );
                cell.west = getCell( i, j - 1 );
            }
        }
    }

    public void openMaze() {
        exit = getCell( 0, 0 );
        entry = getCell( _rows - 1, _columns - 1 );
        Destroy( exit.GetComponent<Cell>().westWall );
        Destroy( entry.GetComponent<Cell>().northWall );
    }
    public void cutMaze() {

        foreach( GameObject gameobject in each_cell() ) {
            gameobject.GetComponent<Cell>().DestroyLinks();
        }
    }

    public void solveMaze() {
        //Show solution
        distances = MazeAlgo.Dijkstra( entry );
    }

    public GameObject getCell( int row, int column ) {
        return ( row < _rows && row >= 0 ) && ( column < _columns && column >= 0 ) ?
            cells[ row, column ] : null;
    }

    public GameObject getRandomCell() {
        return cells[ Random.Range( 0, _rows ), Random.Range( 0, _columns ) ];
    }

    public int getSize() {
        return _rows * _columns;
    }

    public IEnumerable each_row() {

        for( int i = 0 ; i < _rows ; i++ ) {
            GameObject[] gridRow = new GameObject[ _columns ];

            for( int j = 0 ; j < _columns ; j++ ) {
                gridRow[ j ] = cells[ i, j ];
            }
            yield return gridRow;
        }
    }

    public IEnumerable each_cell() {
        for( int i = 0 ; i < _rows ; i++ ) {
            for( int j = 0 ; j < _columns ; j++ ) {
                yield return cells[ i, j ];
            }
        }
    }
}