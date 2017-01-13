using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    void Start()
    {

    }

    void Update()
    {
        if (!isLocalPlayer) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up * h * 120 * Time.deltaTime);
        transform.Translate(Vector3.forward * v * 3 * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    public override void OnStartLocalPlayer()
    {
        //这个方法只会在本地角色那里调用，当创建角色时
        //base.OnStartLocalPlayer();
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    [Command] //服务端执行
    void CmdFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation); 
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10f;
        Destroy(bullet, 2f);

        NetworkServer.Spawn(bullet);
    }
}
