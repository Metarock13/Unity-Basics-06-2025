using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseRoot;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private AudioMixer masterMixer;

    public static bool IsPaused { get; private set; }

    const string KEY_VOL  = "volume";
    const string KEY_SENS = "sensitivity";
    private const string MIX_PARAM = "MasterVolume";
    void Start()
    {
        float vol = PlayerPrefs.GetFloat(KEY_VOL, 0.8f);
        volumeSlider.SetValueWithoutNotify(vol);
        ApplyVolume(vol);

        float sens = PlayerPrefs.GetFloat(KEY_SENS, 1.0f);
        sensitivitySlider.SetValueWithoutNotify(sens);

        if (pauseRoot) pauseRoot.SetActive(false);
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (IsPaused) Resume(); else Pause();
    }

    public void OnVolumeChanged(float v)
    {
        ApplyVolume(v);
        PlayerPrefs.SetFloat(KEY_VOL, Mathf.Clamp01(v));
        PlayerPrefs.Save();
    }

    void ApplyVolume(float v01)
    {
        float dB = v01 <= 0.0001f ? -80f : Mathf.Log10(v01) * 20f;
        masterMixer.SetFloat(MIX_PARAM, dB);
    }

    public void OnApplyButton()
    {
        PlayerPrefs.SetFloat(KEY_SENS, Mathf.Clamp(sensitivitySlider.value, 0.1f, 10f));
        PlayerPrefs.Save();
    }

    public void OnDefaultsButton()
    {
        volumeSlider.value = 0.8f;
        sensitivitySlider.value = 1.0f;
    }

    private void Pause()
    {
        IsPaused = true;
        if (pauseRoot) pauseRoot.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Resume()
    {
        IsPaused = false;
        if (pauseRoot) pauseRoot.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnBackButton() => Resume();
}
