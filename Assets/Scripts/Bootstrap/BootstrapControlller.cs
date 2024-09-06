using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BootstrapControlller : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;

    void Start()
    {
        GameScenesManager.Instance.OnSceneLoadingProgressUpdated += OnSceneLoadingProgressUpdated;
        GameScenesManager.Instance.LoadMenu();
    }

    private void OnSceneLoadingProgressUpdated(float value)
    {
        loadingSlider.value = value;
    }

    private void OnDestroy()
    {
        if(GameScenesManager.Instance != null)
        {
            GameScenesManager.Instance.OnSceneLoadingProgressUpdated -= OnSceneLoadingProgressUpdated;
        }
    }
}
