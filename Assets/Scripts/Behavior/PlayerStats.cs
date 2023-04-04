using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health;
    public int stamina;
    public int energy;

    public TimeState timeState;
    
    private void Start() 
    {
        //Debug.Log("Health = " + health.ToString());
        //Debug.Log("Stamina = " + stamina.ToString());
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
