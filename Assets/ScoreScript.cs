using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    private bool red = false;
    private bool yellow = false;

    private TextMeshProUGUI scorer;

    // Start is called before the first frame update
    void Start()
    {
        scorer = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
    }

    void Scored()
    {
        int i;
        int.TryParse(scorer.text, out i);
        scorer.text = (i + 1).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (red && yellow) {
            red = false;
            yellow = false;
            Scored();
        }

        if (this.GetComponent<Rigidbody>().IsSleeping()) {
            red = false;
            yellow = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RedBall")
            red = true;
        if (collision.gameObject.name == "YellowBall")
            yellow = true;
    }
}
