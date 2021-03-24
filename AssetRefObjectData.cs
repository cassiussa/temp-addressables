using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


public class AssetRefObjectData : MonoBehaviour
{
    [SerializeField] private AssetReference _scene;
    [SerializeField] private List<AssetReference> _references = new List<AssetReference>();
    public GameObject camera;
    private AsyncOperationHandle<SceneInstance> handle;
    public DownloadProgress downloadProgressScript;
    private float downloadStartPoint;
    private bool downloadStartPointAttained;
    public GameObject uiGameObject;
    long Downloadsize = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StartCoroutine(DownloadScene());
    }

    IEnumerator DownloadScene()
    {
        var downloadScene = Addressables.LoadSceneAsync(_scene, LoadSceneMode.Additive);
        downloadScene.Completed += SceneDownloadComplete;
        Debug.LogError("Starting scene download");
        while (!downloadScene.IsDone)
        {
            var status = downloadScene.GetDownloadStatus();
            float progress = status.Percent;
            downloadProgressScript.downloadProgressInput = (int)(progress * 100);
            yield return null;
        }
        Debug.LogError("Download Complete, starting next scene");
        downloadProgressScript.downloadProgressInput = 100;
    }

    private void SceneDownloadComplete(AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> _handle) // ref to AsyncOperationHandle
    {
        if (_handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log(_handle.Result.Scene.name + " successfully loaded.");
            camera.SetActive(false);
            handle = _handle;
            uiGameObject.SetActive(false);
            StartCoroutine(UnloadScene());
        }
    }

    private IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(10);
        Addressables.UnloadSceneAsync(handle, true).Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                camera.SetActive(true);
                uiGameObject.SetActive(true);
                Debug.LogError("Successfull unloaded the scene");
            }
        };
        yield return new WaitForSeconds(5);
        //_scene.LoadSceneAsync(LoadSceneMode.Additive).Completed += SceneDownloadComplete;
        StartCoroutine(DownloadScene());
    }
}
