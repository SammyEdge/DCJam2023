using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health;
    public int stamina;
    public int energy;

    public TimeState timeState;

    public float attackSpeed;
    public float shiftSpeed;
    public int damage;
    public int maxHealth;
    public int maxEnergy;
    public int energyLoseRate;

    public int lightRadius;

    public int restoreEnergyCounter;

    private float energyTimer = 1, attackTimer = 3, shiftTimer = 5;
    public bool attacked = false;
    public bool shiftCooldown = false;
    //keys
    public bool redKey = false, blueKey = false;

    public int killCounterOriginal = 0;
    public int killCounterShifted = 0;

    public GameObject GameOverScreen;

    void Start()
    {
        // attack speed, the lower the better
        //attackSpeed = 3;
        // shift cooldown the lower the better
        //shiftSpeed = 5;
        // damage, the higher the better
        //damage = 1;
        // health cap, the higher the better
        //maxHealth = 50;
        // max energy cap, the higher the better
        //maxEnergy = 50;
        // energy lose rate, the higher the better
        //energyLoseRate = 1;
        // energy restore counter, the lower the better
        // restoreEnergyCounter = 10
    }


    void Update()
    {
        if (shiftTimer < shiftSpeed)
        {
            shiftTimer += Time.deltaTime;
        }
        else if (shiftTimer > shiftSpeed)
        {
            shiftTimer = shiftSpeed;
            shiftCooldown = false;

        }

        // Losing energy in shifted reality
        if (timeState == TimeState.Shifted)
        {
            energyTimer -= Time.deltaTime;
            if (energyTimer <= 0)
            {
                ChangeEnergy(-1);
                energyTimer = energyLoseRate;
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
                    attackTimer = attackSpeed;
                }
            }
        }

        //Return to original if energy == 0
        if (timeState == TimeState.Shifted && energy <= 0 && !gameObject.GetComponentInParent<PlayerMovement>().trueMove)
        {
            // Make shift back
            gameObject.GetComponentInParent<PlayerLogController>().Message("Low energy!");
            gameObject.GetComponentInParent<PlayerSoundController>().UnShiftSound();
            GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>().ShiftTimeState(TimeState.Original);
            ShiftCooldown();

        }

        // check death
        if (timeState == TimeState.Original && health <= 0 && !gameObject.GetComponentInParent<PlayerMovement>().trueMove && !shiftCooldown)
        {
            // last chance
            ChangeEnergy(5);
            gameObject.GetComponentInParent<PlayerLogController>().Message("You are dying! You need to restore your health now!");
            gameObject.GetComponentInParent<PlayerSoundController>().ShiftSound();
            GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>().ShiftTimeState(TimeState.Shifted);
        }
        else if (timeState == TimeState.Original && health <= 0 && !gameObject.GetComponentInParent<PlayerMovement>().trueMove && shiftCooldown)
        {
            // GameOver
            Time.timeScale = 0;
            gameObject.GetComponentInParent<PlayerLogController>().Message("You are dead!");
            GameOverScreen.SetActive(true);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            health = 0;
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        Debug.Log("Health = " + health.ToString());
    }

    public void ChangeStamina(int fatugue)
    {
        stamina -= fatugue;
        Debug.Log("Stamina = " + stamina.ToString());
    }

    public void ChangeEnergy(int energy)
    {
        this.energy += energy;

        if (energy < 0)
        {
            energy = 0;
        }

        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }

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
