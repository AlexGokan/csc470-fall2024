using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{

    public GameObject cell_prefab;

    public float spacing = 1.1f;

    private cellScript[,] grid;//1 comma for 2d array of cell scripts

    void Start()
    {
        grid = new cellScript[10,10];
        
        for(int x=0; x<10; x++){
            for(int y=0; y<10; y++){
                Vector3 pos = transform.position;
                pos.x += x*spacing;
                pos.y += y*spacing;

                GameObject cell = Instantiate(cell_prefab,pos,Quaternion.identity);
                grid[y,x] = cell.GetComponent<cellScript>();

                grid[y,x].alive = Random.value > 0.5f;
                grid[y,x].yi = y;
                grid[y,x].xi = x;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
