using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text ScoreTextRanking;
    public GameObject GameOverText;
    private PlayerScore HighScore;
    private PlayerScore SelfHighScore;
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(GameManager.Instance.gameObject);
        HighScore = getHighScore();
        if (HighScore != null )
        ScoreTextRanking.text = "Best Score: " +HighScore.score  + " by Player " + HighScore.playerName;

     
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private PlayerScore getHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<PlayerScore>(json);

        }
        else
        {
            return null;
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                if (HighScore != null)
                {
                    if (m_Points > HighScore.score)
                    {
                        PlayerScore ps = new PlayerScore(GameManager.Instance.getUserName(), m_Points);
                        setHighScore(ps);
                    }
                }
                else
                {
                    PlayerScore ps = new PlayerScore(GameManager.Instance.getUserName(), m_Points);
                    setHighScore(ps);
                }
                SceneManager.LoadScene("main",LoadSceneMode.Single);
            }
        }
    }

    private void setHighScore(PlayerScore playerScore)
    {
        HighScore = playerScore;
        string json = JsonUtility.ToJson(playerScore);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points} by {GameManager.Instance.getUserName()}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
