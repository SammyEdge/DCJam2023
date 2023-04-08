using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLogController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;

    [SerializeField] private TMP_FontAsset originalFont;
    [SerializeField] private TMP_FontAsset shiftedFont;
    float fadeTimer = 3;

    // Start is called before the first frame update
    void Start()
    {
        logText.text = "Hello, strager!";
    }

    // Update is called once per frame
    void Update()
    {
        fadeTimer -= Time.deltaTime;
        if (fadeTimer <= 0)
        {
            logText.text = "";
            fadeTimer = 3;
        }
    }

    public void Message(string message)
    {
        logText.text = message;
        fadeTimer = 3;
    }
}
