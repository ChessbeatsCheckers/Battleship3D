using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using TMPro;
using Random = UnityEngine.Random;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class createboard : MonoBehaviour
{
    private int switcher = 0;
    public GameObject viewotherboardui;

    public AudioClip winnerwinnersong;
    public AudioClip loserlosersong; 

    public AudioClip explosionsound;
    public AudioClip splashsound;
    private string endtime =""; 
    public Material Yellow; 
    public Material green; 
    public Material white;
    public Material blue; 
    public GameObject boardpiece;
    private int phase2debounce = 1; 
    private GameObject[,] p1board = new GameObject[10,10];
    private GameObject[,] p2board = new GameObject[10, 10];    // Start is called before the first frame update
    private int[,] AiShipsplaced = new int[10, 10];
    public GameObject butorientation, but5spacebattleship, but4spacebattleship, but3spacebattleship, but3spacebattleship2, but2spacebattleship, butexit;
    public GameObject f5spacer, f4spacer, f3spacer1, f3spacer2, f2spacer; 
    bool verticle = true;
    int shipspace = -1;
    int shipselected = -1;
    public GameObject gendataui;
    public GameObject scoreui;
    public GameObject turnui;
    public GameObject campos;
    public GameObject endzonecam;
    public bool endgameactive = false;

    // enduistuff below
    public GameObject enuigeneral;
    public GameObject enplrnametxt;
    public GameObject enexitbutton; 
    public Vector3 ogcamposition;
    public bool endgamedebounce = false;
    bool finalloadcenedebounce = false; 
    endgame end;
   
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
        butexit.GetComponent<Button>().onClick.AddListener(exit);
        ogcamposition = gameObject.transform.position;
        viewotherboardui.GetComponent<Button>().onClick.AddListener(() => {
            ///add cam pos here
            ///

            Debug.Log("moving camera");
            if (OddOrEven(switcher))
            {
                gameObject.transform.position = campos.transform.position;
                switcher++;
            }
            else {
                
                gameObject.transform.position = ogcamposition;
                switcher++;
            }

        });
        createboards(boardpiece);
        
    }
    
    void phase2start()
    {
        endgameactive = true;
        butorientation.active = false;
        Debug.Log("begin phase2");
        viewotherboardui.SetActive(true);
        
        end = new endgame(p1board, p2board, gendataui, scoreui, turnui,gameObject, campos, blue);
        end.setmaterials(white);
        end.setaishipsplaced(AiShipsplaced);
        end.setaudioplr(gameObject,explosionsound, splashsound);
    }

    // Update is called once per frame
    void Update()
    {

        if (endgameactive == false)
        {
            if (phase2debounce == 1)
            {

                if (areshipsplaced())
                {
                    phase2debounce++;
                    switcher++;
                    phase2start();

                }
            }
            preshow();
        }
        else/// endgame is active now
        {
            end.preshow();

            endtime = end.getendzone();

            if (endtime != "")
            {
                prepaireendgui(endtime);


            }




        }



    }


    void prepaireendgui(string winner)
    {

        enuigeneral.SetActive(true);
        enplrnametxt.GetComponent<TextMeshProUGUI>().text= winner;
        gameObject.transform.position = endzonecam.transform.position;
        if (endgamedebounce == false)
        {
            endgamedebounce = true; 
            if (winner == "ai")
            {
                AudioSource a = gameObject.AddComponent<AudioSource>();
                a.clip = loserlosersong;
                a.loop = true;
                a.Play();


            }
            if (winner == "plr")
            {
                AudioSource a = gameObject.AddComponent<AudioSource>();
                a.clip = winnerwinnersong;
                a.loop = true;
                a.Play();
            }
        }
        enexitbutton.GetComponent<Button>().onClick.AddListener(() => {
            if (finalloadcenedebounce == false)
            {
                finalloadcenedebounce=true;
                SceneManager.LoadScene(0);
            }
        });
    }



    void exit()
    { 
     Application.Quit();
    }





    void preshow()
    {
       // int mask = 1 << LayerMask.NameToLayer("ignore");
        RaycastHit hitinfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo);
        if (hit)
        {   
            if(hitinfo.transform.GetComponent<Renderer>().material.color != Color.green)
            hitinfo.transform.GetComponent<Renderer>().material = blue;

           // Debug.Log(hitinfo.transform.name);
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
                    if (p1board[row, col].transform.GetComponent<Renderer>().material.color != Color.green)
                        p1board[row, col].transform.GetComponent<Renderer>().material = blue;

                    if (shipspace != -1)
                    {
                        preshowship(row, col);
                        if (Input.GetMouseButtonUp(0))
                        {
                            if (iscollision(row, col) == false)
                            {
                                writeonboard(row, col);
                                placeship(p1board[row, col]);
                            }
                        }


                    }
                }
            }


        }

        



    }

    bool iscollision(int row, int col)
    {
        bool collision = false; 
        if (verticle)
        {
            if (row + shipspace <= 10)
            {
                for (int count = row; count < (row + shipspace); count++)
                {
                    if (p1board[count, col].transform.GetComponent<Renderer>().material.color == Color.green)
                    {
                        //p1board[count, col].transform.GetChild(1).GetComponent<Renderer>().material = Yellow;
                        collision = true;
                    }
                }
            }
            else
            collision = true;

            return collision;
        }
        else
        {
            if (col + shipspace <= 10)
            {
                for (int count = col; count < (col + shipspace); count++)
                {
                    if (p1board[row, count].transform.GetComponent<Renderer>().material.color == Color.green)
                    {
                        //p1board[count, col].transform.GetChild(1).GetComponent<Renderer>().material = Yellow;
                        collision = true;
                    }
                }
            }
            else
            collision = true;

            return collision;
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

                    if (p1board[count, col].transform.GetComponent<Renderer>().material.color == Color.green)
                    {
                        p1board[count, col].transform.GetChild(1).GetComponent<Renderer>().material = Yellow;
                        
                    }

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

                    if (p1board[row, count].transform.GetComponent<Renderer>().material.color == Color.green)
                    {
                        p1board[row, count].transform.GetChild(1).GetComponent<Renderer>().material = Yellow;

                    }

                }
            }

        }


    }

    void writeonboard(int row, int col)
    {
        if (verticle)
        {
            if (row + shipspace <= 10)
            {
                for (int count = row; count < (row + shipspace); count++)
                {
                    p1board[count, col].transform.GetComponent<Renderer>().material.color = Color.green;
                }
            }

        }
        else
        {
            if (col + shipspace <= 10)
            {
                for (int count = col; count < (col + shipspace); count++)
                {
                    p1board[row, count].transform.GetComponent<Renderer>().material.color = Color.green;
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
                if (p1board[row, col].transform.GetComponent<Renderer>().material.color != Color.green)
                {
                    p1board[row, col].transform.GetComponent<Renderer>().material = white;
                    
                }
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



        boardpiece.transform.position = new Vector3(13, 0, 0);

        for (int col2 = 0; col2 < 10; col2++)
        {
            for (int row2 = 0; row2 < 10; row2++)
            {



                // Debug.Log("test");
                p2board[row2, col2] = GameObject.Instantiate(boardpiece);
                p2board[row2, col2].transform.GetComponent<BoxCollider>().enabled = false;
                p2board[row2, col2].transform.position = new Vector3(col2+13, boardpiece.transform.position.y, row2);
                string name = "B2[" + col2 + ":" + row2 + "]";
                p2board[row2, col2].transform.name = col2 + "A/" + row2;
                p2board[row2, col2].transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            }

        }
        AIplaceships();

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
       // Debug.Log("orientation change");
        if (verticle == true)
        {
            rotatenonplacedships(90);
            verticle = false;
        }
        else
        {
            rotatenonplacedships(-90);
            verticle = true;
        }
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
    // 1 smallest 5 biggest
    void placeship(GameObject spot)
    {
        if (shipselected == 1)
        {
            but2spacebattleship.GetComponent<Button>().enabled = false;
            but2spacebattleship.GetComponent<Image>().enabled = false;
            but2spacebattleship.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;

            if (verticle)
            {

                f2spacer.transform.position = new Vector3(spot.transform.position.x - .11f, spot.transform.position.y + .495f, spot.transform.position.z + .221f);

            }
            else
            {
                f2spacer.transform.position = new Vector3(spot.transform.position.x + .224f, spot.transform.position.y + .495f, spot.transform.position.z );
            }
            
            
        }

        if (shipselected == 2)
        {
            but3spacebattleship2.GetComponent<Button>().enabled = false;
            but3spacebattleship2.GetComponent<Image>().enabled = false;
            but3spacebattleship2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;

            if (verticle)
            {

                f3spacer2.transform.position = new Vector3(spot.transform.position.x - .058f, spot.transform.position.y + .754f, spot.transform.position.z + 1.009f);

            }
            else
            {
                f3spacer2.transform.position = new Vector3(spot.transform.position.x + .945f, spot.transform.position.y + .70f, spot.transform.position.z - 0.109f);
            }


        }

        if (shipselected == 3)
        {
            but3spacebattleship.GetComponent<Button>().enabled = false;
            but3spacebattleship.GetComponent<Image>().enabled = false;
            but3spacebattleship.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;

            if (verticle)
            {

                f3spacer1.transform.position = new Vector3(spot.transform.position.x - .058f, spot.transform.position.y + .754f, spot.transform.position.z + 1.009f);

            }
            else
            {
                f3spacer1.transform.position = new Vector3(spot.transform.position.x + .97f, spot.transform.position.y + .609f, spot.transform.position.z );
            }


        }
        if (shipselected == 4)
        {
            but4spacebattleship.GetComponent<Button>().enabled = false;
            but4spacebattleship.GetComponent<Image>().enabled = false;
            but4spacebattleship.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;

            if (verticle)
            {

                f4spacer.transform.position = new Vector3(spot.transform.position.x, spot.transform.position.y + .176f, spot.transform.position.z + 1.404f);

            }
            else
            {
                f4spacer.transform.position = new Vector3(spot.transform.position.x + 1.334f, spot.transform.position.y + .363f, spot.transform.position.z );
            }


        }
        if (shipselected == 5)
        {
            but5spacebattleship.GetComponent<Button>().enabled = false;
            but5spacebattleship.GetComponent<Image>().enabled = false;
            but5spacebattleship.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;

            if (verticle)
            {

                f5spacer.transform.position = new Vector3(spot.transform.position.x, spot.transform.position.y + 0.383f, spot.transform.position.z + 1.747f);

            }
            else
            {
                f5spacer.transform.position = new Vector3(spot.transform.position.x + 1.507f, spot.transform.position.y + 0.283f, spot.transform.position.z ); ;
            }


        }






        shipselected = -1;
       shipspace = -1;
    }

    void rotatenonplacedships(int amount)
    {
        if (but5spacebattleship.GetComponent<Button>().enabled == true)
        {
            f5spacer.transform.Rotate(0, amount, 0);
        }
        if (but4spacebattleship.GetComponent<Button>().enabled == true)
        {
            f4spacer.transform.Rotate(0, amount, 0);
        }
        if (but3spacebattleship.GetComponent<Button>().enabled == true)
        {
            f3spacer1.transform.Rotate(0, amount, 0);
        }
        if (but3spacebattleship2.GetComponent<Button>().enabled == true)
        {
            f3spacer2.transform.Rotate(0, amount, 0);
        }
        if (but2spacebattleship.GetComponent<Button>().enabled == true)
        {
            f2spacer.transform.Rotate(0, amount, 0);
        }
    }


    //0 is smallest 
    void AIplaceships()
    {
        int Aishipspace = -1;
        for (int col = 0; col < 10; col++)
        {
            for (int row = 0; row < 10; row++)
            {
                AiShipsplaced[row, col] = 0;

            }
        }
        // - 0 = water 
        printarre(AiShipsplaced);
        int randomNumberrow;
        int randomNumberol;
        bool trueorfalse; 
        int count = 0;
        while (count <5)
        {

            if (count == 0)
                Aishipspace = 2;
            if (count == 1)
                Aishipspace = 3;
            if (count == 2)
                Aishipspace = 3;
            if (count == 3)
                Aishipspace = 4;
            if (count == 4)
                Aishipspace = 5;




             randomNumberrow = Random.Range(0, 10);
             randomNumberol = Random.Range(0, 10);
            trueorfalse = randomBoolean();
            if (iscollisionAI(randomNumberrow, randomNumberol, Aishipspace, trueorfalse) == false)
            {
                WriteAiShip(randomNumberrow, randomNumberol, Aishipspace, trueorfalse);
                printarre(AiShipsplaced);
                count++;
            }
        }
        
        




    }
    void WriteAiShip(int row, int col, int Aishipspace, bool trueorfalse)
    {
        if (trueorfalse)
        {
            if (row + shipspace <= 10)
            {
                for (int count = row; count < (row + Aishipspace); count++)
                {
                    AiShipsplaced[count, col] = 1;
                }
            }

        }
        else
        {
            if (col + shipspace <= 10)
            {
                for (int count = col; count < (col + Aishipspace); count++)
                {
                    AiShipsplaced[row, count]=1;
                }
            }

        }

    }

    bool iscollisionAI(int row, int col,int Aishipspace , bool trueorfalse)
    {
        bool collision = false;
        if (trueorfalse)
        {
            if (row + Aishipspace <= 10)
            {
                for (int count = row; count < (row + Aishipspace); count++)
                {
                    if (AiShipsplaced[count, col] == 1)
                    {
                        //p1board[count, col].transform.GetChild(1).GetComponent<Renderer>().material = Yellow;
                        collision = true;
                    }
                    
                }
            }
            else
                collision = true;

            return collision;
        }
        else
        {
            if (col + Aishipspace <= 10)
            {
                for (int count = col; count < (col + Aishipspace); count++)
                {
                    if (AiShipsplaced[row, count] == 1)
                    {
                        //p1board[count, col].transform.GetChild(1).GetComponent<Renderer>().material = Yellow;
                        collision = true;
                    }
                }
            }
            else
                collision = true;

            return collision;
        }


    }

    void printarre(int[,] a)
    {
        Debug.Log(" Printing board------------------- ");
        for (int col = 0; col < 10; col++)
        {
            
            Debug.Log(AiShipsplaced[0, col]+" "+AiShipsplaced[1, col] + " "+ AiShipsplaced[2, col] + " "+ AiShipsplaced[3, col] + " "+ AiShipsplaced[4, col] + " "+ AiShipsplaced[5, col] + " "+ AiShipsplaced[6, col] + " "+ AiShipsplaced[7, col] + " "+ AiShipsplaced[8, col] + " "+AiShipsplaced[9, col] + " ");
        }
    }

    bool randomBoolean()
    {
    if (Random.value >= 0.5)
    {
        return true;
    }
    return false;
    }

    bool areshipsplaced()
    {
      // Debug.Log(but5spacebattleship.GetComponent<Button>().enabled);
        if (but5spacebattleship.GetComponent<Button>().enabled == false && but4spacebattleship.GetComponent<Button>().enabled == false && but3spacebattleship.GetComponent<Button>().enabled == false && but3spacebattleship2.GetComponent<Button>().enabled == false && but2spacebattleship.GetComponent<Button>().enabled == false)
            return true;
        else
            return false; 
    }

    bool OddOrEven(int num)
    {
        return num % 2 == 0;
    }
}




