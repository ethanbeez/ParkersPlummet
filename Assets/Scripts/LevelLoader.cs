using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] Animator transition;

    [SerializeField] float transitionTime;

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab)) 
    //    {
    //        LoadNextLevel();
    //    }

    //    if (Input.GetKeyDown(KeyCode.Backspace)) 
    //    {
    //        ReplayTransition();        
    //    }
    //}

    public void LoadNextLevel() 
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ReplayTransition() 
    {
        StartCoroutine(CycleTransition());
    }

    IEnumerator LoadLevel(int levelIndex) 
    {
        transition.SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator CycleTransition() 
    {
        transition.SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionTime);

        transition.SetTrigger("FadeIn");
    }
}
