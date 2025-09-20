using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu_Manager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject optionsPanel; // Inspector'dan Ayarlar panelini sürükle

    [Header("Ses Ayarlarý")]
    public AudioMixer audioMixer; // Oluþturduðun Audio Mixer'ý sürükle
    public Slider masterVolumeSlider; // Master ses slider'ýný sürükle
    public Slider sfxVolumeSlider; // SFX ses slider'ýný sürükle

    [Header("Sahne Yönetimi")]
    [Tooltip("Oyunun baþlayacaðý sahnenin Build Settings'deki index'i")]
    public int gameSceneIndex = 1; // Baþlatýlacak sahnenin index'i

    void Start()
    {
        // Ayarlar paneli baþlangýçta kapalý olsun    if (optionsPanel is not null) { optionsPanel.SetActive(false);}
        optionsPanel?.SetActive(false);

        LoadSettings(); // Kayýtlý ayarlarý yükle
    }

    #region Ana Menü Butonlarý

    public void StartGame()
    {       
        SceneManager.LoadScene(gameSceneIndex);  // Belirtilen index'teki sahneyi yükle
    }

    public void OpenOptions()
    {        
        optionsPanel?.SetActive(true);// Ayarlar panelini aç
    }

    public void CloseOptions()
    {        
        optionsPanel?.SetActive(false);// Ayarlar panelini kapat
        SaveSettings(); // Panel kapanýrken ayarlarý kaydet
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game..."); // Editörde test için log mesajý
        Application.Quit();

        // Editörde çalýþýrken oyunu durdurmak için
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion

    #region Ayarlar Fonksiyonlarý

    // Grafik ayarýný deðiþtiren fonksiyon (Butonlar bu fonksiyonu çaðýracak)
    // Low için 0, Medium için 1, High için 2 deðeri gönderilecek
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Graphics Quality set to: " + QualitySettings.names[qualityIndex]);
        SaveSettings(); // Ayarý kaydet
    }

    // Master ses seviyesini ayarlayan fonksiyon (Slider bu fonksiyonu çaðýracak)
    public void SetMasterVolume(float volume)
    {
        // Slider deðeri (0.0001-1) ile mixer'in logaritmik dB deðeri (-80 ila 0) arasýnda dönüþüm yapýyoruz.
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    // SFX ses seviyesini ayarlayan fonksiyon (Slider bu fonksiyonu çaðýracak)
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    #endregion

    #region Ayarlarý Kaydetme ve Yükleme

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualityLevel", QualitySettings.GetQualityLevel());
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.Save(); // Deðiþiklikleri diske kaydet
        Debug.Log("Settings Saved!");
    }

    public void LoadSettings()
    {
        // Kalite ayarýný yükle
        int quality = PlayerPrefs.GetInt("QualityLevel", 2); // Varsayýlan: High (index 2)
        SetQuality(quality);

        // Ses ayarlarýný yükle
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1f); // Varsayýlan: %100
        masterVolumeSlider.value = masterVol;
        SetMasterVolume(masterVol); // Mixer'ý da güncelle

        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f); // Varsayýlan: %100
        sfxVolumeSlider.value = sfxVol;
        SetSFXVolume(sfxVol); // Mixer'ý da güncelle

        Debug.Log("Settings Loaded!");
    }

    #endregion
}