
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class NightMode : UdonSharpBehaviour
{
    public PlayerManager manager;
    public SkinnedMeshRenderer meshRenderer;
    public Invisible invisible;
    public CountTimer countTimer;

    private bool _timer_fade;
    private bool _timer_tick;
    private bool _fadeInOrOut;
    private float _delta;
    private float _fade;
    private float _tick;
    private float _alpha;

    private const float TIME_FADE = 1f;
    private const float TIME_OUT = 30f;

    private void Update() {
        if (_timer_fade) {
            _delta += Time.deltaTime;
            _fade += Time.deltaTime;
            if (_delta >= 0.02f) {
                _delta -= 0.02f;
                if (_fadeInOrOut)
                    FadeIn();
                else
                    FadeOut();

                if (_fade > TIME_FADE) {
                    _delta = 0f;
                    _fade = 0f;
                    _timer_fade = false;
                }
            }
        }
        if (_timer_tick) {
            _tick += Time.deltaTime;
            if (_tick > TIME_OUT) {
                DisableNight();
                _timer_tick = false;
                _tick = 0f;
            }
        }
    }
    public void EnableNightWitch() {
        foreach (Player p in manager.players) {
            if (Networking.GetOwner(p.gameObject) == Networking.LocalPlayer && p.IsJoined) {
                if (!p.isWitch) {
                    Debug.Log("마녀가 아니므로 눈을 가림");
                    invisible.gameObject.SetActive(true);
                    _timer_fade = true;
                    _timer_tick = true;
                    _fadeInOrOut = true;
                } else {
                    Debug.Log("마녀이므로 눈을 가리지 않음");
                    countTimer.SetActive(true);
                    countTimer.StartTimer((int)TIME_OUT);
                }
            }
        }
    }
    public void EnableNightConstable() {
        foreach (Player p in manager.players) {
            if (Networking.GetOwner(p.gameObject) == Networking.LocalPlayer && p.IsJoined) {
                if (!p.isConstable) {
                    invisible.gameObject.SetActive(true);
                    _timer_fade = true;
                    _timer_tick = true;
                    _fadeInOrOut = true;
                } else {
                    countTimer.SetActive(true);
                    countTimer.StartTimer((int)TIME_OUT);
                }
            }
        }
    }

    public void DisableNight() {
        invisible.gameObject.SetActive(false);
        _timer_fade = true;
        _fadeInOrOut = false;
    }
    private void FadeIn() {
        float time = _fade / TIME_FADE;
        _alpha = Mathf.Lerp(0, 1, time);
        meshRenderer.material.SetFloat("_Alpha", _alpha);
    }
    private void FadeOut() {
        float time = _fade / TIME_FADE;
        _alpha = Mathf.Lerp(1, 0, time);
        meshRenderer.material.SetFloat("_Alpha", _alpha);
    }

    private void Start() {
        _alpha = meshRenderer.material.GetFloat("_Alpha");
    }
}
