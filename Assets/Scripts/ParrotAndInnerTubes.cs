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

    private int leftShoulder;
    
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
        leftShoulder = 1;
        PlayerPrefs.SetInt("currentShoulder", leftShoulder);
    }

    public void RightShoulder()
    {
        parrotPrefab.transform.position = rightShoulderParent.position;
        leftShoulder = 0;
        PlayerPrefs.SetInt("currentShoulder", leftShoulder);
    }

    public void LeftInnerTube()
    {
        innerTubes[currentIndex].SetActive(false);
        firstPersonInnerTubes[currentIndex].SetActive(false);
        currentIndex = (currentIndex - 1  + innerTubes.Count) % innerTubes.Count;
        innerTubes[currentIndex].SetActive(true);
        firstPersonInnerTubes[currentIndex].SetActive(true);
        textMesh.text = buttonText[currentIndex];
        PlayerPrefs.SetInt("currentFloat", currentIndex);
    }

    public void RightInnerTube()
    {
        innerTubes[currentIndex].SetActive(false);
        firstPersonInnerTubes[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % innerTubes.Count;
        innerTubes[currentIndex].SetActive(true);
        firstPersonInnerTubes[currentIndex].SetActive(true);
        textMesh.text = buttonText[currentIndex];
        PlayerPrefs.SetInt("currentFloat", currentIndex);
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
        
        currentIndex = PlayerPrefs.GetInt("currentFloat");
        for (int i = 0; i < innerTubes.Count; i++)
        {
            if (i == currentIndex)
            {
                innerTubes[i].SetActive(true);
                firstPersonInnerTubes[i].SetActive(true);
            }
            else
            {
                innerTubes[i].SetActive(false);
                firstPersonInnerTubes[i].SetActive(false);
            }
        }
        if (PlayerPrefs.GetInt("currentShoulder") == 1)
        {
            parrotPrefab.transform.position = leftShoulderParent.position;
        }
        else
        {
            parrotPrefab.transform.position = rightShoulderParent.position;
        }
    }
}
