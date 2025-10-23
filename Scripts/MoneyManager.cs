using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;
    public int money = 0;
    public GameObject floatingTextPrefab;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void AddMoney(int amount, Vector3 worldPos)
    {
        money += amount;
        SpawnFloatingText("+" + amount.ToString(), worldPos, true);
        // update UI text if any
    }

    public void RemoveMoney(int amount, Vector3 worldPos)
    {
        money -= amount;
        if (money < 0) money = 0;
        SpawnFloatingText("-" + amount.ToString(), worldPos, false);
    }

    private void SpawnFloatingText(string text, Vector3 pos, bool positive)
    {
        if (floatingTextPrefab == null) return;
        GameObject ft = Instantiate(floatingTextPrefab, pos, Quaternion.identity);
        // you should have a simple script on floatingTextPrefab that sets the text and animates upward
    }
}
