using UnityEngine;

/// <summary>
/// Utility component that hides itself if this game is not a development build.
/// </summary>
public class DevTool : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(Debug.isDebugBuild);
    }

}
