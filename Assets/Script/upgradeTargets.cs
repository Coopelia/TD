using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeTargets : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> turretsList=new List<GameObject>();

    public GameObject gameManager;
    public int dx;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x -= 165;
        pos.y += 68;
        for (int i = 0; i < turretsList.Count; i++)
        {
            turretsList[i].transform.position = pos;
            pos.x += dx;
        }
    }

    public void SetTargetsList(List<TurretType> typeList)
    {
        for (int i = 0; i < turretsList.Count;)
        {
            Destroy(turretsList[i].gameObject);
            turretsList.RemoveAt(i);
        }

        for (int i = 0; i < typeList.Count; i++)
        {
            GameObject gameObject = GameObject.Instantiate(gameManager.GetComponent<TurretPrefabs>().GetGameObject(typeList[i], 0), transform);
            gameObject.GetComponent<UnityEngine.UI.Toggle>().group = this.GetComponent<UnityEngine.UI.ToggleGroup>();
            turretsList.Add(gameObject);
        }
    }
}
