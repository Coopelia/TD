using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [HideInInspector]
    public int[] damage; //子弹伤害数组

    [HideInInspector]
    public GameObject turret; //发射的炮塔

    public float speed; //子弹速度
    public Transform target; //攻击目标
    public GameObject boomEffect;  //爆炸特效

    public float lifeTime; //子弹生存时间

    private void Awake()
    {
        damage = new int[6];
    }

    public void SetTarget(Transform tar)
    {
        target = tar;
        transform.LookAt(target.position);
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            int fd = other.GetComponent<enemy>().TakeDamage(damage);
            turret.GetComponent<Turret>().GetFinalDamage(fd);
            GameObject be = GameObject.Instantiate(boomEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
            Destroy(be, 1);
            //Debug.Log("hitted");
        }
    }
}
