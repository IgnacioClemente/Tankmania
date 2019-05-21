using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    IEnumerator LoadLevelCorroutine(string scene)
    {
        //async es quien se encarga de cargar la escena en paralelo
        System.GC.Collect();
        yield return new WaitForSeconds(2);
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
        while (!async.isDone)
        {
            yield return null;
        }
    }

    public void LoadScene(string scene)
    {
        //primero cargo el loading y luego la escena que me dieron
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading Menu");
        StartCoroutine(LoadLevelCorroutine(scene));
    }
}
