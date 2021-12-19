using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourSingleton<GameController>
{
    public InScene Scene;

    public AudioManager AudioManager { get; private set; }

    private int level;

    // Start is called before the first frame update
    override protected void SingletonStarted()
    {
        SpawnScene();

        AudioManager = gameObject.GetComponent<AudioManager>();

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnScene()
    {
        var scene = new GameObject("Scene");
        Scene = scene.AddComponent<InScene>();

        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        List<GameObject> camerasList = new List<GameObject>();
        camerasList.AddRange(cameras);

        Scene.SetGameboyCamera(camerasList.Find(c => c.name.Contains("MainCamera")));
        //Scene.SetOriginalCamera(camerasList.Find(c => c.name.Contains("Render Camera")));
        //Scene.SwitchCamera(ECameraState.OriginalCamera);

        GameObject[] blackWhiteTexture = GameObject.FindGameObjectsWithTag("BlackWhiteTexture");
        List<GameObject> blackWhiteTextureList = new List<GameObject>();
        blackWhiteTextureList.AddRange(blackWhiteTexture);

        Scene.SetBlackAndWhiteTextureObject(blackWhiteTextureList.Find(c => c.name.Contains("BlackAndWhiteTexture")));
        Scene.SetBlackAndWhiteCameraLocation(blackWhiteTextureList.Find(c => c.name.Contains("Game Camera")));
    }
}
