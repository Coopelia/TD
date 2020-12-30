using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    private float[] pos = new float[2];
    private int index;
    private float speed;
    private bool isMoving;

    public GameObject gameManager;
    public GameObject info_content;
    public Button bt;
    // Start is called before the first frame update
    void Start()
    {
        pos[0] = info_content.transform.position.x;
        pos[1] = pos[0] - 300;
        index = 0;
        speed = 300.0f;
        isMoving = false;
        OnPanelClicked(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfo();
        if (isMoving)
        {
            float tpos = info_content.transform.position.x;
            if ((index == 1 && tpos <= pos[1]) || (index == 0 && tpos >= pos[0]))
            {
                isMoving = false;
            }
            else
            {
                info_content.transform.Translate(new Vector3(pos[index] - tpos, 0, 0).normalized * speed * Time.deltaTime);
            }
        }
    }

    public void OnPanelClicked(bool isOn)
    {
        bt.transform.Rotate(Vector3.forward, 180);
        isMoving = true;
        index = ((index + 1) % 2);
    }

    private void UpdateInfo()
    {
        List<GameObject> onStateTurretCubes = gameManager.GetComponent<build_manager>().onStateTurretCubes;
        List<EntryData> entries = new List<EntryData>();
        Turret turret = null;
        for (int i = 0; i < onStateTurretCubes.Count; i++)
        {
            EntryData entry = new EntryData();
            turret = onStateTurretCubes[i].GetComponent<MapCube>().current_turret.GetComponent<Turret>();
            entry.icon = turret.head_icon;
            entry._name = turret._name;
            entry.number = turret.dps;
            entries.Add(entry);
        }
        info_content.GetComponent<Info_content>().UpdateEntries(entries);
    }
}
