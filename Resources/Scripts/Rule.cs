
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class Rule : UdonSharpBehaviour
{
    public Image image;
    public Sprite[] sprites;
    public AudioSource snd;

    private int curPage = 0;
    
    private const int MAX_PAGE = 6;

    public void OnNext() {
        if (curPage < MAX_PAGE) {
            image.sprite = sprites[++curPage];
            snd.Stop();
            snd.Play();
        }
    }

    public void OnPrev() {
        if (curPage > 0) {
            image.sprite = sprites[--curPage];
            snd.Stop();
            snd.Play();
        }
    }
}
