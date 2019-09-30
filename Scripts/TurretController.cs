using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

    public bool isFiring;

    public ShellController shell;
    public float shellSpeed;
    public Transform shellStart;

    public float timeBetweenShots;
    private float shotCounter;

    void Update()
    {
        if (isFiring) //if shots are being fired then the shot counter will be counting down
        {
            shotCounter -= Time.deltaTime; 
            if (shotCounter <= 0) //fires bullet and resets shotcounter if it goes below or equal to 0
            {
                shotCounter = timeBetweenShots;
                ShellController newShell = Instantiate(shell, shellStart.position, shellStart.rotation) as ShellController; //creates a new shell at the shell start position
                newShell.speed = shellSpeed;
            }
        }
        else
        {
            shotCounter = 0;
        }
    }
}
