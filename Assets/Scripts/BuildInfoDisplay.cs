using UnityEngine;
using UnityEngine.UI;

public class BuildInfoDisplay : MonoBehaviour
{
    [SerializeField]
    private Text buildInfoText;

    void Start()
    {
        LoadAndDisplayBuildInfo();
    }

    private void LoadAndDisplayBuildInfo()
    {
        // Resources 폴더에서 BuildInfo.txt 파일 읽기
        TextAsset buildInfoAsset = Resources.Load<TextAsset>("BuildInfo");
        
        if (buildInfoAsset != null)
        {
            string buildInfo = buildInfoAsset.text;
            
            if (buildInfoText != null)
            {
                buildInfoText.text = buildInfo;
                Debug.Log($"Build info displayed: {buildInfo}");
            }
            else
            {
                Debug.LogError("BuildInfoText is not assigned!");
            }
        }
        else
        {
            string errorMessage = "BuildInfo.txt not found in Resources folder!";
            if (buildInfoText != null)
            {
                buildInfoText.text = errorMessage;
            }
            Debug.LogError(errorMessage);
        }
    }
}
