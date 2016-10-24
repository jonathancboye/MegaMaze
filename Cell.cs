using UnityEngine;
using System.Collections;

public enum Neighbor { NORTH, SOUTH, EAST, WEST };

public class Cell : MonoBehaviour
{
    Hashtable links = new Hashtable();

    public static int width = 3, height = 3;
    public int row { get; set; }
    public int column { get; set; }
    public GameObject north { get; set; }
    public GameObject south { get; set; }
    public GameObject east { get; set; }
    public GameObject west { get; set; }

    public GameObject northWall;
    public GameObject southWall;
    public GameObject eastWall;
    public GameObject westWall;
    public GameObject northWestWall;
    public GameObject northEastWall;
    public GameObject southWestWall;
    public GameObject southEastWall;

    public void link( GameObject cell ) {
        links[ cell ] = true;
        cell.GetComponent<Cell>().links[ gameObject ] = true;
    }

    public void unlink( GameObject cell ) {
        links.Remove( cell );
    }

    public ICollection getlinks() {
        return links.Keys;
    }

    public bool islinked( GameObject cell ) {
        return links.ContainsKey( cell );
    }

    public IEnumerator getNeighbors() {
        return links.GetEnumerator();
    }

    public void DestroyLinks() {
        if( north && links.ContainsKey( north ) ) {
            Destroy( northWall );
        }
        if( south && links.ContainsKey( south ) ) {
            Destroy( southWall );
        }
        if( east && links.ContainsKey( east ) ) {
            Destroy( eastWall );
        }
        if( west && links.ContainsKey( west ) ) {
            Destroy( westWall );
        }
    }

}
