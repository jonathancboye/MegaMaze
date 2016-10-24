using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeAlgo
{
    public static void SideWinderMaze( Grid grid ) {
        List<Cell> run = new List<Cell>();

        foreach( GameObject gameobject in grid.each_cell() ) {
            Cell cell = gameobject.GetComponent<Cell>();
            List<Neighbor> neighbors = new List<Neighbor>();

            if( cell.north ) {
                neighbors.Add( Neighbor.NORTH );
            }
            if( cell.east ) {
                neighbors.Add( Neighbor.EAST );
            }
            if( neighbors.Count > 0 ) {
                int index = Random.Range( 0, neighbors.Count );

                if( neighbors[ index ] == Neighbor.EAST ) {
                    run.Add( cell );
                    cell.link( cell.east );
                } else {
                    run.Add( cell );
                    index = Random.Range( 0, run.Count );
                    cell = run[ index ];
                    cell.link( cell.north );
                    run.Clear();
                }
            }
        }
    }

    public static void BinaryMaze( Grid grid ) {

        foreach( GameObject gameobject in grid.each_cell() ) {
            Cell cell = gameobject.GetComponent<Cell>();
            List<GameObject> neighbors = new List<GameObject>();

            if( cell.north ) {
                neighbors.Add( cell.north );
            }
            if( cell.east ) {
                neighbors.Add( cell.east );
            }
            if( neighbors.Count > 0 ) {
                int index = Random.Range( 0, neighbors.Count );
                cell.link( neighbors[ index ] );
            }
        }
    }

    public static Distances Dijkstra( GameObject entry ) {
        Distances distances = new Distances( entry );
        List<GameObject> frontier = new List<GameObject>();
        frontier.Add( entry );

        while( frontier.Count > 0 ) {
            List<GameObject> new_frontier = new List<GameObject>();

            foreach( GameObject gameobject in frontier ) {
                Cell cell = gameobject.GetComponent<Cell>();

                foreach( GameObject link in cell.getlinks() ) {

                    if( !distances.hasKey( link ) ) {
                        distances[ link ] = distances[ gameobject ] + 1;
                        new_frontier.Add( link );
                    }
                }
                frontier = new_frontier;
            }
        }
        return distances;
    }
}
