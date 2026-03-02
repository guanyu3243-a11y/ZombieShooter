using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject menuPanel;        // £®Start/Settings/Exit£©
    public GameObject settingsPanel;    // £®Slider + Back£©

    [Header("Settings Sliders (Optional)")]
    public Slider volumeSlider;         // Volume
    public Slider sensitivitySlider;    // For move speed

    [Header("Target Player (Optional)")]
    public PlayerController player;     // introduce player from main

    [Header("Scene Names")]
    public string gameSceneName = "Main";  //Loading "Main" scene

    
    private const string KEY_VOLUME = "SETTINGS_VOLUME";
    private const string KEY_SENSITIVITY = "SETTINGS_SENSITIVITY";

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
    }

    // ======= Panel switch =======

    public void ShowMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
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
    }

    // Back 
    public void OnClickBack()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
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

    // ======= ”¶”√…Ë÷√ =======

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
}