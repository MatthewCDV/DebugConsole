using UnityEngine;
using TMPro;

public class MoneyScript : MonoBehaviour
{
    public static MoneyScript Instance { get; private set; }
    private void Awake()
    {
        // Simple singleton pattern
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"Destroying duplicate MoneyScript on '{gameObject.name}'. Use MoneyScript.Instance to access the money manager.");
            Destroy(this);
            return;
        }
        Instance = this;
    }   

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private int money = 0;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            AddMoney(10);
    }

    public void AddMoney(int amount)
    {
        if (amount == 0) return;
        money += amount;
        UpdateUI();
    }

    public void SetMoney(int amount)
    {
        money = amount;
        UpdateUI();
    }

    public int GetMoney() => money;

    private void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = FormatMoney(money);
    }

    private string FormatMoney(int value)
    {
        return value.ToString("N0") + " $";
    }
}
