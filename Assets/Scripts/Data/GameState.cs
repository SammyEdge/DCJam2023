using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Utils;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(Player);
        DontDestroyOnLoad(Utils);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("City", LoadSceneMode.Single);
        }
    }
}
