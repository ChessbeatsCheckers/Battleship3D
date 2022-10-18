using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class createboard : MonoBehaviour
{
    public GameObject boardpiece;
    private GameObject[,] p1board = new GameObject[10,10];
    private GameObject[,] p2board = new GameObject[10, 10];    // Start is called before the first frame update
    void Start()
    {
        //int row = 1; 
       // int col = 1;
//
      // GameObject ob = GameObject.Instantiate(boardpiece);
      // p1board[row, col] = GameObject.Instantiate(boardpiece);
      // p1board[row, col].transform.position = new Vector3(col, boardpiece.transform.position.y, row);
       
       createboards(boardpiece);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // up is z right is x
      void createboards(GameObject boardpiece)
      {
        char colc = 'A';
        

        for (int col = 0; col < 10; col++)
        {
            for (int row = 0; row < 10; row++)
            {


                
                Debug.Log("test");
                p1board[row,col] = GameObject.Instantiate(boardpiece);
                p1board[row, col].transform.position = new Vector3(col, boardpiece.transform.position.y, row);
                string name = "B1[" + col + ":" + row + "]";
                p1board[row, col].transform.name = "BoardPiece" ;
                p1board[row, col].transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            }

        }



        boardpiece.transform.position = new Vector3(1000, 1000, 1000);
        }

    char decideletter(int col)
    {
        if (col == 1)
            return 'B';
        else if (col == 2)
            return 'C';
        else if (col == 3)
            return 'D';
        else if (col == 4)
            return 'E';
        else if (col == 5)
            return 'F';
        else if (col == 6)
            return 'G';
        else if (col == 7)
            return 'H';
        else if (col == 8)
            return 'I';
        else if (col == 9)
            return 'L';
        else
            return 'A';


    }
        
 }


      

