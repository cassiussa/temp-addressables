using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadProgress : MonoBehaviour
{
    public int downloadProgressInput;
    private int cachedDownloadProgressInput;
    public int downloadProgressOutput;

    void Start()
    {
        downloadProgressOutput = 0;
    }
    
    void Update()
    {
        if (cachedDownloadProgressInput != downloadProgressInput)
        {
            downloadProgressOutput = downloadProgressInput;
            cachedDownloadProgressInput = downloadProgressInput;
        }
    }
}
