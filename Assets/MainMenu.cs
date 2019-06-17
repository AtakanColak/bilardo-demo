using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;


public class MainMenu : MonoBehaviour
{
    private TextMeshProUGUI time;
    private TextMeshProUGUI hits;
    private string filename = "gamedata.json";

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Start() {
        time = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
        hits  = GameObject.Find("Hits").GetComponent<TextMeshProUGUI>();
        StreamReader reader = new StreamReader(filename);
        string json = reader.ReadToEnd();
        reader.Close();
        GameData g = JsonUtility.FromJson<GameData>(json);
        time.text = g.time.ToString();
        hits.text = g.hits.ToString();
    }

    [System.Serializable]
    public class GameData
    {
        public int score;
        public float time;
        public int hits;
    }
}
