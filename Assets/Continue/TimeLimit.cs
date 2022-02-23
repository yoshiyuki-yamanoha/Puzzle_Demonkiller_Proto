using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    [SerializeField] private float timeLimit;
    float currentTime;

    [SerializeField] private Text timerText;

    void Start()
    {
        currentTime = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0) currentTime = 0;

        timerText.text = currentTime.ToString("00") + "s";
        
    }
}
