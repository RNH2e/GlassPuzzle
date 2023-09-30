using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject succes,fail;
    float newDly;
    bool callSucces, callFail;
    public Text text;
    int i=1;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            succes.gameObject.SetActive(false);

        }
        if (callFail || callSucces)
        {
            newDly += Time.deltaTime*2;
            Debug.Log(newDly);

        }
        LevelInfo();

    }
    public void CheckSucces(float dly)
    {
        callSucces = true;
        if (newDly>dly)
        {
            succes.gameObject.SetActive(true);
            newDly = 0;
        }
     
        return;
    }
    public void CheckFail(float delay)
    {
        callFail = true;
        if (newDly > delay)
        {
            Debug.Log("faillll");
            fail.gameObject.SetActive(true);
            newDly = 0;
        }
        return;
    }

    public void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.LoadScene(0);

        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
    }
    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void LevelInfo()
    {
        text.text = "LEVEL " + SceneManager.GetActiveScene().buildIndex.ToString();
    }
}
