using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info_content : MonoBehaviour
{
    public GameObject entryPrefab;
    private List<GameObject> entries = new List<GameObject>();
    private float left;
    private float top;
    private float dy;

    // Start is called before the first frame update
    void Start()
    {
        left = 0;
        top = -118;
        dy = 24;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdatePosition()
    {
        Vector3 pos = transform.position;
        pos.x += left;
        pos.y -= top;
        for (int i = 0; i < entries.Count; i++)
        {
            entries[i].transform.position = pos;
            pos.y -= dy;
        }
    }

    public void UpdateEntries(List<EntryData> new_ets)
    {
        for (int i = 0; i < entries.Count; i++)
            Destroy(entries[i]);
        entries.Clear();
        for (int i = 0; i < new_ets.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(entryPrefab, transform);
            obj.GetComponent<Entry>().icon.sprite = new_ets[i].icon;
            obj.GetComponent<Entry>()._name.text = new_ets[i]._name;
            obj.GetComponent<Entry>().number.text = new_ets[i].number.ToString();
            entries.Add(obj);
        }
        UpdatePosition();
    }
}
