using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private HealthBarControl healthBarControl;
    [SerializeField] private float maxHealth;
    private float currentHealth;
    private bool canTakeDamage = true;
    
    [Header("Flash")]
    [SerializeField] private float flashDuration;
    [SerializeField, Range (0,1)] private float flashStrength;
    [SerializeField] private Color flashCol;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    private SpriteRenderer spriter;
    private string flashColorGraphParameter = "_FlashColor"; 
    private string flashAmountGraphParameter = "_FlashAmount";
    private int flashColorGraphParameterInt;
    private int flashAmountGraphParameterInt;

    [Header("Stats Colliders")] 
    [SerializeField] private Collider2D crouchStatsCol;
    [SerializeField] private Collider2D standingStatsCol;
    private Collider2D currentStatsCol;
    
    private void Awake()
    {
        flashColorGraphParameterInt = Shader.PropertyToID(flashColorGraphParameter);
        flashAmountGraphParameterInt = Shader.PropertyToID(flashAmountGraphParameter);
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBarControl.SetSliderValue(currentHealth, maxHealth);
        spriter = GetComponentInParent<SpriteRenderer>();
        defaultMaterial = spriter.material;
    }

    public void DamagePlayer(float damage)
    {
        if (!canTakeDamage)
            return;
        
        currentHealth -= damage;
        healthBarControl.SetSliderValue(currentHealth, maxHealth);
        StartCoroutine(Flash());
        if (currentHealth <= 0)
        {
            if (player.stateMachine.currentState != PlayerStates.State.KnockBack)
                Debug.Log("Player is dead");
        }
    }

    private IEnumerator Flash()
    {
        canTakeDamage = false;
        spriter.material = flashMaterial;
        flashMaterial.SetColor(flashColorGraphParameterInt, flashCol);
        flashMaterial.SetFloat(flashAmountGraphParameterInt,  flashStrength);
        
        yield return new WaitForSeconds(flashDuration);
        spriter.material = defaultMaterial;
        
        if (currentHealth > 0)
            canTakeDamage = true;
    }

    public void EnableStatsStandCol()
    {
        if (currentHealth <= 0)
            return;

        crouchStatsCol.enabled = false;
        standingStatsCol.enabled = true;
        currentStatsCol = standingStatsCol;
    }

    public void EnableStatsCrouchCol()
    {
        if (currentHealth <= 0)
            return;

        crouchStatsCol.enabled = true;
        standingStatsCol.enabled = false;
        currentStatsCol = crouchStatsCol;
    }
    
    public bool GetCanTakeDamage()
    {
        return canTakeDamage;
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
