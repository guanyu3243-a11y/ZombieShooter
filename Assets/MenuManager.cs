using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    [Header("Audio Sliders")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    public AudioSource bgmSource;
    [Header("Panels")]
    public GameObject menuPanel;        // £¨Start/Settings/Exit£©
    public GameObject settingsPanel;    // £¨Slider + Back£©

    [Header("Settings Sliders (Optional)")]
    public Slider volumeSlider;         // Volume
    public Slider sensitivitySlider;    // For move speed

    [Header("Target Player (Optional)")]
    public PlayerController player;     // introduce player from main

    [Header("Scene Names")]
    public string gameSceneName = "Main";  //Loading "Main" scene

    [Header("Title UI")]
    public GameObject titleText;
    public GameObject subTitleText;


    private const string KEY_VOLUME = "SETTINGS_VOLUME";
    private const string KEY_SENSITIVITY = "SETTINGS_SENSITIVITY";
    private const string KEY_BGM = "SETTINGS_BGM";
    private const string KEY_SFX = "SETTINGS_SFX";
    // Default
    [Header("Default Values")]
    [Range(0f, 1f)] public float defaultVolume = 1f;
    public float defaultSensitivity = 6f;  

    void Start()
    {
        ShowMenu();

        float v = PlayerPrefs.GetFloat(KEY_VOLUME, defaultVolume);
        float s = PlayerPrefs.GetFloat(KEY_SENSITIVITY, defaultSensitivity);

        
        if (volumeSlider != null) volumeSlider.value = v;
        if (sensitivitySlider != null) sensitivitySlider.value = s;

        
        ApplyVolume(v);
        ApplySensitivity(s);

        
        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        if (sensitivitySlider != null)
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

        float bgm = PlayerPrefs.GetFloat(KEY_BGM, 1f);
        float sfx = PlayerPrefs.GetFloat(KEY_SFX, 1f);

        if (bgmSlider != null) bgmSlider.value = bgm;
        if (sfxSlider != null) sfxSlider.value = sfx;

        ApplyBGM(bgm);
        ApplySFX(sfx);

        bgmSlider.onValueChanged.AddListener(OnBGMChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    // ======= Panel switch =======

    public void ShowMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        SetTitleVisible(true);
    }

    public void ShowSettings()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
        SetTitleVisible(false);
    }
    void SetTitleVisible(bool visible)
{
    if (titleText != null) titleText.SetActive(visible);
    if (subTitleText != null) subTitleText.SetActive(visible);
}

    // ======= Button function =======

    // Start:Open game
    public void OnClickStart()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Settings
    public void OnClickSettings()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        SetTitleVisible(false);
    }

    // Back 
    public void OnClickBack()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
        SetTitleVisible(true);
    }

    // Exit
    public void OnClickExit()
    {
        Application.Quit();

        
    }

    // ======= Slider call-back =======

    public void OnVolumeChanged(float value)
    {
        ApplyVolume(value);
        PlayerPrefs.SetFloat(KEY_VOLUME, value);
        PlayerPrefs.Save();
    }

    
    public void OnSensitivityChanged(float value)
    {
        ApplySensitivity(value);
        PlayerPrefs.SetFloat(KEY_SENSITIVITY, value);
        PlayerPrefs.Save();
    }
    public void OnBGMChanged(float value)
    {
        ApplyBGM(value);
        PlayerPrefs.SetFloat(KEY_BGM, value);
        PlayerPrefs.Save();
    }

    public void OnSFXChanged(float value)
    {
        ApplySFX(value);
        PlayerPrefs.SetFloat(KEY_SFX, value);
        PlayerPrefs.Save();
    }

    // ======= Apply setting =======

    private void ApplyVolume(float value)
    {
        //Global volume
        AudioListener.volume = Mathf.Clamp01(value);
    }

    private void ApplySensitivity(float value)
    {
        
        if (player != null)
        {
            player.moveSpeed = value;
        }
    }
    private void ApplyBGM(float value)
    {
        if (bgmSource != null)
            bgmSource.volume = value;
    }

    private void ApplySFX(float value)
    {
     
    }
}