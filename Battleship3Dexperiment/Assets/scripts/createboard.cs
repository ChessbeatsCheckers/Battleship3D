using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createboard : MonoBehaviour
{
    public GameObject boardpiece; 
    private GameObject[,] p1board = new GameObject[10,10];
    private GameObject[,] p2board = new GameObject[10, 10]; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



      void createboards(GameObject boardpiece)
      {
        int row = 0;
        int col = 0;

        while (col < 10)
        {
            while (row < 10)
            {
                p1board[row,col] = GameObject.Instantiate(boardpiece);
                
                row++;
            }
            col++;
        }

        Debug.Log("asda"); 
       }


      
}
