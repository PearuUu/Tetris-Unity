using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoSpawner : MonoBehaviour
{
    [SerializeField] Transform[] Tetrominos;
    
    void Start()
    {
        TetrominoSpawn();
    }

    public void TetrominoSpawn()
    {
        int rand = Random.Range(0, Tetrominos.Length);

        Instantiate(Tetrominos[rand], transform.position, Tetrominos[rand].rotation);
    }
}
