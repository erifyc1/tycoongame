using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int balance;
    private Text balanceText;
    [SerializeField] int FPS;
    private int workingFps;
    private float timer;
    void Start()
    {
        balanceText = GameObject.FindGameObjectWithTag("balanceText").GetComponent<Text>();
        SetScore(0);
        AddScore(10);
    }

    public void SetScore(int amount)
    {
        balance = amount;
        balanceText.text = "Balance\n$" + balance;
    }

    public void AddScore(int amount)
    {
        balance += amount;
        balanceText.text = "Balance\n$" + balance;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        workingFps++;
        if (timer > 1)
        {
            FPS = workingFps;
            workingFps = 0;
            timer = 0;
        }
    }

    public void RunCoroutine(IEnumerator ienum)
    {
        StartCoroutine(ienum);
    }
}
