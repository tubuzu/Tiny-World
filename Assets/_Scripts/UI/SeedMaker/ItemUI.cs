using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public BaseItemProfileSO itemProfile;
    public Image image;
    public Sprite lockSprite;
    public Button button;


    public void SetupProfile(BaseItemProfileSO itemProfile)
    {
        this.itemProfile = itemProfile;
        if (!itemProfile.unlocked) this.image.sprite = lockSprite;
        else this.image.sprite = itemProfile.sprite;
    }
}
