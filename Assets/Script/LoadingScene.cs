using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private int n_progressValue;
    private AsyncOperation operation;

    public Slider slider;
    public Text tip;

    // Start is called before the first frame update
    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Loading")
        {
            StartCoroutine(AsyncLoading());
        }
    }

    IEnumerator AsyncLoading()
    {
        operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main");
        operation.allowSceneActivation = false;
        yield return operation;
    }

    // Update is called once per frame
    void Update()
    {
        if (n_progressValue < 100)
        {
            n_progressValue++;
        }

        tip.text = "正在加载(" + n_progressValue.ToString() + "%)";
        slider.value = (float)n_progressValue / 100;

        if (n_progressValue == 100)
        {
            tip.text = "加载完成";
            operation.allowSceneActivation = true;
        }
    }
}
