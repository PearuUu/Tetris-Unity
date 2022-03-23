using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    GameManager gameManager;
    GridGenerator gridGenerator;
    TetrominoSpawner tetrominoSpawner;
    BackgroundCube[] backgroundCubes;

    private float previousTime;
    float pressTime = 0;
    float currentTime = 0f;
    [SerializeField] float moveTreshold;
    [SerializeField] float moveSpeed;
    static float fallTimeGlobal = 1f;
    float fallTime;

    [SerializeField] float acceleration;

    private bool moveDown, moveRight, moveLeft, rotateCounterwise, rotateClockwise, ground, moveRightHold, moveLeftHold;
    [SerializeField] Vector3 tetrominoMove, rotationPoint;

    public GhostPiece ghostPieceScript;
    public GameObject ghostPiece;




    

    // Start is called before the first frame update
    void Start()
    {
        ghostPiece = Instantiate(ghostPiece, transform.position, transform.rotation);
        ghostPieceScript = ghostPiece.GetComponent<GhostPiece>();
    }

    private void Awake()
    {
        gridGenerator = GameObject.FindObjectOfType<GridGenerator>();
        tetrominoSpawner = GameObject.FindObjectOfType<TetrominoSpawner>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        backgroundCubes = GameObject.FindObjectsOfType<BackgroundCube>();
        fallTime = fallTimeGlobal;
        


    }

    // Update is called once per frame
    void Update()
    {
        TetrominoFall();
        TetrominoControl();
        TetrominoGround();
       
    }

    void TetrominoFall()
    {
        moveDown = Input.GetKey(KeyCode.DownArrow);
        
        
        if(Time.time - previousTime > (moveDown ? fallTime/10: fallTime))
        {
            transform.position -= tetrominoMove;
            
            // check if tetromino hit ground
            if (!ValidMove())
            {
                
                transform.position += tetrominoMove;
                AddToGrid();
                CheckLines();
                TetrominoAcceleration();
                

                if (gridGenerator.GameOver())
                {
                    gameManager.GameOver();
                    this.enabled = false;
                }
                else
                {
                    transform.tag = "Ground";
                    tetrominoSpawner.TetrominoSpawn();
                    this.enabled = false;
                    
                }
                ghostPieceScript.DestroyPiece();
                
            }
            previousTime = Time.time;
        }
    }

    void TetrominoControl()
    {
        moveRight = (Input.GetKeyDown(KeyCode.RightArrow));
        moveLeft = (Input.GetKeyDown(KeyCode.LeftArrow));
        rotateCounterwise = (Input.GetKeyDown(KeyCode.Z));
        rotateClockwise = (Input.GetKeyDown(KeyCode.X));
        
        moveRightHold = Input.GetKey(KeyCode.RightArrow);
        moveLeftHold = Input.GetKey(KeyCode.LeftArrow);
        
        if(moveRightHold)
        {
            pressTime += Time.deltaTime;
            if(pressTime > moveTreshold)
            {
                currentTime += Time.deltaTime;
                if(currentTime > moveSpeed)
                {
                    transform.position += new Vector3(1f, 0, 0);
                    ghostPieceScript.Ghost();
                    currentTime = 0f;
                }
                
                if (!ValidMove())
                {
                    transform.position -= new Vector3(1f, 0, 0);
                    ghostPieceScript.Ghost();
                }
            }
        } 
        if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            pressTime = 0;
        }
        
        if(moveLeftHold)
        {
            pressTime += Time.deltaTime;
            if(pressTime > moveTreshold)
            {
                currentTime += Time.deltaTime;
                if(currentTime > moveSpeed)
                {
                    transform.position -= new Vector3(1f, 0, 0);
                    ghostPieceScript.Ghost();
                    currentTime = 0f;
                }
                
                if (!ValidMove())
                {
                    transform.position += new Vector3(1f, 0, 0);
                    ghostPieceScript.Ghost();
                    
                }
            }
        } 
        if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            pressTime = 0;
        }


        if (moveRight)
        {
            
            transform.position += new Vector3(1f, 0, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(-1f, 0, 0);
            }
            ghostPieceScript.Ghost();
        }
        else if (moveLeft)
        {
            transform.position += new Vector3(-1f, 0, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(1f, 0, 0);
            }
            ghostPieceScript.Ghost();
        }

        if (rotateClockwise)
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
            ghostPieceScript.Ghost();

        }
        else if (rotateCounterwise)
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            }
            ghostPieceScript.Ghost();

        }
    }

    bool ValidMove()
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

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            if(child.tag == "Ghost")
            {
                continue;
            }
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);

            gridGenerator.grid[roundedX, roundedY] = child;
        }
    }

    void CheckLines()
    {
        for(int i = gridGenerator.h - 1; i >= 0; i--)
        {
            if (FullLine(i))
            {
                DeleteLine(i);
                TetrominosDown(i);
            }
        }
    }

    bool FullLine(int i)
    {
        for(int j = 0; j<gridGenerator.w; j++)
        {
            if(gridGenerator.grid[j,i] == null)
            {
                return false;
            }
        }

        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < gridGenerator.w; j++)
        {
            Destroy(gridGenerator.grid[j, i].gameObject);
            gridGenerator.grid[j, i] = null;
        }

    }

    void TetrominosDown(int i)
    {
        for (int y = i; y < gridGenerator.h; y++)
        {
           for(int j = 0; j < gridGenerator.w; j++)
           {
                if(gridGenerator.grid[j,y] != null)
                {
                    gridGenerator.grid[j, y - 1] = gridGenerator.grid[j, y];
                    gridGenerator.grid[j, y] = null;
                    gridGenerator.grid[j, y - 1].position -= tetrominoMove;
                }
           }
        }
    }

    void TetrominoAcceleration()
    {
        fallTime /= acceleration;
        //Debug.Log(fallTime);
    }

    void TetrominoGround()
    {
        ground = Input.GetKeyDown(KeyCode.Space);
        
        if (ground)
        {
            transform.position = ghostPiece.transform.position;
            ghostPieceScript.DestroyPiece();
            AddToGrid();
            CheckLines();
            TetrominoAcceleration();
            transform.tag = "Ground";
            tetrominoSpawner.TetrominoSpawn();
            this.enabled = false;
        }
    }
}
