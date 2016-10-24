using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Distances
{
    Hashtable distances = new Hashtable();

    public Distances( GameObject root ) {
        distances[ root ] = 0;
    }

    public int this[ GameObject cell ] {
        get { return GetValue( cell ); }
        set { SetValue( cell, value ); }
    }

    public bool hasKey( GameObject key ) {
        return distances.Contains( key );
    }

    int GetValue( GameObject cell ) {
        return ( int ) distances[ cell ];
    }

    void SetValue( GameObject cell, int distance ) {
        distances[ cell ] = distance;
    }

    public ICollection cells() {
        return distances.Keys;
    }


}
