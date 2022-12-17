using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TerrainTools;

public class endgame:MonoBehaviour
{
    public int count = 0; 

    public GameObject Gendata;
    public GameObject score;
    public GameObject turn;
    public GameObject camera;
    public GameObject friendlycampos;
    public Material blue;
    public Material white;
    public int[,] AiShipsplaced;
    GameObject[,] p1board;
    GameObject[,] enemynpcboard;

    // Start is called before the first frame update

    public endgame(GameObject[,] p1board, GameObject[,] npcboard, GameObject Gendataui, GameObject scoreui, GameObject turnui, GameObject camera, GameObject campos, Material blue)
    {

        this.camera = camera;
        this.score = scoreui;
        this.Gendata = Gendataui;
        this.turn = turnui;
        this.p1board = p1board;
        this.enemynpcboard = npcboard;
        this.friendlycampos = campos;
        this.blue = blue;
        starting();
    }

    void starting()
    {
        
        score.SetActive(true);
        Gendata.SetActive(true);
        turn.SetActive(true);

        /// data setting above
        movecamerafriendlyboard();
        enableboard2();




    }


    void movecamerafriendlyboard()
    {
        camera.transform.position = friendlycampos.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        count++;
        Debug.Log(count);
    }



    public void preshow()
    {
        // int mask = 1 << LayerMask.NameToLayer("ignore");
        RaycastHit hitinfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo);
        if (hit)
        {
            if (hitinfo.transform.GetComponent<Renderer>().material.color != Color.red)
                hitinfo.transform.GetComponent<Renderer>().material = blue ;

            // Debug.Log(hitinfo.transform.name);
            GetGameObjectandmakeotherswhite(hitinfo.transform.name, hitinfo.transform);
        }

    }
    
    void cleanboard()
    {
        for (int col = 0; col < 10; col++)
        {

            for (int row = 0; row < 10; row++)
            {
                if (enemynpcboard[row, col].transform.GetComponent<Renderer>().material.color != Color.green)
                {
                    enemynpcboard[row, col].transform.GetComponent<Renderer>().material = white;

                }
                enemynpcboard[row, col].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            }


        }

    }
    
    //-1 a hit spot, 0 water, -2 a hit ship 
    int hitaiboard( int cordx, int cordy)
    {
        for (int col = 0; col < 10; col++)
        {

            for (int row = 0; row < 10; row++)
            {
                if (cordx == col && cordy == row)
                {
                    if (AiShipsplaced[cordx, cordy] == 0)
                    {
                        AiShipsplaced[cordx, cordy] = -1;
                    }
                }

            }
        }
                return 0;


    }
    void GetGameObjectandmakeotherswhite(string rowcol, Transform obj)
    {
        int rowe = rowcol[0] - '0';
        int cole = rowcol[2] - '0';

        for (int col = 0; col < 10; col++)
        {

            for (int row = 0; row < 10; row++)
            {
                if (enemynpcboard[row, col].transform != obj)
                {



                }
                else
                {
                    cleanboard();
                    if (enemynpcboard[row, col].transform.GetComponent<Renderer>().material.color != Color.green)
                        enemynpcboard[row, col].transform.GetComponent<Renderer>().material = blue;

                    if (Input.GetMouseButtonUp(0))
                    {
                        hitaiboard(rowe, cole);
                        // shooting missle

                    }
                }
            }


        }





    }

    void paintaiboard() /// work on this
    {
        for (int col2 = 0; col2 < 10; col2++)
        {
            for (int row2 = 0; row2 < 10; row2++)
            {
                enemynpcboard[row2, col2].transform.GetComponent<BoxCollider>().enabled = true;
            }
        }
    }


    public void setmaterials(Material white)
    {
        this.white = white; 
    }

    public void setaishipsplaced(int[,] AiShipsplaced)
    {
        this.AiShipsplaced = AiShipsplaced; 
    }

    public void enableboard2()// deactivates board1
    {

        for (int col2 = 0; col2 < 10; col2++)
        {
            for (int row2 = 0; row2 < 10; row2++)
            {
                enemynpcboard[row2, col2].transform.GetComponent<BoxCollider>().enabled = true;
            }
        }

        for (int col2 = 0; col2 < 10; col2++)
        {
            for (int row2 = 0; row2 < 10; row2++)
            {
                p1board[row2, col2].transform.GetComponent<BoxCollider>().enabled = false;
            }
        }

    }



}
