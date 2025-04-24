using TMPro;
using UnityEngine;

public class ControlDisplay : MonoBehaviour
{
    [TextArea]
    public string Basic, Third, Boat;
    [Header("Unity Set Up")]
    public PlayerManager manager;

    private TextMeshProUGUI textBox;

    void Awake()
    {
        textBox = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if(manager.IsOnBoat){
            textBox.text = Boat;
            return;
        }

        if(manager.IsThirdPerson){
            textBox.text = Third;
            return;
        }

        textBox.text = Basic;
    }
}
