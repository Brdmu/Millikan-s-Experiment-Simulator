using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentManager : MonoBehaviour
{
  public static ExperimentManager Instance;

  [Header("Ссылки на объекты")]
  public GameObject oilDropPrefab;
  public Transform spawnPoint; // Пустой объект над дыркой

  [Header("UI Элементы")]
  public Slider voltageSlider;
  public TMP_Text voltageValueText;

  [Header("Физические параметры установки")]
  public float distanceBetweenPlates = 5f; // 5 мм в метрах
  public float currentVoltage;

  void Awake() => Instance = this;

  void Update()
  {
    currentVoltage = voltageSlider.value;
    if (voltageValueText != null)
      voltageValueText.text = $"Напряжение: {currentVoltage:F0} V";
  }

  // Метод для кнопки "Распылить"
  public void Spray()
  {
    int dropsCount = 15; // Сколько капель вылетает за раз
    float sprayRadius = 0.5f; // Радиус облака распыления

    for (int i = 0; i < dropsCount; i++)
    {
      // Создаем случайное смещение внутри круга
      Vector2 randomCircle = Random.insideUnitCircle * sprayRadius;
      Vector3 spawnOffset = new Vector3(randomCircle.x, 0, randomCircle.y);

      // Создаем каплю
      Instantiate(oilDropPrefab, spawnPoint.position + spawnOffset, Quaternion.identity);
    }
  }
}