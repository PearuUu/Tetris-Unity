using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCube : MonoBehaviour
{
    [SerializeField] GridGenerator gridGenerator;
/*    [SerializeField] Material normal;
    [SerializeField] Material surrounded;*/
    MeshRenderer meshRenderer;
    int x,y;

    private void Awake()
    {
        gridGenerator = GameObject.FindObjectOfType<GridGenerator>();
        x = Mathf.RoundToInt(transform.position.x);
        y = Mathf.RoundToInt(transform.position.y);
        /*meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = normal; */
    }

    private bool isSurrounded()
    {

        if(gridGenerator.grid[x,y] != null)
        {
            return false;
        }
        else if(x == gridGenerator.w-1)
        {
            if(y == gridGenerator.h-1)
            {
                // prawy gorny rog
                if (gridGenerator.grid[x - 1, y] != null)
                {  
                    return true;
                }
                return false;
            }
            else
            {
                
                if ((gridGenerator.grid[x - 1, y] != null && gridGenerator.grid[x, y + 1] != null) || gridGenerator.backgroundGrid[x, y + 1] != null || gridGenerator.backgroundGrid[x - 1, y] != null)
                {
                    return true;
                }
                return false;
            }
        }
        else if (x == 0)
        {
            if (y == gridGenerator.h-1)
            {
                // lewy gorny rog
                if (gridGenerator.grid[x + 1, y] != null)
                { 
                    return true;
                }
                return false;
            }
            else
            {
                //Debug.Log(y);
                if ((gridGenerator.grid[x, y + 1] != null && gridGenerator.grid[x + 1, y] != null) || gridGenerator.backgroundGrid[x, y + 1] != null || gridGenerator.backgroundGrid[x + 1, y] != null)
                { 
                    return true;
                }
                return false;
            }
        }
        else
        {
            if (y == gridGenerator.h - 1)
            {
                if((gridGenerator.grid[x - 1, y] != null && gridGenerator.grid[x + 1, y] != null))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if((gridGenerator.grid[x - 1, y] != null && gridGenerator.grid[x, y + 1] != null && gridGenerator.grid[x + 1, y] != null) || gridGenerator.backgroundGrid[x, y + 1] != null || gridGenerator.backgroundGrid[x+1, y] != null || gridGenerator.backgroundGrid[x-1,y] != null)
                {
                    return true;
                }
                return false;
            }
            
        }
    }

    public void ChangeMaterial()
    {
        if (isSurrounded())
        {
            //this.meshRenderer.material = surrounded;
            gridGenerator.backgroundGrid[x, y] = transform;
        }
        else
        {
            //this.meshRenderer.material = normal;
            gridGenerator.backgroundGrid[x, y] = null;
        }
    }
}
