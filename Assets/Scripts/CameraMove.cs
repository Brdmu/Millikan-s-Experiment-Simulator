using UnityEngine;

public class CameraMove : MonoBehaviour
{
   [Header("Movement Settings")]
   public float mainSpeed = 18.0f;
   public float shiftAdd = 250.0f;
   public float maxShift = 1000.0f;
   public float camSens = 1.8f; 

   private float rotationX = 0.0f;
   private float rotationY = 0.0f;
   private bool isRightMouseDown = false;
   private float totalRun = 1.0f;

   void Start()
   {
      // Инициализируем текущие углы вращения
      Vector3 currentRotation = transform.eulerAngles;
      rotationX = currentRotation.x;
      rotationY = currentRotation.y;
   }

   void Update()
   {
      // Вращение камеры только при удержании правой кнопки мыши
      if ( Input.GetMouseButton(1) )
      {
         if ( !isRightMouseDown )
         {
            isRightMouseDown = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Меняем на Locked для плавности
         }

         // Получаем входные данные мыши
         float mouseX = Input.GetAxis("Mouse X") * camSens;
         float mouseY = Input.GetAxis("Mouse Y") * camSens;

         // Накопление вращения
         rotationY += mouseX;
         rotationX -= mouseY;

         // Ограничиваем угол X чтобы камера не переворачивалась
         rotationX = Mathf.Clamp(rotationX, -90f, 90f);

         // Применяем вращение через Quaternion
         transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
      }
      else
      {
         if ( isRightMouseDown )
         {
            isRightMouseDown = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
         }
      }

      // Движение камеры (работает всегда)
      HandleKeyboardMovement();
   }

   private void HandleKeyboardMovement()
   {
      Vector3 p = GetBaseInput();
      if ( p.sqrMagnitude > 0 )
      {
         if ( Input.GetKey(KeyCode.LeftShift) )
         {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
         }
         else
         {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
         }

         p = p * Time.deltaTime;

         // Двигаем камеру относительно её текущего поворота
         if ( Input.GetKey(KeyCode.Space) )
         {
            // Движение только по горизонтальной плоскости (XZ)
            Vector3 horizontalMove = transform.TransformDirection(new Vector3(p.x, 0, p.z));
            transform.position += horizontalMove;
         }
         else
         {
            // Полное движение относительно камеры
            transform.Translate(p, Space.Self);
         }
      }
   }

   private Vector3 GetBaseInput()
   {
      Vector3 p_Velocity = new Vector3();
      if ( Input.GetKey(KeyCode.W) )
      {
         p_Velocity += new Vector3(0, 0, 1);
      }
      if ( Input.GetKey(KeyCode.S) )
      {
         p_Velocity += new Vector3(0, 0, -1);
      }
      if ( Input.GetKey(KeyCode.A) )
      {
         p_Velocity += new Vector3(-1, 0, 0);
      }
      if ( Input.GetKey(KeyCode.D) )
      {
         p_Velocity += new Vector3(1, 0, 0);
      }
      return p_Velocity;
   }
}