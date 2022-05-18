using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSE : MonoBehaviour
{
    //再生を管理する効果音をセットする変数
    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audioSource;
    private AudioSource FootSEPlay;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = this.gameObject.transform.GetComponent<AudioSource>();
    }


    public void Play(string bgmName)
    {
        switch (bgmName)
        {
            case "FootSE"://
                audioSource.clip = audioClips[0];
                audioSource.volume = 0.3f;
                audioSource.Play();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
