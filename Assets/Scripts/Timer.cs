using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI currentTime;
    public TextMeshProUGUI bestTime;

    public GameObject door;

    private float elapsed;
    private bool endOfLevel;
    [Space]
    [SerializeField]
    private string bestTimePP = "BestTime";

    private void Start()
    {
        float _best = PlayerPrefs.GetFloat(bestTimePP);
        bestTime.text = FormatSeconds(_best);
    }
    // Update is called once per frame
    void Update()
    {
        EndOfLevel(door);
        if (!endOfLevel)
            CurrentTimeUpdate();
        else
            BestTimeUpgrade();
    }

    void CurrentTimeUpdate()
    {
        elapsed += Time.deltaTime;
        currentTime.text = FormatSeconds(elapsed);
    }

    void BestTimeUpgrade()
    {
        float _bestTime = PlayerPrefs.GetFloat(bestTimePP);

        if (_bestTime.Equals(0))
        {
            PlayerPrefs.SetFloat(bestTimePP, elapsed);
            bestTime.text = FormatSeconds(elapsed);
        } else if(_bestTime > elapsed)
        {
            PlayerPrefs.SetFloat(bestTimePP, elapsed);
            bestTime.text = FormatSeconds(elapsed);
        }            
    }

    bool EndOfLevel(GameObject _door)
    {
        return _door.activeInHierarchy ? endOfLevel = true : endOfLevel = false;
    }

    string FormatSeconds(float _elapsed)
    {
        int d = (int)(_elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        int hundredths = d % 100;
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
    }

}
