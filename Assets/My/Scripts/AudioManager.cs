using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("# BGM")]
    [SerializeField] AudioClip[] bgmClips;
    AudioSource bgmPlayer;
    // 너무 커서 조금 보정
    float editedBgmVolume = 0.8f;

    [Header("# SFX")]
    [SerializeField] AudioClip[] sfxClips;
    int channels = 10;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum BGM {
        MainTitle,
        Bakery,
        GetChuCrayon,
        Battle
    }
    public enum Sfx {
        /// <summary>
        /// UI 등에 사용해보기
        /// </summary>
        ButtonClick,
        Count,
        CountFinal,
        /// <summary>
        /// 환호 와-아-
        /// </summary>
        BattleStartExclamation,
        /// <summary>
        /// 사도 선택하고 게임 시작
        /// </summary>
        BattleEndExclamation,
        GameStart,
        buy
    }


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Init();

        PlayBGM(BGM.MainTitle);
    }

    public void SetVolume(float bgmVolume, float sfxVolume)
    {
        bgmPlayer.volume = bgmVolume * editedBgmVolume;
        for (int index=0; index < sfxPlayers.Length; index++) {
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = true;
        bgmPlayer.loop = true;
        bgmPlayer.volume = GameManager.instance.data.bgmVolume.Get() * editedBgmVolume;
        // bgmPlayer.clip = bgmClip;
        // bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index=0; index < sfxPlayers.Length; index++) {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            // sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = GameManager.instance.data.sfxVolume.Get();
        }
    }

    public void PlayBGM(BGM bGM)
    {
        int index = (int)bGM;

        bgmPlayer.Stop();
        if (bGM == BGM.Battle) {
            // 배틀 음 3개 index, index+1, index+2
            index += Random.Range(0, 2+1);
        }
        bgmPlayer.clip = bgmClips[index];
        bgmPlayer.Play();
    }
    public void StopBGM() => bgmPlayer.Stop();

    public void PlaySfx(Sfx sfx)
    {
        for (int index=0; index < sfxPlayers.Length; index++) {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if ( sfxPlayers[loopIndex].isPlaying)
                continue;
            
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
    public void StopAllSfx()
    {
        for (int index=0; index < sfxPlayers.Length; index++) {
            sfxPlayers[index].Stop();
        }
    }
}
