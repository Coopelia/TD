using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control : MonoBehaviour
{
    public Transform trans;
    public float speed;
    public float[] aera;
    private float sensitivity;
    // Start is called before the first frame update
    void Start()
    {
        trans = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = 0, v = 0, fd = 0;
        sensitivity = 1;
        fd = Input.GetAxis("Mouse ScrollWheel") * 40;
        if (Input.GetKey(KeyCode.Mouse1))
        {
            h = -Input.GetAxisRaw("Mouse X");
            v = -Input.GetAxisRaw("Mouse Y");
            sensitivity = 2;
        }
        else
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }
        trans.Translate(new Vector3(h, -fd, v+fd) * Time.deltaTime * speed * sensitivity, Space.Self);
        Vector3 pos = trans.position;
        if (pos.x < aera[0])
            pos.x = aera[0];
        if (pos.x > aera[1])
            pos.x = aera[1];
        if (pos.z < aera[2])
            pos.z = aera[2];
        if (pos.z > aera[3])
            pos.z = aera[3];
        if (pos.y < aera[4])
            pos.y = aera[4];
        if (pos.y > aera[5])
            pos.y = aera[5];
        trans.position = pos;
    }
}
