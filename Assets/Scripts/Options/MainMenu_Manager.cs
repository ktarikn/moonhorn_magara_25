using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu_Manager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject optionsPanel; // Inspector'dan Ayarlar panelini s�r�kle

    [Header("Ses Ayarlar�")]
    public AudioMixer audioMixer; // Olu�turdu�un Audio Mixer'� s�r�kle
    public Slider masterVolumeSlider; // Master ses slider'�n� s�r�kle
    public Slider sfxVolumeSlider; // SFX ses slider'�n� s�r�kle

    [Header("Sahne Y�netimi")]
    [Tooltip("Oyunun ba�layaca�� sahnenin Build Settings'deki index'i")]
    public int gameSceneIndex = 1; // Ba�lat�lacak sahnenin index'i

    void Start()
    {
        // Ayarlar paneli ba�lang��ta kapal� olsun    if (optionsPanel is not null) { optionsPanel.SetActive(false);}
        optionsPanel?.SetActive(false);

        LoadSettings(); // Kay�tl� ayarlar� y�kle
    }

    #region Ana Men� Butonlar�

    public void StartGame()
    {       
        SceneManager.LoadScene(gameSceneIndex);  // Belirtilen index'teki sahneyi y�kle
    }

    public void OpenOptions()
    {        
        optionsPanel?.SetActive(true);// Ayarlar panelini a�
    }

    public void CloseOptions()
    {        
        optionsPanel?.SetActive(false);// Ayarlar panelini kapat
        SaveSettings(); // Panel kapan�rken ayarlar� kaydet
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game..."); // Edit�rde test i�in log mesaj�
        Application.Quit();

        // Edit�rde �al���rken oyunu durdurmak i�in
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion

    #region Ayarlar Fonksiyonlar�

    // Grafik ayar�n� de�i�tiren fonksiyon (Butonlar bu fonksiyonu �a��racak)
    // Low i�in 0, Medium i�in 1, High i�in 2 de�eri g�nderilecek
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Graphics Quality set to: " + QualitySettings.names[qualityIndex]);
        SaveSettings(); // Ayar� kaydet
    }

    // Master ses seviyesini ayarlayan fonksiyon (Slider bu fonksiyonu �a��racak)
    public void SetMasterVolume(float volume)
    {
        // Slider de�eri (0.0001-1) ile mixer'in logaritmik dB de�eri (-80 ila 0) aras�nda d�n���m yap�yoruz.
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    // SFX ses seviyesini ayarlayan fonksiyon (Slider bu fonksiyonu �a��racak)
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    #endregion

    #region Ayarlar� Kaydetme ve Y�kleme

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualityLevel", QualitySettings.GetQualityLevel());
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.Save(); // De�i�iklikleri diske kaydet
        Debug.Log("Settings Saved!");
    }

    public void LoadSettings()
    {
        // Kalite ayar�n� y�kle
        int quality = PlayerPrefs.GetInt("QualityLevel", 2); // Varsay�lan: High (index 2)
        SetQuality(quality);

        // Ses ayarlar�n� y�kle
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1f); // Varsay�lan: %100
        masterVolumeSlider.value = masterVol;
        SetMasterVolume(masterVol); // Mixer'� da g�ncelle

        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f); // Varsay�lan: %100
        sfxVolumeSlider.value = sfxVol;
        SetSFXVolume(sfxVol); // Mixer'� da g�ncelle

        Debug.Log("Settings Loaded!");
    }

    #endregion
}