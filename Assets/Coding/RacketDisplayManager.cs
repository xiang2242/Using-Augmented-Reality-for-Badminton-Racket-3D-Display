using UnityEngine;

public class RacketDisplayManager : MonoBehaviour
{
    public GameObject YY88DModel;
    public GameObject YY1000ZZModel;
    public GameObject AX100Model;

    void Start()
    {

        // 获取用户选择的球拍名称
        string selectedRacket = PlayerPrefs.GetString("SelectedRacket");
        Debug.Log("SelectedRacket = " + selectedRacket);


        // 先隐藏所有模型
        YY88DModel.SetActive(false);
        YY1000ZZModel.SetActive(false);
        AX100Model.SetActive(false);


        // 根据选择显示对应模型
        switch (selectedRacket)
        {
            case "YY88DModel":
            Debug.Log("RacketDisplayManager loaded!");
                YY88DModel.SetActive(true);
                break;
            case "YY1000ZZModel":
                YY1000ZZModel.SetActive(true);
                break;
            case "AX100Model":
                AX100Model.SetActive(true);
                break;
            default:
                Debug.LogWarning("There is no matching racket model!");
                break;
        }
    }
}
