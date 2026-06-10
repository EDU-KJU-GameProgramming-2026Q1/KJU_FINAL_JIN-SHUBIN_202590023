using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // 全局单例，任意脚本可调用
    public static BGMManager Instance;

    [Header("Global Single BGM")]
    public AudioClip globalBGMClip;
    [Range(0f, 1f)] public float bgmVolume = 0.35f;

    private AudioSource bgmAudioSource;

    void Awake()
    {
        // 防止重复生成管理器
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // 跨场景保留，切换场景音乐不会断掉
        DontDestroyOnLoad(gameObject);

        // 自动挂载音频组件
        bgmAudioSource = GetComponent<AudioSource>();
        if (bgmAudioSource == null)
            bgmAudioSource = gameObject.AddComponent<AudioSource>();

        // 全局2D循环背景音乐设置
        bgmAudioSource.volume = bgmVolume;
        bgmAudioSource.loop = true;
        bgmAudioSource.playOnAwake = false;
        bgmAudioSource.spatialBlend = 0;
    }

    /// <summary>
    /// 启动全局BGM（全游戏只用这一首，只调用一次即可）
    /// </summary>
    public void PlayGlobalBGM()
    {
        if (globalBGMClip == null) return;
        // 如果已经在播放，不再重复播放，避免重叠杂音
        if (bgmAudioSource.isPlaying) return;

        bgmAudioSource.clip = globalBGMClip;
        bgmAudioSource.Play();
    }

    // 备用控制接口（可选，游戏结束/暂停界面使用）
    public void PauseBGM()
    {
        bgmAudioSource.Pause();
    }

    public void ResumeBGM()
    {
        bgmAudioSource.UnPause();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmAudioSource.volume = bgmVolume;
    }
}