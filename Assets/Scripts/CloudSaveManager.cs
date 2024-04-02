using UnityEngine;

public class CloudSaveManager : MonoBehaviour
{
    [SerializeField] private CloudSaveManagerInitType initType;

    private void Start()
    {
        switch (initType)
        {
            case CloudSaveManagerInitType.InitScene:
                break;
                
            
            case CloudSaveManagerInitType.GameScene:
                break;
        }
    }
}

public enum CloudSaveManagerInitType
{
    InitScene,
    GameScene
}