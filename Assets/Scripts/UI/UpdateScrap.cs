using System.Collections;
using TMPro;
using UnityEngine;

public class UpdateScrap : MonoBehaviour
{
    [Tooltip("How long to wait between updates")]
    public float UpdateDelay;

    private TextMeshProUGUI textBox;

    private void Start()
    {
        textBox = GetComponent<TextMeshProUGUI>();
        StartCoroutine(nameof(UpdateLoop));
    }

    private IEnumerator UpdateLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(UpdateDelay);
            textBox.text = Economy.Scrap.ToString();
        }
    }
}
