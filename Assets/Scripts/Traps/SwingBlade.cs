using System.Timers;
using UnityEngine;

public class SwingBlade : MonoBehaviour
{

    [Header("Swing Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float maxAngle;
    private float timer;
    
    [Header("Knockback Settings")]
    [SerializeField] private float bladeDamage;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    private int pushDirection = 1;
    private float previousAngle = 0f;

    void Update()
    {
        timer += speed * Time.deltaTime;
        float angle = maxAngle * Mathf.Sin(timer);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
        if (angle > previousAngle)
            pushDirection = 1;
        else if (angle < previousAngle)
            pushDirection = -1;
        
        previousAngle = angle;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAbility ability = collision.GetComponentInParent<KnockBackAbility>();
        ability.StartKnockBack(knockBackDuration, knockBackForce, pushDirection);
        
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        playerStats.DamagePlayer(bladeDamage);
    }
}
