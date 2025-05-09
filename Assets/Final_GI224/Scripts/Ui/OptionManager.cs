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
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("ส่วนอื่นๆ")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private AudioSource musicSource; // สำหรับเสียงเพลงประกอบ
    [SerializeField] private AudioSource sfxSource;   // สำหรับเสียงเอฟเฟค

    // ตัวเลือกความละเอียดหน้าจอแบบง่าย (ไม่ต้องดึงมาจากระบบ)
    private Resolution[] resolutionOptions = new Resolution[]
    {
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1366, height = 768 },
        new Resolution { width = 1600, height = 900 },
        new Resolution { width = 1920, height = 1080 }
    };

    private void Start()
    {
        // ตั้งค่าเริ่มต้น
        InitializeUI();
        LoadSettings();

        // เพิ่ม Event Listener สำหรับปุ่มต่างๆ
        saveButton.onClick.AddListener(SaveSettings);
        closeButton.onClick.AddListener(CloseSettingsPanel);
    }

    private void InitializeUI()
    {
        // ตั้งค่า Dropdown ความละเอียดหน้าจอ
        resolutionDropdown.ClearOptions();
        System.Collections.Generic.List<string> options = new System.Collections.Generic.List<string>();

        foreach (Resolution res in resolutionOptions)
        {
            options.Add(res.width + " x " + res.height);
        }

        resolutionDropdown.AddOptions(options);

        // ตั้งค่า Toggle Fullscreen
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void SetResolution()
    {
        // เลือกความละเอียดหน้าจอจาก dropdown
        int index = resolutionDropdown.value;
        bool isFullscreen = fullscreenToggle.isOn;

        // ตั้งค่าความละเอียดหน้าจอ
        Screen.SetResolution(
            resolutionOptions[index].width,
            resolutionOptions[index].height,
            isFullscreen
        );
    }

    public void SetMasterVolume()
    {
        // ตั้งค่าระดับเสียงรวม
        AudioListener.volume = masterVolumeSlider.value;
    }

    public void SetMusicVolume()
    {
        // ตั้งค่าระดับเสียงเพลงประกอบ
        if (musicSource != null)
        {
            musicSource.volume = musicVolumeSlider.value;
        }
    }

    public void SetSFXVolume()
    {
        // ตั้งค่าระดับเสียงเอฟเฟค
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolumeSlider.value;
        }
    }

    public void LoadSettings()
    {
        // โหลดการตั้งค่าความละเอียดหน้าจอ
        int resIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        resolutionDropdown.value = resIndex;

        // โหลดการตั้งค่า Screen
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;

        // โหลดการตั้งค่าระดับเสียง
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;

        // ตั้งค่าระดับเสียงเริ่มต้น
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();

        // ตั้งค่าความละเอียดหน้าจอ
        SetResolution();
    }

    public void SaveSettings()
    {
        // บันทึกการตั้งค่าความละเอียดหน้าจอ
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);

        // บันทึกการตั้งค่าระดับเสียง
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);

        // บันทึกลงใน PlayerPrefs
        PlayerPrefs.Save();

        Debug.Log("บันทึกการตั้งค่าเรียบร้อย");
    }

    public void CloseSettingsPanel()
    {
        // บันทึกการตั้งค่าก่อนปิด
        SaveSettings();

        // ปิด Panel การตั้งค่า
        gameObject.SetActive(false);
    }

    // ฟังก์ชันสำหรับเล่นเสียงตัวอย่างเมื่อปรับ slider
    public void PlayTestSound()
    {
        if (sfxSource != null && sfxSource.clip != null)
        {
            sfxSource.PlayOneShot(sfxSource.clip);
        }
    }
}
