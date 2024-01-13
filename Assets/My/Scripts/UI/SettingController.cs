using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingController : MonoBehaviour
{
    [Header("해상도")]
    FullScreenMode screenMode;
    [SerializeField] Toggle fullScreenToggle;
    List<Resolution> resolutionsList = new List<Resolution>();
    [SerializeField] TMP_Dropdown resolutionDropdown;
    int resolutionNum;
    [Header("볼륨")]
    [SerializeField] Slider bgmSlider;
    [SerializeField] TextMeshProUGUI bgmText;
    [SerializeField] Slider sfxSlider;
    [SerializeField] TextMeshProUGUI sfxText;

    void Start()
    {
        // 해상도 레이트가 60인 것만 집어넣어서 초기화
        resolutionDropdown.options.Clear();
        for (int i=0; i < Screen.resolutions.Length; i++) {
            Resolution item = Screen.resolutions[i];

            if (item.refreshRate == 60) {
                resolutionsList.Add(item);

                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
                resolutionDropdown.options.Add(option);
            }
        }

        if (GameManager.instance.data.resolutionNum.HasKey()) {
            // 저장이 되어 있다면 그대로 사용
            resolutionNum = GameManager.instance.data.resolutionNum.Get();
            resolutionDropdown.value = resolutionNum;
        } else {
            resolutionDropdown.value = resolutionDropdown.options.Count - 1;
        }
        resolutionDropdown.RefreshShownValue();

        // 볼륨 관련
        bgmSlider.value = GameManager.instance.data.bgmVolume.Get() * 100;
        BgmVolumeChanged();
        sfxSlider.value = GameManager.instance.data.sfxVolume.Get() * 100;
        SfxVolumeChanged();

        // 풀 스크린 여부
        fullScreenToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow)? true : false;
    }

    /// <summary>
    /// 해상도 토글 작동시 실행
    /// </summary>
    /// <param name="isFull"></param>
    public void FullScreenButton(bool isFull)
    {
        screenMode = isFull? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        ResolutionButton();
    }

    /// <summary>
    /// 해상도 드랍박스 선택이 되면 실행
    /// </summary>
    /// <param name="x"></param>
    public void DropBoxOptionChange(int x)
    {
        resolutionNum = x;
        GameManager.instance.data.resolutionNum.Save(x);
        ResolutionButton();
    }

    void ResolutionButton()
    {
        Screen.SetResolution(
            resolutionsList[resolutionNum].width,
            resolutionsList[resolutionNum].height,
            screenMode
        );
    }

    public void SaveVolume()
    {
        GameManager.instance.data.bgmVolume.Save(bgmSlider.value / 100);
        GameManager.instance.data.sfxVolume.Save(sfxSlider.value / 100);
        AudioManager.instance.SetVolume(bgmSlider.value / 100, sfxSlider.value / 100);
    }

    public void BgmVolumeChanged()
    {
        bgmText.text = (bgmSlider.value / 100).ToString("#%");
    }

    public void SfxVolumeChanged()
    {
        sfxText.text = (sfxSlider.value / 100).ToString("#%");
    }
}
