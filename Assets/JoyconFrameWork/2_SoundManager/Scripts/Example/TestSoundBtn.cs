using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlaySoundType { BGM, SE }

public class TestSoundBtn : MonoBehaviour {

    public PlaySoundType soundType;
    public string soundName;

    public GameObject soundObject;

    private Button button;

	// Use this for initialization
	void Start () {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(PressedBtn);

        if(!string.IsNullOrEmpty(soundName))
            SoundManager.Instance.PrepareAssets(soundName);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(PressedBtn);
    }

    void PressedBtn()
    {
        switch(soundType)
        {
            case PlaySoundType.BGM:
                if (!soundName.Equals(""))
                    SoundManager.Instance.PlayMusic(soundName, true);
                else
                    SoundManager.Instance.StopAudio(true);
                break;
            case PlaySoundType.SE:
                if(soundObject)
                {
                    SoundManager.Instance.PlaySound3D(soundName, soundObject, 1f, 1f);
                }
                else
                {
                    SoundManager.Instance.PlaySound2D(soundName);
                }
                break;
        }

        Debug.Log("Play Sound : " + soundName);
    }
}
