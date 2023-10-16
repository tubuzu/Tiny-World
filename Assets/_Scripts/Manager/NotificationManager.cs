using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManager : MyMonoBehaviour
{
    public static NotificationManager Instance;

    TextMeshProUGUI message;
    public float displayTime = 3f;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null) Instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.message = GetComponentInChildren<TextMeshProUGUI>();
        this.gameObject.SetActive(false);
    }

    public void SetText(string txt)
    {
        message.text = txt;
        StopAllCoroutines();
        StartCoroutine(DisplayNotification());
    }

    IEnumerator DisplayNotification()
    {
        this.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        this.gameObject.SetActive(false);
    }
}
