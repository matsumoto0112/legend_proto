using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> seList;
    public static List<AudioClip> soundList_SE;
    [SerializeField]
    private List<AudioClip> bgmList;
    public static List<AudioClip> soundList_BGM;
    [SerializeField]
    private List<AudioClip> voiceList;
    public static List<AudioClip> soundList_VOICE;

    public static AudioSource se;
    public static AudioSource bgm;
    public static AudioSource voice;



    // Start is called before the first frame update
    void Start()
    {
        //if (GameObject.Find("SoundManager") != null && GameObject.Find("SoundManager") != gameObject)
        //{
        //    Destroy(gameObject);
        //}
        BGM_SE_Load();
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}
    }

    //SEを再生
    public static void PlaySE(int seNumber)
    {
        se.PlayOneShot(soundList_SE[seNumber]);
    }

    //BGMを再生
    public static void PlayBGM(int bgmNumber, float volume)
    {
        if (!CheckBGM(bgmNumber))
        {
            bgm.Stop();
            bgm.volume = volume;
            bgm.clip = soundList_BGM[bgmNumber];
            bgm.Play();
        }
    }

    //VOICEを再生
    public static void PlayVOICE(int voiceNumber)
    {
        voice.PlayOneShot(soundList_VOICE[voiceNumber]);
    }

    //BGMを止める
    public static void StopBGM()
    {
        bgm.Stop();
    }

    //現在のBGMが指定したBGMと一緒か否か
    public static bool CheckBGM(int bgmNumber)
    {
        if (bgm.clip == null)
        {
            return false;
        }
        if (bgm.clip == soundList_BGM[bgmNumber])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //VOICEが再生中か否か
    public static bool CheckPlayVOICE()
    {
        return voice.isPlaying;
    }

    //BGMをフェードアウトさせる
    public static void FadeOut()
    {
        bgm.volume -= 0.005f;
    }

    //再生中のBGMのボリュームを確認
    public static float BGMVolumeCheck()
    {
        return bgm.volume;
    }

    //再生中のBGMのボリュームを変更
    public static void SetBGMVolume(float volume)
    {
        bgm.volume = volume;
    }

    //現在のBGMを取得
    public static AudioSource GetBGM()
    {
        return bgm;
    }

    //BGMとSEをロードする
    public void BGM_SE_Load()
    {
        soundList_SE = seList;
        soundList_BGM = bgmList;
        soundList_VOICE = voiceList;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        se = audioSources[0];
        bgm = audioSources[1];
        voice = audioSources[2];
    }
}
