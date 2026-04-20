using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera cam;

    [Header("Настройки зума")]
    public float zoomSpeed = 5f;      // Скорость изменения
    public float minSize = 1f;        // Максимальное приближение
    public float maxSize = 8f;        // Максимальное отдаление (начальный размер был 5)

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Считываем прокрутку колесика мыши
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            // Вычисляем новый размер
            float newSize = cam.orthographicSize - scroll * zoomSpeed;

            // Ограничиваем размер в заданных пределах, чтобы не "улететь" слишком далеко
            cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
        }
    }
}