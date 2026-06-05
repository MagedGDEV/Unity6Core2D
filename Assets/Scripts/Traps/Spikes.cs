using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float spikeDamage;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAbility ability = collision.GetComponentInParent<KnockBackAbility>();
        ability.StartKnockBack(knockBackDuration, knockBackForce, gameObject.transform);
        
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        playerStats.DamagePlayer(spikeDamage);
    }
}
