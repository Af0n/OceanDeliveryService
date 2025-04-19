using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ParrotAndInnerTubes : MonoBehaviour
{
    public GameObject parrotPrefab;
    public Transform rightShoulderParent;
    public Transform leftShoulderParent;
    
    public List<GameObject> innerTubes = new List<GameObject>();
    public List<GameObject> firstPersonInnerTubes = new List<GameObject>();
    public List<string> buttonText = new List<string>();
    private int currentIndex = 0;
    
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    
    public TextMeshProUGUI textMesh;
    
    void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume")){
            LoadSettings();
        } else {
            SetMusicVolume();
            SetSfxVolume();
        }
    }

    public void LeftShoulder()
    {
        parrotPrefab.transform.position = leftShoulderParent.position;
    }

    public void RightShoulder()
    {
        parrotPrefab.transform.position = rightShoulderParent.position;
    }

    public void LeftInnerTube()
    {
        innerTubes[currentIndex].SetActive(false);
        firstPersonInnerTubes[currentIndex].SetActive(false);
        currentIndex = (currentIndex - 1  + innerTubes.Count) % innerTubes.Count;
        innerTubes[currentIndex].SetActive(true);
        firstPersonInnerTubes[currentIndex].SetActive(true);
        textMesh.text = buttonText[currentIndex];
    }

    public void RightInnerTube()
    {
        innerTubes[currentIndex].SetActive(false);
        firstPersonInnerTubes[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % innerTubes.Count;
        innerTubes[currentIndex].SetActive(true);
        firstPersonInnerTubes[currentIndex].SetActive(true);
        textMesh.text = buttonText[currentIndex];
    }
    
    public void SetMusicVolume(){
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume(){
        float volume = sfxSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
    
    private void LoadSettings(){
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetSfxVolume();
    }
}
