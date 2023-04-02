using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBars : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider healthBar;
    public Slider staminaBar;
    public GameObject player;
    PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
         playerStats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerStats.health;
        staminaBar.value = playerStats.stamina;
    }
}
