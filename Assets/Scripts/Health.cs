using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public const int maxHealth = 100;
    [SyncVar(hook = "OnHealthChanged")]
    public int currentHealth;
    public Slider healthSlider;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isServer == false) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            RpcRespawn();
            currentHealth = maxHealth;
            Debug.Log("Death");
        }
    }

    void OnHealthChanged(int health)
    {
        healthSlider.value = health / (float)maxHealth;
    }

    [ClientRpc] //LocalPlayerss相关的
    void RpcRespawn()
    {
        //if (!isLocalPlayer) return;
        transform.position = Vector3.zero;
    }


}
