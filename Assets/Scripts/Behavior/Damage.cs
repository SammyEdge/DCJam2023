using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public GameObject player;

    public void DealDamageToPlayer(int damage)
    {
        player.GetComponent<PlayerStats>().TakeDamage(damage);
    }

    public void ChangePlayerStamina(int stamina)
    {
        player.GetComponent<PlayerStats>().ChangeStamina(stamina);
    }
}
