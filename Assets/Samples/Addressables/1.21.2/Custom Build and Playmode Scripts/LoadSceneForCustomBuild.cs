using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LoadSceneForCustomBuild : MonoBehaviour
{
    bool loading;

    public string sceneKey;

    public GameObject loadingCanvasGO;

    public GameObject eventSystemGO;

    public Canvas loadingCanvas;

    public TMP_Text loadingText;

    public GameObject loadingTextGO;

    bool menu = true;


    AsyncOperationHandle<SceneInstance> handle;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(this.loadingCanvasGO);
        // DontDestroyOnLoad(this.eventSystemGO);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Addressables.LoadSceneAsync(SceneKey);
        StartCoroutine(nameof(LoadSceneAsync));
    }

    IEnumerator LoadSceneAsync()
    {
        //Not allowing scene activation immediately
        handle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Single, false);

        loading = true;

        loadingTextGO.SetActive(true);

        yield return handle;



        //...

        //One way to handle manual scene activation.
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // loadingTextGO.SetActive(false);

            

            yield return handle.Result.ActivateAsync();

            loading = false;

            loadingCanvas.enabled = false;

            menu = false;
        }

        //...
    }

    private void Update()
    {
        if (loading)
        {
            loadingText.text = "Loading: " + handle.PercentComplete * 100 + "%";
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                menu = !menu;

                Cursor.visible = menu;
                Cursor.lockState = menu ? CursorLockMode.None : CursorLockMode.Locked;

                loadingCanvas.enabled = menu;
            }
        }
    }

}
