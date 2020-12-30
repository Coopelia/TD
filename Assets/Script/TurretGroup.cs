using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretGroup : MonoBehaviour
{
    public List<GameObject> turretsListBuffer; //仅用于初始化

    [HideInInspector]
    public List<GameObject> turretsList;

    public int dx;
    public int max_num;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < turretsListBuffer.Count; i++)
        {
            GameObject gameObject = GameObject.Instantiate(turretsListBuffer[i]);
            gameObject.transform.SetParent(transform);
            gameObject.GetComponent<UnityEngine.UI.Toggle>().group = this.GetComponent<UnityEngine.UI.ToggleGroup>();
            turretsList.Add(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 pos = transform.position;
        pos.x += -11;
        pos.y += -12;
        for (int i = 0; i < turretsList.Count; i++)
        {
            turretsList[i].transform.position = pos;
            pos.x += dx;
        }
    }

    public bool AddTurret(GameObject turretToggle)
    {
        if (turretsList.Count >= max_num)
            return false;
        GameObject gameObject = GameObject.Instantiate(turretToggle);
        gameObject.transform.SetParent(transform);
        gameObject.GetComponent<UnityEngine.UI.Toggle>().group = this.GetComponent<UnityEngine.UI.ToggleGroup>();
        turretsList.Add(gameObject);
        return true;
    }

    public void RemoveTurret(int index)
    {
        if (index >= 0 && index < turretsList.Count)
        {
            Destroy(turretsList[index].gameObject);
            turretsList.RemoveAt(index);
        }
    }

    public void RemoveSeleted()
    {
        for (int i = 0; i < turretsList.Count; i++)
        {
            if(turretsList[i].GetComponent<UnityEngine.UI.Toggle>().isOn)
            {
                this.RemoveTurret(i);
                return;
            }
        }
    }
}
