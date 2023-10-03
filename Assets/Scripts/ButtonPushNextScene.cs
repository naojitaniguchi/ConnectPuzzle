using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPushNextScene : MonoBehaviour
{
    public string nextScene;
    [SerializeField] bool waitForFade = false;
    [SerializeField] float waitTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buttonPushed()
    {
        if ( waitForFade)
        {
            StartCoroutine(WaitAndLoadScene());
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    IEnumerator WaitAndLoadScene()
    { 
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(nextScene);
    }
}
