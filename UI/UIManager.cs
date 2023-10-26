using UnityEngine;
using System.Collections;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    string uiName = "";
    void Start()
    {
        uiName = this.name;
        //Debug.Log("name : " + uiName);
    }

    public void ChoiceMode()
    {
        //Debug.Log("start to choice");
        switch (uiName)
        {
            case "Full":
                //Debug.Log("press Full");
                ArmyManager.gameType = TestType.full;
                SceneManager.LoadScene("Test1");
                break;
            case "SomeEmpty":
                //Debug.Log("press SomeEmpty");
                ArmyManager.gameType = TestType.someEmpty;
                SceneManager.LoadScene("Test1");
                break;
            case "Reset":
                SceneManager.LoadScene("TestMain");
                break;
        }
    }
    public void ChoiceSpeed()
    {
        switch (uiName)
        {
            case "X1":
                ArmyManager.armySpeed = 0.1f;

                break;
            case "X2":
                ArmyManager.armySpeed = 0.2f;
                break;
            case "X3":
                ArmyManager.armySpeed = 0.3f;
                break;
        }
        Debug.Log(ArmyManager.armySpeed);
    }
}
