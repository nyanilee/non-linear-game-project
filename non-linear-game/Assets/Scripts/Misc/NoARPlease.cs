using UnityEngine;

using Vuforia;

public class NoARPlease : MonoBehaviour {
    [SerializeField]
    private CameraClearFlags cameraClearFlags;

    // Use this for initialization
    private void Start() {
        var mainCamera = Camera.main;
        if (mainCamera) {
            if (mainCamera.GetComponent<VuforiaBehaviour>() != null) {
                mainCamera.GetComponent<VuforiaBehaviour>().enabled = false;
            }

            if (mainCamera.GetComponent<VideoBackgroundBehaviour>() != null) {
                mainCamera.GetComponent<VideoBackgroundBehaviour>().enabled =
                    false;
            }

            if (mainCamera.GetComponent<DefaultInitializationErrorHandler>()
                != null) {
                mainCamera.GetComponent<DefaultInitializationErrorHandler>()
                    .enabled = false;
            }

            mainCamera.clearFlags = this.cameraClearFlags;
        }
    }
}