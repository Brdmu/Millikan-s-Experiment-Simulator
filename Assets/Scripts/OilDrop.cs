using UnityEngine;

public class OilDrop : MonoBehaviour
{
  private Rigidbody rb;
  private Renderer rend;

  [Header("Физические параметры (СИ)")]
  private float radius;
  private float charge;
  private float massEffective;

  // Константы точно по скриншотам Википедии
  const float rhoOil = 858f;      // Плотность масла
  const float rhoAir = 1.29f;     // Плотность воздуха
  const float eta = 1.83e-5f;     // Вязкость воздуха
  const float g = 9.81f;
  const float e = 1.602e-19f;     // Элементарный заряд

  // Коэффициент перевода реальных метров в юниты Unity
  // Если расстояние между пластинами 5 юнитов = 5 мм (0.005 м), то 1 м = 1000 юнитов
  const float unityScaleFactor = 1000f;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rend = GetComponent<Renderer>();

    // 1. Радиус (от 1 до 3 мкм, как в реальном опыте)
    radius = Random.Range(1.0e-6f, 3.0e-6f);

    // 2. Заряд (от 1 до 10 электронов)
    int n = Random.Range(1, 10);
    charge = n * e;

    // 3. Эффективная масса (сразу вычитаем силу Архимеда: rho - rho_air)
    float volume = (4f / 3f) * Mathf.PI * Mathf.Pow(radius, 3);
    massEffective = volume * (rhoOil - rhoAir);

    // Отключаем гравитацию Unity, массу оставляем 1 (на нее мы больше не смотрим)
    rb.useGravity = false;

    // --- БЛОК ВИЗУАЛИЗАЦИИ ---
    float visualScale = radius * 40000f;
    transform.localScale = new Vector3(visualScale, visualScale, visualScale);

    if (rend != null)
    {
      rend.material = new Material(rend.material);
      float t = Mathf.InverseLerp(1.0e-6f, 3.0e-6f, radius);
      float intensity = Mathf.Lerp(0.3f, 1.2f, t);
      Color emissionColor = Color.white * intensity;
      rend.material.SetColor("_EmissionColor", emissionColor);
      rend.material.EnableKeyword("_EMISSION");
    }
  }

  void FixedUpdate()
  {
    // 1. Получаем напряжение
    float u = ExperimentManager.Instance.currentVoltage;
    float d = 0.005f; // 5 мм в реальности

    // 2. Напряженность поля E
    // Электрическое поле есть ТОЛЬКО между пластинами (Y от -2.5 до 2.5)
    float E = 0f;

    // Учитываем небольшую погрешность (2.49 вместо 2.5), 
    // чтобы поле выключалось чуть раньше, чем капля коснется самого верха трубы
    if (transform.position.y < 2.49f && transform.position.y > -2.49f)
    {
      E = u / d;
    }

    // 3. Вычисляем силы в Ньютонах
    // Если капля выше 2.5 или ниже -2.5, fElectric будет ровно 0
    float fElectric = charge * E;
    float fGravity = massEffective * g;

    // 4. Результирующая сила (вверх - плюс, вниз - минус)
    float netForce = fElectric - fGravity;

    // 5. ВЫЧИСЛЯЕМ УСТАНОВИВШУЮСЯ СКОРОСТЬ
    float vReal = netForce / (6f * Mathf.PI * eta * radius);

    // 6. Переводим в масштаб Unity
    float vUnity = vReal * unityScaleFactor;

    // 7. Применяем скорость
    rb.linearVelocity = new Vector3(0, vUnity, 0);

    // --- Очистка сцены ---
    // Если капля улетела далеко вниз или застряла высоко в трубе — удаляем её
    if (transform.position.y > 6f || transform.position.y < -3.0f)
    {
      Destroy(gameObject);
    }
  }
}