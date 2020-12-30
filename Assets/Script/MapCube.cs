using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour
{
    [HideInInspector]
    public GameObject current_turret;
    public GameObject buildEffect;

    private Renderer render;
    private Color t_color; //当前颜色
    private Color source_color; //最初的颜色
    private Color before_seleted_color;
    private bool isSeleted;
    private void Start()
    {
        render = this.GetComponent<Renderer>();
        t_color = render.material.color;
        source_color = render.material.color;
        before_seleted_color = render.material.color;
        isSeleted = false;
        render.enabled = false;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            render.material.color = t_color;
    }

    public GameObject BuildTurret(GameObject turret)
    {
        current_turret = turret;
        current_turret.transform.position = transform.position;
        current_turret.GetComponent<Turret>().SetAct(true);
        //current_turret = GameObject.Instantiate(turret, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1);
        render.enabled = true;
        render.material.color = new Color(0, 0.8f, 0.8f, 0.7f);
        t_color = render.material.color;
        return current_turret;
    }

    public GameObject UpgradedTurret(GameObject turret)
    {
        GameObject old = current_turret;
        current_turret = GameObject.Instantiate(turret, transform.position, old.transform.rotation);
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(old);
        Destroy(effect, 1);
        render.material.color = new Color(0, 0.8f, 0.8f, 0.7f);
        t_color = render.material.color;
        return current_turret;
    }

    public GameObject SeparateTurret()
    {
        GameObject obj = current_turret;
        current_turret = null;
        render.material.color = source_color;
        t_color = source_color;
        render.enabled = false;
        return obj;
    }

    public void DestroyTurret()
    {
        //弹回装备
        int[] es = current_turret.GetComponent<Turret>().equips;
        for (int j = 0; j < es.Length; j++)
        {
            if (es[j] != -1)
                GameObject.Find("GameManager").GetComponent<build_manager>().equipBag.GetComponent<EquipBag>().AddEquip(es[j]);
        }
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        render.material.color = source_color;
        t_color = source_color;
        Destroy(current_turret);
        Destroy(effect, 1);
        render.enabled = false;
    }

    public void Seleted()
    {
        this.isSeleted = true;
        render.enabled = true;
        before_seleted_color = t_color;
        render.material.color = new Color(1, 0, 0, 0.7f);
        t_color = render.material.color;
    }

    public void UnSeleted()
    {
        this.isSeleted = false;
        if (current_turret == null)
            render.enabled = false;
        render.material.color = before_seleted_color;
        t_color = render.material.color;
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            render.enabled = true;
            if (current_turret == null)
                render.material.color = new Color(1.0f, 0.4f, 0.7f, 1.0f);
            else
                render.material.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        }
    }
    private void OnMouseExit()
    {
        if (current_turret == null)
            render.enabled = false;
        render.material.color = t_color;
    }
}
