using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using TMPro;
public class createboard : MonoBehaviour
{
    public Material Yellow; 
    public Material white;
    public Material blue; 
    public GameObject boardpiece;
    private GameObject[,] p1board = new GameObject[10,10];
    private GameObject[,] p2board = new GameObject[10, 10];    // Start is called before the first frame update
    public GameObject butorientation, but5spacebattleship, but4spacebattleship, but3spacebattleship, but3spacebattleship2, but2spacebattleship;
    public GameObject f5spacer, f4spacer, f3spacer1, f3spacer2, f2spacer; 
    bool verticle = true;
    int shipspace = -1;
    int shipselected = -1; 
    void Start()
    {
        //int row = 1; 
        // int col = 1;
        //
        // GameObject ob = GameObject.Instantiate(boardpiece);
        // p1board[row, col] = GameObject.Instantiate(boardpiece);
        // p1board[row, col].transform.position = new Vector3(col, boardpiece.transform.position.y, row);

        butorientation.GetComponent<Button>().onClick.AddListener(orientationchage);
        but5spacebattleship.GetComponent<Button>().onClick.AddListener(set5spacebattleship);
        but4spacebattleship.GetComponent<Button>().onClick.AddListener(set4spacebattleship);
        but3spacebattleship.GetComponent<Button>().onClick.AddListener(set3spacebattleship);
        but3spacebattleship2.GetComponent<Button>().onClick.AddListener(set3spacesub);
        but2spacebattleship.GetComponent<Button>().onClick.AddListener(set2spacebattleship);



        createboards(boardpiece);
        
    }

    // Update is called once per frame
    void Update()
    {

        preshow();
    }


    void preshow()
    {
       // int mask = 1 << LayerMask.NameToLayer("ignore");
        RaycastHit hitinfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo);
        if (hit)
        {
            hitinfo.transform.GetComponent<Renderer>().material = blue;
            Debug.Log(hitinfo.transform.name);
            GetGameObjectandmakeotherswhite(hitinfo.transform.name, hitinfo.transform);
        }
    
    }
    

    void GetGameObjectandmakeotherswhite(string rowcol,Transform obj)
    {
       int rowe = rowcol[0] - '0';
       int cole = rowcol[2] - '0';

        for (int col = 0; col < 10; col++)
        {
            
            for (int row = 0; row < 10; row++)
            {
                if (p1board[row, col].transform != obj)
                {
                   


                }
                else
                {
                    cleanboard();
                    p1board[row, col].transform.GetComponent<Renderer>().material = blue;

                    if(shipspace!=-1)
                    preshowship(row, col);

                }
            }


        }

        



    }

    void preshowship(int row, int col)
    {
        if (verticle)
        {
            if (row + shipspace <= 10)
            {
                for (int count = row; count < (row + shipspace); count++)
                {
                    p1board[count, col].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true; 
                }
            }
            
        }
        else
        {
            if (col + shipspace <= 10)
            {
                for (int count = col; count < (col + shipspace); count++)
                {
                    p1board[row, count].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
                }
            }

        }


    }

    void cleanboard()
    {
        for (int col = 0; col < 10; col++)
        {

            for (int row = 0; row < 10; row++)
            {
                p1board[row, col].transform.GetComponent<Renderer>().material = white;
                p1board[row, col].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            }


        }

    }

    // up is z right is x
      void createboards(GameObject boardpiece)
      {
        char colc = 'A';
        

        for (int col = 0; col < 10; col++)
        {
            for (int row = 0; row < 10; row++)
            {


                
               // Debug.Log("test");
                p1board[row,col] = GameObject.Instantiate(boardpiece);
                p1board[row, col].transform.position = new Vector3(col, boardpiece.transform.position.y, row);
                string name = "B1[" + col + ":" + row + "]";
                p1board[row, col].transform.name = col+"/"+row ;
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

    public void orientationchage()
    {
        Debug.Log("orientation change");
        if (verticle == true)
            verticle = false;
        else
            verticle = true;
    
    }

    public void set5spacebattleship()
    {
        shipselected = 5;
        shipspace = 5;
    }
    public void set4spacebattleship()
    {
        shipselected = 4;
        shipspace = 4;
    }
    public void set3spacebattleship()
    {
        shipselected = 3;
        shipspace = 3;
    }
    public void set3spacesub()
    {
        shipselected = 2;
        shipspace = 3;
    }
    public void set2spacebattleship()
    {
        shipselected = 1;
        shipspace = 2;
    }

}


      

