
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UIButton : UdonSharpBehaviour
{
    public Option option;
    public GameObject QuestionUI;

    public void OpenUI() {
        option.playSound();
        QuestionUI.SetActive(true);
    }
    public void CloseUI() {
        option.playSound();
        QuestionUI.SetActive(false);
    }
}
