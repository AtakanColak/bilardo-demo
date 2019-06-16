using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameScript : MonoBehaviour
{
    public Material mat;
    private Transform camera;
    private Transform whiteball;
    private Transform cuestick;
    private LineRenderer line;
    private TextMeshProUGUI timer;
    private Vector3 dragOrigin;
    private float time;
    private float sensitivity = 1;
    private float counter = 0.0f;
    private float linespeed = 6f;
    private float linedist;
    private float max = 0.0f;
    private Vector3 cameraFocus()
    {
        return whiteball.position + 2 * direction() + new Vector3(0, 1, 0);
    }

    private Vector3 direction()
    {
        return (whiteball.position - camera.position);
    }

    private Vector3 RemoveY(Vector3 vector) {
        return vector - new Vector3(0, vector.y, 0);
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
        line = GameObject.Find("Line").GetComponent<LineRenderer>();
        Button returnToMainMenu = GameObject.Find("ExitButton").GetComponent<Button>();
        returnToMainMenu.onClick.AddListener(delegate { MainMenu(); });
    }

    private bool Deflect(Ray ray, out Ray deflected, out RaycastHit hit)
    {
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 normal = hit.normal;
            Vector3 deflect = Vector3.Reflect(ray.direction, normal);

            deflected = new Ray(hit.point, deflect);
            return true;
        }
        deflected = new Ray(Vector3.zero, Vector3.zero);
        return false;
    }

    private void DrawLine()
    {
        line.transform.position = whiteball.position;
        line.SetPosition(0, whiteball.position);
        Vector3 forward = RemoveY(direction());
        Ray a = new Ray(whiteball.position, forward);
        Ray b;
        Ray c; 
        RaycastHit hit;
        if (Deflect(a, out b, out hit))
        {
            Vector3 originDir = b.origin - a.origin;
            if (originDir.magnitude >= max)
            {
                Vector3 maxLen = a.origin + Vector3.Normalize(originDir) * max;
                line.SetPosition(1, maxLen);
                line.SetPosition(2, maxLen);
            }
            else
            {
                float remaining = max - originDir.magnitude;
                line.SetPosition(1, b.origin);
                Vector3 bDir = Vector3.Normalize(RemoveY(b.direction));
                line.SetPosition(2, b.origin + bDir * remaining);
                if (Deflect(new Ray(b.origin, bDir), out c, out hit))
                {
                    Vector3 next = c.origin - b.origin;
                    if (next.magnitude < remaining)
                    {
                        line.SetPosition(2, c.origin);
                    }  
                }
            }    
        }
    }

    private void PositionStick()
    {
        cuestick.position = camera.position + new Vector3(0, -0.5f, 0) + -0.5f * direction();
        cuestick.LookAt(whiteball);
        DrawLine();
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

        if (!Input.GetMouseButton(0)) { dragOrigin = Input.mousePosition; return; }
        Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
        camera.RotateAround(whiteball.position, -Vector3.up, pos.x * sensitivity);
        max = pos.y * 10;
        if (max > 5.0f) max = 5.0f;
        camera.LookAt(cameraFocus());
        PositionStick();

    }
}
