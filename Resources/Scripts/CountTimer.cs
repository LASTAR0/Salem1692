
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class CountTimer : UdonSharpBehaviour
{
    public Image circle;
    public Image NumberTen;
    public Image NumberOne;
    [Space(5)]
    public Sprite[] number;

    private int _curTime;
    private int _defaultTime;
    private bool _timer = false;
    private float _delta = 0f;
    private float _fill = 0f;

    public void SetActive(bool bl) {
        circle.gameObject.SetActive(bl);
        NumberTen.gameObject.SetActive(bl);
        NumberOne.gameObject.SetActive(bl);
    }
    public void StartTimer(int time) {
        _curTime = 30;
        _defaultTime = _curTime;
        _timer = true;
        SetNumber(_curTime);
    }
    public void SetNumber(int num) {
        int ten = num / 10;
        int one = num % 10;
        NumberTen.sprite = number[ten];
        NumberOne.sprite = number[one];
    }

    private void look() {
        Quaternion rot = Networking.LocalPlayer.GetRotation();
        transform.rotation = Quaternion.AngleAxis(rot.eulerAngles.y, Vector3.up);
    }
    private void fillDown() {
        float time = _fill / _defaultTime;
        circle.fillAmount = Mathf.Lerp(1, 0, time);
    }
    private void Update() {
        if (_timer) {
            _delta += Time.deltaTime;
            _fill += Time.deltaTime;
            fillDown();
            look();
            if (_delta >= 1.0f) {
                _delta -= 1.0f;
                _curTime -= 1;
                SetNumber(_curTime);
                if (_curTime <= 0) {
                    _timer = false;
                    _delta = 0f;
                    _fill = 0f;
                    _curTime = 0;
                    SetActive(false);
                }
            }
        }
    }
}
