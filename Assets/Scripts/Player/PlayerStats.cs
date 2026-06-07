using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private HealthBarControl healthBarControl;
    [SerializeField] private float maxHealth;
    private float currentHealth;
    
    [Header("Flash")]
    [SerializeField] private float flashDuration;
    [SerializeField, Range (0,1)] private float flashStrength;
    [SerializeField] private Color flashCol;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    private SpriteRenderer spriter;
    private bool canTakeDamage;
    private string flashColorGraphParameter = "_FlashColor"; 
    private string flashAmountGraphParameter = "_FlashAmount";
    private int flashColorGraphParameterInt;
    private int flashAmountGraphParameterInt;

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
        spriter.material = flashMaterial;
        flashMaterial.SetColor(flashColorGraphParameterInt, flashCol);
        flashMaterial.SetFloat(flashAmountGraphParameterInt,  flashStrength);
        
        yield return new WaitForSeconds(flashDuration);
        spriter.material = defaultMaterial;
    }
    
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
