using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health;
    public int stamina;
    public int energy;

    public TimeState timeState;

    private float energyTimer = 1, attackTimer = 3;
    public bool attacked = false;
    

    void Update()
    {
        // Losing energy in shifted reality
        if (timeState == TimeState.Shifted)
        {
            energyTimer -= Time.deltaTime;
            if (energyTimer <= 0)
            {
                ChangeEnergy(-1);
                energyTimer = 1;
            }
        }

        // Attack cooldown
        if (attacked)
        {
            attackTimer -= Time.deltaTime;
            {
                if (attackTimer <= 0)
                {
                    attacked = false;
                    attackTimer = 3;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Health = " + health.ToString());
    }

    public void ChangeStamina(int fatugue)
    {
        stamina -=fatugue;
        Debug.Log("Stamina = " + stamina.ToString());
    }

    public void ChangeEnergy(int energy)
    {
        this.energy += energy;
        Debug.Log("Energy = " + this.energy.ToString());
    }
}
