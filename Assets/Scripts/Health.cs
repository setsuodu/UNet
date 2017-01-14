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
    public bool destroyOnDeath;

    private NetworkStartPosition[] spawnPoints;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isServer == false) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(this.gameObject);
                return;
            }

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
        if (!isLocalPlayer) return;
        Vector3 spawnPosition = Vector3.zero;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        }

        transform.position = spawnPosition;
    }


}
