using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    private Dictionary<string, AudioClip> soundDic;

    private AudioSource seAudio;
    private AudioSource bgmAudio0;          //주플레이 AudioSource
    private AudioSource bgmAudio1;          //빠지는 플레이 AudioSource

    public float masterVolume = 1f;         //마스터 볼륨
    public float crossFadeTime = 5f;        //fade 시간

    private float seVolume = 0f;            //사운드 볼륨
    private float bgmVolume0 = 0f;          //AudioSource0 번볼륨
    private float bgmVolume1 = 0f;          //AudioSource1 번볼륨

    private bool fadeAudio;

    //Sound Data
    [SerializeField]
    private string tablePath = "DataTable/SoundTable";
    private SoundTable soundTable;

    private void Awake()
    {
        soundDic = new Dictionary<string, AudioClip>();
        soundTable = Resources.Load(tablePath) as SoundTable;

        seAudio = this.gameObject.AddComponent<AudioSource>();
        bgmAudio0 = this.gameObject.AddComponent<AudioSource>();
        bgmAudio1 = this.gameObject.AddComponent<AudioSource>();

        bgmAudio0.loop = true;
        bgmAudio1.loop = true;

        seAudio.volume = 0f;
        bgmAudio0.volume = 0f;
        bgmAudio1.volume = 0f;

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        //Sound가 플레이 중이라면..
        if (fadeAudio)
        {
            //주플레이 AudioSource 는 볼륨을 올린다.
            if (bgmVolume0 < 1f)
            {
                bgmVolume0 += Time.deltaTime / crossFadeTime;
                if (bgmVolume0 >= 1f)
                    bgmVolume0 = 1f;
            }

            //빠진는 AudioSource 는 볼륨을 내린다.
            if (bgmVolume1 > 0f)
            {
                bgmVolume1 -= Time.deltaTime / crossFadeTime;
                if (bgmVolume1 <= 0f)
                {
                    bgmVolume1 = 0f;
                    bgmAudio1.Stop();
                    fadeAudio = false;
                }
            }
        }

        seAudio.volume = seVolume * masterVolume;
        bgmAudio0.volume = bgmVolume0 * masterVolume;
        bgmAudio1.volume = bgmVolume1 * masterVolume;
    }

    private void OnDestroy()
    {
        UnloadResourcesAll();
    }

    ///<summary>
    ///Prepare Sound Asset
    ///</summary>
    public void PrepareAssets(string _tag)
    {
        AudioClip soundClip = GetAudioClip(_tag);
        if (soundClip == null)
        {
            Debug.LogError("Prepare Faild. Sound data : " + _tag);
            return;
        }

        Debug.Log("Prepare Success. Sound data : " + _tag);
    }

    ///<summary>
    ///2D Sound OneShot
    ///</summary>
    public void PlaySound2D(string _tag, float _volume = 1f, float _volumeScale = 1f)
    {
        AudioClip soundClip = GetAudioClip(_tag);
        if (soundClip == null)
        {
            Debug.LogError("Not found Sound data : " + _tag);
            return;
        }

        seVolume = _volume;
        seAudio.PlayOneShot(soundClip, _volumeScale);
    }

    ///<summary>
    ///2D Sound Loop
    ///</summary>
    public void PlaySound2D(string _tag, GameObject audioObject, float _volume = 1f, bool _loop = false)
    {
        if (audioObject == null)
        {
            Debug.LogError("Sound target is null : " + _tag);
            return;
        }

        AudioClip soundClip = GetAudioClip(_tag);
        if (soundClip == null)
        {
            Debug.LogError("Not found Sound data : " + _tag);
            return;
        }

        AudioSource audio = audioObject.GetComponent<AudioSource>();
        if (audio == null)
        {
            audio = audioObject.AddComponent<AudioSource>();
        }

        audio.volume = _volume;
        audio.loop = _loop;
        audio.clip = soundClip;
        audio.Play();
    }

    ///<summary>
    ///3D Sound OneShot
    ///</summary>
    public void PlaySound3D(string _tag, GameObject audioObject, float _volume = 1f, float _spatialBlend = 1f)
    {
        if (audioObject == null)
        {
            Debug.LogError("Sound target is null : " + _tag);
            return;
        }

        AudioClip soundClip = GetAudioClip(_tag);
        if (soundClip == null)
        {
            Debug.LogError("Not found Sound data : " + _tag);
            return;
        }

        AudioSource spatialAudio = audioObject.GetComponent<AudioSource>();
        if (spatialAudio == null)
        {
            spatialAudio = audioObject.AddComponent<AudioSource>();
        }

        spatialAudio.volume = _volume;
        spatialAudio.spatialBlend = _spatialBlend;
        spatialAudio.PlayOneShot(soundClip);
    }

    ///<summary>
    ///3D Sound Loop
    ///</summary>
    public void PlaySound3D(string _tag, GameObject audioObject, float _volume = 1f,
        float _spatialBlend = 1f, bool _loop = false)
    {
        if (audioObject == null)
        {
            Debug.LogError("Sound target is null : " + _tag);
            return;
        }

        AudioClip soundClip = GetAudioClip(_tag);
        if (soundClip == null)
        {
            Debug.LogError("Not found Sound data : " + _tag);
            return;
        }

        AudioSource spatialAudio = audioObject.GetComponent<AudioSource>();
        if (spatialAudio == null)
        {
            spatialAudio = audioObject.AddComponent<AudioSource>();
        }

        spatialAudio.volume = _volume;
        spatialAudio.spatialBlend = _spatialBlend;
        spatialAudio.loop = _loop;
        spatialAudio.clip = soundClip;
        spatialAudio.Play();
    }

    ///<summary>
    ///BGM
    ///</summary>
    public void PlayMusic(string _tag, bool _fade = false)
    {
        AudioClip soundClip = GetAudioClip(_tag);
        if (soundClip == null)
        {
            Debug.LogError("Not found Sound data : " + _tag);
            return;
        }

        if (!_fade)
        {
            bgmAudio0.clip = soundClip;
            bgmAudio0.Play();
        }
        else
        {
            //기존에 플레이되는 것을 1 번으로
            AudioSource temp = bgmAudio0;
            bgmAudio0 = bgmAudio1;
            bgmAudio1 = temp;

            //볼륨값스왑
            float tempVolume = bgmVolume0;
            bgmVolume0 = bgmVolume1;
            bgmVolume1 = tempVolume;

            //클립에 새로운 오디오 클립 물린다
            bgmAudio0.clip = soundClip;
            bgmAudio0.Play();
            fadeAudio = true;
        }
    }

    ///<summary>
    ///Stop BGM
    ///</summary>
    public void StopAudio(bool _fade = false)
    {
        if (_fade)
        {
            //기존에 플레이되는 것을 1 번으로
            AudioSource temp = bgmAudio0;
            bgmAudio0 = bgmAudio1;
            bgmAudio1 = temp;

            //볼륨값스왑
            float tempVolume = bgmVolume0;
            bgmVolume0 = bgmVolume1;
            bgmVolume1 = tempVolume;

            bgmAudio0.Stop();
            fadeAudio = true;
        }
        else
        {
            if (bgmAudio0.isPlaying)
                bgmAudio0.Stop();
            if (bgmAudio0.clip != null)
                bgmAudio0.clip = null;

            if (bgmAudio1.isPlaying)
                bgmAudio1.Stop();
            if (bgmAudio1.clip != null)
                bgmAudio1.clip = null;
        }
    }

    ///<summary>
    ///Stop Target Audio
    ///</summary>
    public void StopAudio(GameObject _targetObject)
    {
        if (_targetObject == null)
        {
            Debug.LogError("Sound TargetObject is null");
            return;
        }

        AudioSource audio = _targetObject.GetComponent<AudioSource>();
        audio.Stop();
    }

    AudioClip GetAudioClip(string _tag)
    {
        if (!soundDic.ContainsKey(_tag))
        {
            //Resources 경로 설정
            SoundTable.SoundInfo soundInfo = soundTable.GetSoundInfo(_tag);
            if (soundInfo == null)
            {
                Debug.LogError("Not found SoundInfo : " + _tag);
                return null;
            }

            string resourcePath = string.Format("{0}{1}", soundInfo.path, soundInfo.tag);

            //Resources 에서 오디오클립 로드
            AudioClip newClip = Resources.Load(resourcePath) as AudioClip;

            if (newClip == null)
            {
                Debug.LogError("Not found Audioclip : " + _tag);
                return null;
            }

            //Table 에 추가
            soundDic.Add(_tag, newClip);
        }

        return soundDic[_tag];
    }

    ///<summary>
    ///Unload resource with tag
    ///</summary>
    public void UnloadResource(string _tag)
    {
        if (soundDic.ContainsKey(_tag))
            soundDic.Remove(_tag);

        Resources.UnloadUnusedAssets();
    }

    ///<summary>
    ///Unload resource all
    ///</summary>
    public void UnloadResourcesAll()
    {
        soundDic.Clear();
        Resources.UnloadUnusedAssets();
    }
}
