using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TerrainTools;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class endgame:MonoBehaviour
{
    private bool endgames = false;
   private bool debounce= false; 
    public int count = 0;
    private int plr1points= 0;
    private int aipoints = 0;
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

        //paintp1redtest();

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



        if (endgames == false)
        {
            // int mask = 1 << LayerMask.NameToLayer("ignore");
            RaycastHit hitinfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo);
            if (hit)
            {
                if (hitinfo.transform.GetComponent<Renderer>().material.color != Color.red)
                    hitinfo.transform.GetComponent<Renderer>().material = blue;

                // Debug.Log(hitinfo.transform.name);
                GetGameObjectandmakeotherswhite(hitinfo.transform.name, hitinfo.transform);
            }
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
        Debug.Log("attempting to hit "+ cordx +" and  " + cordy);
        if (boardname == "ai")
        {
            if(AiShipsplaced[cordx, cordy] == 0 || AiShipsplaced[cordx, cordy] == 1)
            {
                    if (AiShipsplaced[cordx, cordy] == 0)
                        AiShipsplaced[cordx, cordy] = -1;

                    if (AiShipsplaced[cordx, cordy] == 1)
                        AiShipsplaced[cordx, cordy] = -2;

                return 1;
            }
            else
            {
             return 0;
            }
            
        }///ai boardm hit ends
        else if(boardname=="plr") 
        {
            if (plrshipsplaced[cordx, cordy] == 0 || plrshipsplaced[cordx, cordy] == 1)
            {
                Debug.Log("WE ARE HITTING PLR 1 BOARD AND MARKING DOWN , T AT " + cordx + cordy + " and its name is " + p1board[cordx, cordy].transform.name);
                if (plrshipsplaced[cordx, cordy] == 0)
                    plrshipsplaced[cordx, cordy] = -1;

                if (plrshipsplaced[cordx, cordy] == 1)
                    plrshipsplaced[cordx, cordy] = -2;

                return 1;
            }
            else
            {
                return 0;
            }
            
        }
        return -100; 
    }

    void GetGameObjectandmakeotherswhite(string rowcol, Transform obj)
    { if (debounce == false)
        {
            bool success = false; 
            debounce = true;
            int hitsuccess = -100;
            int rowe = rowcol[0] - '0';
            //change on turn
            int cole = rowcol[3] - '0';

            int[] validcords;
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

                            validcords = getvalidcordinatesforai();
                            hitsuccess = hitboard(validcords[0], validcords[1], "plr");
                            if (hitsuccess == 1)
                            {
                                camera.transform.position = cameraogposition.position;
                                
                                success = true; 
                                

                                // next turn
                            }
                            {
                                if(hitsuccess==-100)
                                    Debug.Log("not doing anything");
                                if (hitsuccess == 0)
                                    Debug.Log("you hit that allready try again on plrboard");
                                Debug.Log("ALERT HITBOARD SHOT AND RETURNED -------------------- "+hitsuccess);

                                
                                //error message
                            }
                        }
                        //// AI HITTING PLAYER ABOVE AND PLAYER HITTING AI BELOW-----------------------------
                        if (Input.GetMouseButtonUp(0))
                        {
                            if (endgames == false)
                            {
                                if (OddOrEven(turnsim))
                                {
                                    hitsuccess = hitboard(rowe, cole, "ai");




                                    if (hitsuccess == 1)
                                    {

                                        success = true;

                                        camera.transform.position = cameraogposition.position;


                                        // next turn
                                    }
                                    {
                                        if (hitsuccess == -100)
                                            Debug.Log("not doing anything");
                                        if (hitsuccess == 0)
                                            Debug.Log("you hit that allready try again on plrboard");
                                        //error message
                                        Debug.Log("ALERT HITBOARD SHOT AND RETURNED -------------------- " + hitsuccess);
                                    }

                                    // shooting missle
                                    printarre(plrshipsplaced);
                                }
                            }
                        }



                    }
                }


            }

            if (success)
            {
                


                turnsim++;

            }

            debounce = false;
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


    void pringameobjarre(GameObject[,] a)
    {
        Debug.Log(" Printing board------------------- ");
        for (int col = 0; col < 10; col++)
        {

            for (int row2 = 0; row2 < 10; row2++)
            {
                Debug.Log(a[col, row2].transform.name);
            }
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
                if (p1board[row2, col2].GetComponent<Renderer>().material.color == Color.green)
                {
                    plrshipsplaced[col2, row2] = 1;
                    Debug.Log("this name has a ship part on it" + col2 + " n " + row2 + " and name is " + p1board[row2, col2].transform.name);
                }
                else
                {
                    Debug.Log("this name has a water part on it" + col2 + " n " + row2 + " and name is " + p1board[row2, col2].transform.name);
                    plrshipsplaced[col2, row2] = 0;
                }
            }
        }

        Debug.Log("this is the plrs board-=-------------------------------------------------==============================s===================------------------================-----");
        printarre(plrshipsplaced);
        pringameobjarre(p1board);
    }

    int [] getvalidcordinatesforai()
    {
        bool  validatadatafound = false;
        int rowe = Random.Range(0, 10);
        int cole = Random.Range(0, 10);
        int[] validcords = new int[2];
        Debug.Log("attempting ai valid");
        while (validatadatafound == false)
        {
            Debug.Log("workin===================================================================================================================");
            
                    rowe = Random.Range(0, 10);
                    cole = Random.Range(0, 10);
                    if (plrshipsplaced[rowe, cole] == 0|| plrshipsplaced[rowe, cole] ==1 )
                    {
                        validatadatafound = true;
                        validcords[0] = rowe;
                        validcords[1] = cole;
                    }
             
        }
        Debug.Log("FINISHED=============================================================================================================================================");
        Debug.Log("Our valid cordinate is "+ validcords[0]+ " " +validcords[1]+" with the name of "+ p1board[rowe, cole].transform.name);
        if (plrshipsplaced[rowe, cole] == 1)
            Debug.Log("IT DOES HAVE A SHIP ON IT");
        else
            Debug.Log("IT IS WATER");

        return validcords;
    }


    void paintp1redtest()
    {

        for (int i = 0; 1 < 10; i++)
        {
            for (int b = 0; b < 10; b++)
            {
                //if (p1board[b, i].GetComponent<Renderer>().material.color == Color.green)
                if (plrshipsplaced[b,i]==1)
                {
                    p1board[b, i].GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }

    }

    public string getendzone()
    {

        calcpoints();

        if (plr1points == 17)
        {
            endgames = true;
            return "plr";
        }

        if (aipoints == 17)
        {
            endgames = true;
            return "ai";
        }
        
        return "";
     
    }

    private void calcpoints()
    {
        int aipoints = 0;
        int plr1points = 0;

       

        for (int i = 0; i < 10; i++)
        {
            for (int a = 0; a < 10; a++)
            {
                if (plrshipsplaced[a, i] == -2)
                    aipoints++;

                if (AiShipsplaced[a,i]==-2)
                    plr1points++;

            }
        }

        this.plr1points = plr1points;
        this.aipoints = aipoints;
        score.GetComponent<TextMeshProUGUI>().text = aipoints.ToString() + " / " + plr1points.ToString();
    }


}
