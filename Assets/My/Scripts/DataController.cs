using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data<T>
{
    readonly string key;
    readonly System.Func<string, T> getFunc;
    readonly System.Action<string, T> setFunc;

    public Data(string key)
    {
        this.key = key;

        if (typeof(T) == typeof(int))
        {
            getFunc = k => (T)(object)PlayerPrefs.GetInt(k);
            setFunc = (k, v) => PlayerPrefs.SetInt(k, (int)(object)v);
        }
        else if (typeof(T) == typeof(float))
        {
            getFunc = k => (T)(object)PlayerPrefs.GetFloat(k);
            setFunc = (k, v) => PlayerPrefs.SetFloat(k, (float)(object)v);
        }
        else if (typeof(T) == typeof(string))
        {
            getFunc = k => (T)(object)PlayerPrefs.GetString(k);
            setFunc = (k, v) => PlayerPrefs.SetString(k, (string)(object)v);
        }
    }

    /// <summary>
    /// 정보를 가져옴
    /// </summary>
    /// <returns></returns>
    public T Get() => getFunc(key);

    /// <summary>
    /// 값을 저장함
    /// </summary>
    /// <param name="value">저장할 값</param>
    public void Save(T value) => setFunc(key, value);

    /// <summary>
    /// 데이터가 있는지 조회
    /// </summary>
    /// <returns></returns>
    public bool HasKey() => PlayerPrefs.HasKey(key); 

    /// <summary>
    /// 데이터 삭제
    /// </summary>
    public void Delete() => PlayerPrefs.DeleteKey(key); 
}

public class DataController
{
    public Data<int> coin = new Data<int>("coin");
    public Data<int> highScore = new Data<int>("highScore");

    public Data<int> hpLevel = new Data<int>("hpLevel");
    public Data<int> damageLevel = new Data<int>("damageLevel");
    public Data<int> criticalChanceLevel = new Data<int>("criticalChanceLevel");
    public Data<int> criticalDamageLevel = new Data<int>("criticalDamageLevel");

    // 해상도
    public Data<int> resolutionNum = new Data<int>("resolutionNum");
    // 볼륨
    public Data<float> bgmVolume = new Data<float>("bgmVolume");
    public Data<float> sfxVolume = new Data<float>("sfxVolume");
}
