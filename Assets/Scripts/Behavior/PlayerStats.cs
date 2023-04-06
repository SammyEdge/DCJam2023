using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health;
    public int stamina;
    public int energy;

    public TimeState timeState;

    private float energyTimer = 1, attackTimer = 3, shiftTimer = 5;
    public bool attacked = false;
    public bool shiftCooldown = false;
    //keys
    public bool redKey = false, blueKey = false;

    public int killCounter = 0;

    void Update()
    {
        if (shiftTimer < 5)
        {
            shiftTimer += Time.deltaTime;
        }
        else if (shiftTimer > 5)
        {
            shiftTimer = 5;
            shiftCooldown = false;

        }

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

        //Return to original if energy == 0
        if (timeState == TimeState.Shifted && energy <= 0)
        {
            // Make shift
            GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>().ShiftTimeState(TimeState.Original);
            ShiftCooldown();
            
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

    public void GetRedKey(bool key)
    {
        redKey = key;
        return;
    }

    public void GetBlueKey(bool key)
    {
        blueKey = key;
        return;
    }

    public void ShiftCooldown()
    {
        shiftTimer = 0;
        shiftCooldown = true;
    }
}
