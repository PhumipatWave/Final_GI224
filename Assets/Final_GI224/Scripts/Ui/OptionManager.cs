using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionManager : MonoBehaviour
{
    [Header("หน้าจอ")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("เสียง")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("ส่วนอื่นๆ")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private AudioSource sfxSource; 

    private Resolution[] resolutionOptions = new Resolution[]
    {
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1366, height = 768 },
        new Resolution { width = 1600, height = 900 },
        new Resolution { width = 1920, height = 1080 }
    };

    private void Start()
    {
        // Starter Setting
        InitializeUI();
        LoadSettings();

        // add Event Listener to bottun
        saveButton.onClick.AddListener(SaveSettings);
        closeButton.onClick.AddListener(CloseSettingsPanel);

        // add Event Listener for UI need immediate update
        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolutionOnly(); });
        fullscreenToggle.onValueChanged.AddListener(delegate { SetFullscreenOnly(); });
    }

    private void InitializeUI()
    {
        // Setting Dropdown resolution
        resolutionDropdown.ClearOptions();
        System.Collections.Generic.List<string> options = new System.Collections.Generic.List<string>();

        foreach (Resolution res in resolutionOptions)
        {
            options.Add(res.width + " x " + res.height);
        }

        resolutionDropdown.AddOptions(options);

        // Setting Toggle Fullscreen
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    // setting resolution
    public void SetResolutionOnly()
    {
        int index = resolutionDropdown.value;
        
        Screen.SetResolution(
            resolutionOptions[index].width,
            resolutionOptions[index].height,
            Screen.fullScreen
        );
        
        Debug.Log($"changes {resolutionOptions[index].width} x {resolutionOptions[index].height}");
    }

    // setting fullscreen
    public void SetFullscreenOnly()
    {
        bool isFullscreen = fullscreenToggle.isOn;
        
        Screen.fullScreen = isFullscreen;
        
        Debug.Log($"Setting full screen {(isFullscreen ? "On" : "Off")}");
    }

    public void SetMasterVolume()
    {
        AudioListener.volume = masterVolumeSlider.value;
    }

    public void SetSFXVolume()
    {
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolumeSlider.value;
        }
    }

    public void LoadSettings()
    {
        // load setting resolution
        int resIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        resolutionDropdown.value = resIndex;

        // load full Screen setting
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;

        // load setting volume
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        masterVolumeSlider.value = masterVolume;
        sfxVolumeSlider.value = sfxVolume;

        SetMasterVolume();
        SetSFXVolume();

        SetResolutionOnly();
        SetFullscreenOnly();
    }

    public void SaveSettings()
    {
        // Save resolution
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);

        // Save volume
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);

        // Save in PlayerPrefs
        PlayerPrefs.Save();

        Debug.Log("บันทึกการตั้งค่าเรียบร้อย");
    }

    public void CloseSettingsPanel()
    {
        SaveSettings();

        gameObject.SetActive(false);

        Time.timeScale = 1.0f;
    }

    // Play SFX when change volume
    public void PlayTestSound()
    {
        if (sfxSource != null && sfxSource.clip != null)
        {
            sfxSource.PlayOneShot(sfxSource.clip);
        }
    }
}
