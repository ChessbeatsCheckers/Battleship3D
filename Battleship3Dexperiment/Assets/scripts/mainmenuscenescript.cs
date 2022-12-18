using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class mainmenuscenescript : MonoBehaviour
{

    public GameObject enterbutton;


    // Start is called before the first frame update
    void Start()
    {
        enterbutton.GetComponent<Button>().onClick.AddListener(() => {

            SceneManager.LoadScene(1);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
