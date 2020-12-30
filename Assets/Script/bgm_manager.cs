using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgm_manager : MonoBehaviour
{
    public AudioClip[] bgm_preparation;
    private int pre_index;
    public AudioClip[] bgm_fight;
    private int fight_index;
    public AudioClip bgm_win;
    public AudioClip bgm_failed;

    private AudioSource audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        pre_index = 0;
        fight_index = 0;
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayeBgm(int id, bool isLoop) //0--胜利，1--失败
    {
        audioPlayer.Stop();
        switch (id)
        {
            case 0:
                audioPlayer.clip = bgm_win;
                break;
            case 1:
                audioPlayer.clip = bgm_failed;
                break;
            default:
                break;
        }
        audioPlayer.loop = isLoop;
        audioPlayer.Play();
    }

    public void PlayNextPreBGM()
    {
        audioPlayer.Stop();
        audioPlayer.loop = true;
        audioPlayer.clip = bgm_preparation[pre_index++];
        audioPlayer.Play();
    }

    public void PlayNextFightBGM()
    {
        audioPlayer.Stop();
        audioPlayer.loop = true;
        audioPlayer.clip = bgm_fight[fight_index++];
        audioPlayer.Play();
    }
}
