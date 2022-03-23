using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private static int width, height;
    public Transform[,] grid;
    public Transform[,] backgroundGrid;
    public int w, h;

    [SerializeField] Transform backgroundCube,borderCube;


    // Start is called before the first frame update
    void Start()
    {
        width = w;
        height = h;
        grid = new Transform[width, height];
        backgroundGrid = new Transform[width, height];
        /*int b = 0;
        foreach(Transform t in grid)
        {
            b++;
            Debug.Log(b + ": " + t);
        }*/

        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateGrid()
    {

        for(int i = 0; i<height; i++)
        {
            // left and right border
            Instantiate(borderCube, new Vector3(width, i, 0), Quaternion.identity, gameObject.transform);
            Instantiate(borderCube, new Vector3(-1, i, 0), Quaternion.identity, gameObject.transform);
            
            for (int j = 0; j<width; j++)
            {
                //background and bottom and top
                Instantiate(backgroundCube, new Vector3(j, i, 0),Quaternion.identity, gameObject.transform);
                Instantiate(borderCube, new Vector3(j, height, 0), Quaternion.identity, gameObject.transform);
                Instantiate(borderCube, new Vector3(j, -1, 0), Quaternion.identity, gameObject.transform);
            }
        }

        // corners
        Instantiate(borderCube, new Vector3(-1, height, 0), Quaternion.identity, gameObject.transform);
        Instantiate(borderCube, new Vector3(width, height, 0), Quaternion.identity, gameObject.transform);
        Instantiate(borderCube, new Vector3(-1, -1, 0), Quaternion.identity, gameObject.transform);
        Instantiate(borderCube, new Vector3(width, -1, 0), Quaternion.identity, gameObject.transform);

    }

    public bool GameOver()
    {
        for(int i = height-1; i > height - 3; i--)
        {
            //Debug.Log("i: "+i);
            for(int j = 0; j < width; j++)
            {
                //Debug.Log("j: " + j);
                if (grid[j,i] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
