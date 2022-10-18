using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createboard : MonoBehaviour
{
    public GameObject boardpiece;
    private GameObject[,] p1board = new GameObject[10,10];
    private GameObject[,] p2board = new GameObject[10, 10];    // Start is called before the first frame update
    void Start()
    {
        int row = 1; 
        int col = 1;

       GameObject ob = GameObject.Instantiate(boardpiece);
       p1board[row, col] = GameObject.Instantiate(boardpiece);
       p1board[row, col].transform.position = new Vector3(col, boardpiece.transform.position.y, row);
       
       //createboards(boardpiece);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // up is z right is x
      void createboards(GameObject boardpiece)
      {
        int row = 0;
        int col = 0;

        while (col < 10)

            
            while (row < 10)
            { 
                
                p1board[row,col] = GameObject.Instantiate(boardpiece);
                p1board[row, col].transform.position = new Vector3(col, boardpiece.transform.position.y, row);
                row++;
            }
            row = 0;
            col++;
        }

        
       }


      

