using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameScript : MonoBehaviour
{
    private Vector3 cameraOffset = new Vector3(-2, 1f, 0);
    private Vector3 cuestickOffset = new Vector3(-2.2f, 0.5f, 0);
    private Vector3 cuestickOutside = new Vector3(-7, 1, -1);

    public AudioClip hit;
    private AudioSource source;

    public Material mat;
    private Rigidbody ballbody, redbody, yellowbody;
    private Slider slider;
    private Transform camera;
    private Transform whiteball, redball, yellowball;
    private Transform cuestick;
    private LineRenderer line;
    private TextMeshProUGUI timer;
    private Vector3 dragOrigin;
    private float time = 0.0f;
    private float sensitivity = 1.0f;
    private float counter = 0.0f;
    private float linespeed = 6f;
    private float linedist;
    private float max = 0.0f;
    private float cuemove = 0.0f;
    private bool dir = false;
    private bool spacePress = false;

    private Vector3 cameraFocus()
    {
        return whiteball.position + 2 * direction() + new Vector3(0, 1, 0);
    }

    private Vector3 direction()
    {
        return (whiteball.position - camera.position);
    }

    private Vector3 RemoveY(Vector3 vector)
    {
        return vector - new Vector3(0, vector.y, 0);
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void LoadGameObjects()
    {
        source = GetComponent<AudioSource>();
        hit = Resources.Load("hitsound") as AudioClip;
        timer = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>().transform;
        whiteball = GameObject.Find("WhiteBall").transform;
        redball = GameObject.Find("RedBall").transform;
        yellowball = GameObject.Find("YellowBall").transform;
        ballbody = GameObject.Find("WhiteBall").GetComponent<Rigidbody>();
        redbody = GameObject.Find("RedBall").GetComponent<Rigidbody>();
        yellowbody = GameObject.Find("YellowBall").GetComponent<Rigidbody>();
        cuestick = GameObject.Find("CueStick").transform;
        slider = GameObject.Find("SoundSlider").GetComponent<Slider>();
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
        cuestick.position = camera.position + new Vector3(0, -0.5f, 0) + -0.5f * (whiteball.position - camera.position);
        cuestick.LookAt(whiteball);
        //DrawLine();
    }

    private void CameraLook()
    {
        Vector3 d = whiteball.position - camera.position;
        Vector3 o = new Vector3(0, 1, 0);
        Vector3 f = whiteball.position + 2 * d + o;
        camera.LookAt(f);
    }

    private void CueStickAim()
    {
        Vector3 d = whiteball.position - camera.position;
        Vector3 o = new Vector3(0, -0.5f, 0);
        cuestick.position = camera.position + o + -0.5f * d;
        cuestick.LookAt(whiteball);
        cuemove = 0.0f;
        dir = false;
    }

    private void PositionInitial()
    {
        whiteball.position = new Vector3(-4, 0.01f, 0);
        camera.position = whiteball.position + cameraOffset;
        cuestick.position = whiteball.position + cuestickOffset;
    }

    void Start()
    {
        LoadGameObjects();
        PositionInitial();
        CameraLook();
        CueStickAim();
    }

    void Update()
    {
        time += Time.deltaTime;
        timer.text = time.ToString("0.0") + " s";
        CameraLook();
        if (Vector3.Distance(whiteball.position, camera.position) > 100) {
            ballbody.velocity = new Vector3(0, 0, 0);
            PositionInitial();
            CueStickAim();
        }

        if (Vector3.Distance(yellowball.position, camera.position) > 100) {
            yellowbody.velocity = new Vector3(0, 0, 0);
            yellowball.position = new Vector3(2, 0.01f, 1);
        }

        if (Vector3.Distance(redball.position, camera.position) > 100)
        {
            redbody.velocity = new Vector3(0, 0, 0);
            redball.position = new Vector3(2, 0.01f, -1);
        }

        if (!ballbody.IsSleeping() || !redbody.IsSleeping() || !yellowbody.IsSleeping())
        {
            if (spacePress)
                return;
        }

        if (Input.GetKey("space"))
        {
            spacePress = true;
            if (cuemove <= 0.0f)
                dir = false;
            if (cuemove >= 1.0f)
                dir = true;

            //cuemove += dir ? -0.01f : 0.01f;
            float m = dir ? 0.01f : -0.01f;
            cuemove -= m;
            cuestick.position += m * Vector3.Normalize(whiteball.position - cuestick.position);
            cuestick.LookAt(whiteball);
            return;
        }
        
        else if (Input.GetKeyUp("space"))
        {
            cuestick.position = cuestickOutside;
            
            source.PlayOneShot(hit, slider.value);
            ballbody.AddForce(1.25f * cuemove * RemoveY(direction()), ForceMode.Impulse);
        }
        
        else if (spacePress)
        {
            camera.position = whiteball.position + cameraOffset;
            cuestick.position = whiteball.position + cuestickOffset;
            CameraLook();
            CueStickAim();
            spacePress = false;
        }


        if (!Input.GetMouseButton(0))
        {
            dragOrigin = Input.mousePosition;
            line.enabled = false;
            return;
        }
        line.enabled = true;
        Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
        camera.RotateAround(whiteball.position, -Vector3.up, pos.x * sensitivity);
        max = pos.y * 10;
        if (max > 5.0f) max = 5.0f;
        CameraLook();
        CueStickAim();
        DrawLine();
    }
}
