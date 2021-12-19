using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InScene : MonoBehaviour
{
    private GameObject gameboyCamera;

    private GameObject originalCamera;

    private GameObject blackAndWhiteTexture;

    private GameObject blackAndWhiteCamera;

    private Player player;

    private int currentLevel;

    /// <summary>
    /// Camera current state
    /// </summary>
    private ECameraState currentState;

    // Start is called before the first frame update
    public void Start()
    {
        //var go = new GameObject("Player");
        //this.player = go.AddComponent<Player>();
        //go.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        float smoothTime = 0.1f;
        Vector3 velocity = Vector3.zero;
        gameboyCamera.transform.position = Vector3.SmoothDamp(gameboyCamera.transform.position, new Vector3(0, currentLevel * -10, -10), ref velocity, smoothTime);
    }

    public void SetGameboyCamera(GameObject camera)
    {
        gameboyCamera = camera;
    }

    public void SetOriginalCamera(GameObject camera)
    {
        originalCamera = camera;
    }

    public void SetBlackAndWhiteTextureObject(GameObject blackAndWhiteTextureLocation)
    {
        blackAndWhiteTexture = blackAndWhiteTextureLocation;
    }

    public void SetBlackAndWhiteCameraLocation(GameObject blackAndWhiteCamera)
    {
        blackAndWhiteTexture = blackAndWhiteCamera;
    }

    /// <summary>
    /// Switches the camera
    /// </summary>
    /// <param name="newState">What to switch the camera to</param>
    private void SwitchCamera(ECameraState newState)
    {
        gameboyCamera.SetActive(false);
        originalCamera.SetActive(false);

        switch (newState)
        {
            case ECameraState.GameboyCamera:
                gameboyCamera.SetActive(true);
            break;
            case ECameraState.OriginalCamera:
                originalCamera.SetActive(true);
            break;
        }

        currentState = newState;
    }

    /// <summary>
    /// Toggles the camera
    /// </summary>
    private void ToggleCamera()
    {
        switch (currentState)
        {
            case ECameraState.GameboyCamera:
                SwitchCamera(ECameraState.OriginalCamera);
            break;
            case ECameraState.OriginalCamera:
                SwitchCamera(ECameraState.GameboyCamera);
            break;
        }
    }

    public void MoveLevel(int level)
    {
        currentLevel = level;
    }
}
