using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverInfo : MonoBehaviour
{
    public Text content;

    public void SetTextAndShow(string s)
    {
        content.text = s;
        this.gameObject.SetActive(true);
    }

    public void OnClickedOk()
    {
        this.gameObject.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }
}
