using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_InputField text;
    // Start is called before the first frame update
   
    public static GameManager Instance;
    private string UserName;
    private void Awake()
    {

        
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }   
    public void StartGame()
    {
        UserName = text.text;
        SceneManager.LoadScene("main",LoadSceneMode.Single);
    }
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
    public string getUserName() { return UserName; }
     
}
