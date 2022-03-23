using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPiece : MonoBehaviour
{

    Transform parent;
    Tetromino parentScript;

    GridGenerator gridGenerator;
    // Start is called before the first frame update
    void Awake()
    {
        parent = GameObject.FindGameObjectWithTag("Tetromino").GetComponent<Transform>();
        parentScript = parent.GetComponent<Tetromino>();
        gridGenerator = GameObject.FindObjectOfType<GridGenerator>();
    }

    void Start()
    {
        Ghost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ghost()
    {
        int parentX = Mathf.RoundToInt(parent.position.x);
        Debug.Log(parentX);
        Quaternion parentRotation = parent.rotation;
        int minY = -1;
        
        foreach(Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);

            //Debug.Log("RY: "+roundedY);

            while(gridGenerator.grid[roundedX,roundedY] == null && roundedY > 0)
            {
                roundedY--;
                Debug.Log(gridGenerator.grid[roundedX,roundedY]);
            }
            if(roundedY < minY)
            {
                minY = roundedY;
                roundedY = Mathf.RoundToInt(child.transform.position.y);
            }
        }     
        
        Debug.Log(minY);
        transform.rotation = parentRotation;
        transform.position = new Vector3(parentX, minY, transform.position.z);
        while(!ValidMoveGhost())
        {
            transform.position += new Vector3(0,1,0);
        }
        
    }

    public void DestroyPiece()
    {
        Debug.Log("Destroy");
        Destroy(gameObject);
    }

    bool ValidMoveGhost()
    {
        foreach (Transform child in transform)
        {

            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);

            if (roundedX < 0 || roundedX >= gridGenerator.w || roundedY < 0 || roundedY >= gridGenerator.h)
            {
                return false;
            }

            if (gridGenerator.grid[roundedX, roundedY] != null) return false;

        }
        return true;
    }
}
