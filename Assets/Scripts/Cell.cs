using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     MICRO LEVEL
/// </summary>
public class Cell : MonoBehaviour
{

    public int state { get; set; } // is either dead or alive 
    public int newState { get; set; }
    public static Cell Instance;

    private MeshRenderer mr;

    void Awake()
    {
        Instance = this;
        mr = GetComponent<MeshRenderer>();
    }

    public int calcNewState(int n_neighbors)
    {
        int newstate = 0;

        // RULE: reproduction
        // 0 → 3 live → 1
        if (this.state == 0 && n_neighbors == 3)
        {
            newstate = 1;
            //  Debug.Log("alive");
        }

        // RULE: under-/overpopulation
        // 1 → < 2 live OR > 3 live → 0
        else if (this.state == 1 && (3 < n_neighbors || n_neighbors < 2))
        {
            newstate = 0;
            //  Debug.Log("dead");
        }
        else
        {
            newstate = this.state;
        }
        return newstate;
    }


    public void drawCell()
    {
        if (state == 0)
        {
            mr.material.DisableKeyword("_EMISSION");
        }
        else
        {
            mr.material.EnableKeyword("_EMISSION");
        }
    }
}
