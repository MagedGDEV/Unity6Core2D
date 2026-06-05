using System;
using Unity.VisualScripting;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float spikeDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        playerStats.DamagePlayer(spikeDamage);
    }
}
