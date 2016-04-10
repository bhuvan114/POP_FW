using UnityEngine;
using System.Collections;
using System;

public class DataCollection : MonoBehaviour
{

    private int actions;
    private float timer;
    private bool isRunning;
    private String post_url = "http://posttestserver.com/post.php?dir=mpd133";

    // Use this for initialization
    void Start()
    {
        isRunning = true;
        actions = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            gameTimer();
        }
    }

    void gameTimer()
    {
        timer += Time.deltaTime;
    }

    public void setIsRunning(bool flag)
    {
        Debug.Log("Toggle Timer: " + timer);
        isRunning = flag;
        StartCoroutine(postData());
    }

    public void incrementActions()
    {
        ++actions;
        Debug.Log("Total Interactions: " + actions);
    }

    IEnumerator postData()
    {
        // Build the URL and feed it to a WWW object
        WWWForm form = new WWWForm();
        form.AddField("actions", actions.ToString());
        form.AddField("time", timer.ToString());
        WWW survey_info = new WWW(post_url, form);

        // Wait for the request to send
        yield return survey_info;

        if (survey_info.error != null)
        {
            Debug.Log(survey_info.error); // You should probably check your page for errors
                                          // Optionally print survey_info.text to get a corresponding webpage with error messages
        }
    }
}