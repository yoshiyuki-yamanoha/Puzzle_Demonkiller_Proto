using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //[SerializeField]
    private int frameRate = 60;
    [SerializeField]
    private Text fpsText;
    private float Interval = 0.1f;
    private float _time_cnt;
    private int _frames;
    private float _time_mn;
    private float _fps;
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = frameRate;
        fpsText = GameObject.Find("FPS").GetComponent<Text>();
    }
    void Update()
    {
        _time_mn -= Time.deltaTime;
        _time_cnt += Time.timeScale / Time.deltaTime;
        _frames++;

        if (0 < _time_mn) return;
        //Debug.Log(_fps);
        _fps = _time_cnt / _frames;
        _time_mn = Interval;
        _time_cnt = 0;
        _frames = 0;

        fpsText.text = "FPS: " + _fps.ToString("f2");
    }
}
