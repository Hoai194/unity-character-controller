using UnityEngine;

public class SelectedAnimal : MonoBehaviour
{
    [SerializeField] private GameObject visualSelectedAnimal;
    [SerializeField] private Animal animal;
    private void Start()
    {
        // Subscribe to the OnSelectedAnimalChanged event from the Player class
        Player.Instance.OnSelectedAnimalChanged += Player_OnSelectedAnimalChanged;
    }

    private void Player_OnSelectedAnimalChanged(object sender, Player.OnSelectedAnimalChangedEventArgs e)
    {
        if (e.selectedAnimal == animal)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        visualSelectedAnimal.SetActive(true);
    }
    private void Hide()
    {
        visualSelectedAnimal.SetActive(false);
    }
}
