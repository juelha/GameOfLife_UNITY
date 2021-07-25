using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///     MACRO LEVEL
/// </summary>
public class Grid : MonoBehaviour
{
    public GameObject CellPrefab;
    public Cell[,] GridArray;
    private int x_length, y_length, n_col, n_row;

    void Start()
    {
        setupGrid();
    }

    void Update()
    {
        calcNewGrid();
    }

    /// <summary>
    /// Methods for setting up and filling the grid 
    /// </summary>
    public void setupGrid()
    {
        // top right corner of camera:
        y_length = (int)Camera.main.orthographicSize;
        x_length = y_length * (Screen.width / Screen.height);

        // full camera coverage
        n_col = x_length * 2;
        n_row = y_length * 2;

        // set up grid
        GridArray = new Cell[n_col, n_row];
        for (int i = 0; i < n_col; i++)
        {
            for (int j = 0; j < n_row; j++)
            {
                GridArray[i, j] = fillGrid(CellPrefab, i, j, (int)Math.Floor(UnityEngine.Random.Range(0.0f, 2.0f))); // normal round() will round 0.5 to 0 
                GridArray[i, j].drawCell();
            }
        }
    }

    public Cell fillGrid(GameObject prefab, int x, int y, int value)
    {
        Vector3 pos = new Vector3(x - (x_length - 0.5f), y - (y_length - 0.5f));
        pos.z = 0;
        Cell newCell = Instantiate(prefab, pos, Quaternion.identity).GetComponent<Cell>();
        newCell.state = value;
        return newCell;
    }


    /// <summary>
    /// calculates the new generation of the Grid and updates the state of the cells at the end
    /// </summary>

    public void calcNewGrid()
    {
        // calc new state of cells
        for (int i = 0; i < n_col; i++)
        {
            for (int j = 0; j < n_row; j++)
            {
                var oldCell = GridArray[i, j];

                // count alive neighbors
                var n_neighbor = countNeighbors(i, j, GridArray);

                GridArray[i, j].newState = oldCell.calcNewState(n_neighbor);
            }
        }
        // set state to calculated new state and draw
        for (int i = 0; i < n_col; i++)
        {
            for (int j = 0; j < n_row; j++)
            {
                GridArray[i, j].state = GridArray[i, j].newState;
                GridArray[i, j].drawCell();
            }
        }
    }

    private bool inBounds(int x, int y)
    {
        return (x >= 0) && (x < n_col) && (y >= 0) && (y < n_row);
    }

    public int countNeighbors(int x, int y, Cell[,] OldGridArray)
    {
        var sum = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                // if out of bounds -> neighbor = dead
                if (inBounds(x + i, y + j))
                {
                    sum += OldGridArray[x + i, y + j].state;
                }
                else { continue; }
            }
        }
        // minus self
        sum -= OldGridArray[x, y].state;
        return sum;
    }
}
