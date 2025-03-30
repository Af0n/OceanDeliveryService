using TMPro;
using UnityEngine;

public class UpdateThirdPersonInteractUI : MonoBehaviour
{
    public TextMeshProUGUI TextBox;
    public Transform SingleView, MultiView;

    public PlayerManager manager;

    void Update()
    {
        EvalVisibility();

        EvalDisplay();
    }

    public void EvalVisibility(){
        SingleView.gameObject.SetActive(manager.HasThirdPersonInteractable);
        MultiView.gameObject.SetActive(manager.HasMultipleTPI);
    }

    public void EvalDisplay(){
        if(manager.interaction.SelectedInteractable == null){
            return;
        }
        TextBox.text = manager.interaction.SelectedInteractable.name;
    }
}
