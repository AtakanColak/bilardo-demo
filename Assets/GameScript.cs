using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameScript : MonoBehaviour
{
    private Transform camera;
    private Transform whiteball;
    private Transform cuestick;
    private TextMeshProUGUI timer;
    private float time;
    private float sensitivity = 1;
    private Vector3 dragOrigin;

    private Vector3 cameraFocus()
    {
        return whiteball.position + 2 * (whiteball.position - camera.position) + new Vector3(0, 1, 0);
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void LoadGameObjects()
    {
        timer = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>().transform;
        whiteball = GameObject.Find("WhiteBall").transform;
        cuestick = GameObject.Find("CueStick").transform;
        Button returnToMainMenu = GameObject.Find("ExitButton").GetComponent<Button>();
        returnToMainMenu.onClick.AddListener(delegate { MainMenu(); });
    }

    private void PositionStick()
    {
        cuestick.position = camera.position + new Vector3(0, -0.5f, 0) + -0.5f * (whiteball.position - camera.position);
        cuestick.LookAt(whiteball);
    }

    void Start()
    {
        LoadGameObjects();
        time = 0.0f;
        camera.position = whiteball.position + new Vector3(-2, 1f, 0);
        camera.LookAt(cameraFocus());
        PositionStick();
    }

    void Update()
    {
        time += Time.deltaTime;
        timer.text = time.ToString("0.0") + " s";
       
        if (!Input.GetMouseButton(0)) { dragOrigin = Input.mousePosition;  return; }
        Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
        camera.RotateAround(whiteball.position, -Vector3.up, pos.x* sensitivity);
        camera.LookAt(cameraFocus());
        PositionStick();   
    }
}
