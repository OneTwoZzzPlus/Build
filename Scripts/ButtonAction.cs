using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ButtonAction : MonoBehaviour
{
    public int index;
    private InputReader inputReader;
    private Button button;
    private UnityAction clickAction;

    public void Construct(int index, InputReader inputReader)
    {
        this.index = index;
        this.inputReader = inputReader;

        button = GetComponent<Button>();
        clickAction = new UnityAction(() => inputReader.ChoiceButtonAction(index));
        button.onClick.AddListener(clickAction);
    }
}
