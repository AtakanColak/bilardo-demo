using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameScript : MonoBehaviour
{

    private TextMeshProUGUI timer;
    private float time;

    private void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Start is called before the first frame update
    void Start()
    {
        Button returnToMainMenu = GameObject.Find("ExitButton").GetComponent<Button>();
        returnToMainMenu.onClick.AddListener(delegate { MainMenu(); });
        timer = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        time = 0.0f;
        Debug.Log("Started");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timer.text = time.ToString("0.0") + " s";
    }
}
