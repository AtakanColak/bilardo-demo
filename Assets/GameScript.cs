using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameScript : MonoBehaviour
{
    private Transform MainCamera;
    private Transform whiteball;
    private TextMeshProUGUI timer;
    private float time;
    private float sensitivity = 1;
    private Vector3 dragOrigin;

    private Vector3 cameraFocus()
    {
        return whiteball.position + new Vector3(0, 0.5f, 0);
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void Start()
    {
        time = 0.0f;
        Button returnToMainMenu = GameObject.Find("ExitButton").GetComponent<Button>();
        returnToMainMenu.onClick.AddListener(delegate { MainMenu(); });
        timer = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>().transform;
        whiteball = GameObject.Find("WhiteBall").transform;
        MainCamera.position = whiteball.position + new Vector3(-2, 1f, 0);
    }

    void Update()
    {
        time += Time.deltaTime;
        timer.text = time.ToString("0.0") + " s";
        MainCamera.LookAt(cameraFocus());
        //if (Input.GetMouseButtonDown(0))
        //{
        //    dragOrigin = Input.mousePosition;
        //    return;
        //}
        if (!Input.GetMouseButton(0)) { dragOrigin = Input.mousePosition;  return; }
        Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);

        MainCamera.RotateAround(cameraFocus(), -Vector3.up, pos.x* sensitivity);
        //float rotateHorizontal = Input.GetAxis("Mouse X");
        //MainCamera.RotateAround(cameraFocus(), -Vector3.up, rotateHorizontal * sensitivity);
    }
}
