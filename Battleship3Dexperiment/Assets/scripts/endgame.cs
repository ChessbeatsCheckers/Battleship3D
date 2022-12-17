using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TerrainTools;
using UnityEngine.UIElements;
using Random = System.Random;

public class endgame:MonoBehaviour
{
    private Random random = new Random();
    public int count = 0;
    private int turnsim=0;
    public GameObject Gendata;
    public GameObject score;
    public GameObject turn;
    public GameObject camera;
    public GameObject friendlycampos;
    public Material blue;
    public Material white;
    public int[,] AiShipsplaced;
    public int[,] plrshipsplaced = new int[10, 10];
    GameObject[,] p1board;
    GameObject[,] enemynpcboard;
    Transform cameraogposition; 
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
        numericalizeplrarre();



    }


    void movecamerafriendlyboard()
    {
        cameraogposition = camera.transform ;
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
    int hitboard( int cordx, int cordy, string boardname)
    {

        if (boardname == "ai")
        {
            if(AiShipsplaced[cordx, cordy] == 0 || AiShipsplaced[cordx, cordy] == 1)
            {
                    if (AiShipsplaced[cordx, cordy] == 0)
                        AiShipsplaced[cordx, cordy] = -1;

                    if (AiShipsplaced[cordx, cordy] == 1)
                        AiShipsplaced[cordx, cordy] = -2;
            }
            else
            {
             return 0;
            }
            return 1;
        }///ai boardm hit ends
        else 
        {
            return 1;
        }
    }

    void GetGameObjectandmakeotherswhite(string rowcol, Transform obj)
    {
        int hitsuccess=-100;
        int rowe = rowcol[0] - '0';
        //change on turn
        int cole = rowcol[3] - '0';
       // Debug.Log("your trying to hit " + rowe + " and " + cole);
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

                    paintboards();

                    if (!OddOrEven(turnsim))// if odd ai hits plrboard
                    {
                        rowe= random.Next(1, 11);
                        cole= random.Next(1, 11);

                        hitsuccess = hitboard(rowe, cole, "plr");
                        if (hitsuccess == 1)
                        {
                            camera.transform.position = cameraogposition.position;
                            turnsim++;

                            // next turn
                        }
                        {

                            //error message
                        }
                    }
                    //// AI HITTING PLAYER ABOVE AND PLAYER HITTING AI BELOW-----------------------------
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (OddOrEven(turnsim))
                        {
                            hitsuccess = hitboard(rowe, cole, "ai");

                        }
                        

                        if (hitsuccess == 1)
                        {
                            camera.transform.position = cameraogposition.position;
                            turnsim++;
                            
                         // next turn
                        }
                        { 

                          //error message
                        }

                        // shooting missle
                        printarre(AiShipsplaced);
                    }



                }
            }


        }





    }

    void paintboards() /// work on this
    {
        for (int col2 = 0; col2 < 10; col2++)
        {
            for (int row2 = 0; row2 < 10; row2++)
            {
                if (AiShipsplaced[col2, row2] == -1 || AiShipsplaced[col2, row2] == -2) {
                    if(AiShipsplaced[col2, row2] == -1 )
                    enemynpcboard[row2, col2].transform.GetComponent<Renderer>().material.color = Color.grey;

                    if (AiShipsplaced[col2, row2] == -2)
                        enemynpcboard[row2, col2].transform.GetComponent<Renderer>().material.color = Color.red;

                }

                if (plrshipsplaced[col2, row2] == -1 || plrshipsplaced[col2, row2] == -2)
                {
                    if (plrshipsplaced[col2, row2] == -1)
                        p1board[row2, col2].transform.GetComponent<Renderer>().material.color = Color.grey;

                    if (plrshipsplaced[col2, row2] == -2)
                        p1board[row2, col2].transform.GetComponent<Renderer>().material.color = Color.red;

                }


                // enemynpcboard[row2, col2].transform.GetComponent<BoxCollider>().enabled = true;
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

    void printarre(int[,] a)
    {
        Debug.Log(" Printing board------------------- ");
        for (int col = 0; col < 10; col++)
        {

            Debug.Log(a[0, col] + " " + a[1, col] + " " + a[2, col] + " " + a[3, col] + " " + a[4, col] + " " + a[5, col] + " " + a[6, col] + " " + a[7, col] + " " + a[8, col] + " " + a[9, col] + " ");
        }
    }
      bool OddOrEven(int num)
    {
        return num % 2 == 0;
    }



    void numericalizeplrarre()
    {

     

        for (int col2 = 0; col2 < 10; col2++)
        {
            for (int row2 = 0; row2 < 10; row2++)
            {
                if (p1board[col2, row2].GetComponent<Renderer>().material.color == Color.green)
                    plrshipsplaced[col2, row2] = 1;
                else
                    plrshipsplaced[col2, row2] = 0;

            }
        }

        Debug.Log("this is the plrs board-=-------------------------------------------------=================================================------------------================-----");
        printarre(plrshipsplaced);
    }

}
