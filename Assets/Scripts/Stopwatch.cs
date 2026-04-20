using UnityEngine;
using TMPro;
using System.Collections;

public class Stopwatch : MonoBehaviour
{
   private int sec = 0;
   private int min = 0;
   public TMP_Text stopwatchText;
   [SerializeField] private int delta = 1;
   private Coroutine timerCoroutine;
   public TMP_Text playPauseButText;

   public TMP_Text saveTimeText1;
   public TMP_Text saveTimeText2;
   public TMP_Text saveTimeText3;
   private int saveTimeNum = 1;


  IEnumerator ITimer()
   {
      while ( true )
      {
         if ( sec == 59 )
         {
            min++;
            sec = -1;
         }
         sec += delta;
         stopwatchText.text = min.ToString("D2") + ":" + sec.ToString("D2");
         yield return new WaitForSeconds(1);
      }
   }

   // Запуск/Остановка секундомера
   public void StartStopwatch()
   {
      if ( timerCoroutine == null )
      {
         timerCoroutine = StartCoroutine(ITimer());
         playPauseButText.text = "Пауза";
      }
      else
      {
         StopCoroutine(timerCoroutine);
         timerCoroutine = null;
         playPauseButText.text = "Пуск";
      }
   }

   // Сброс секундомера
   public void ResetStopwatch()
   {
      if ( timerCoroutine != null )
      {
         StopCoroutine(timerCoroutine);
         timerCoroutine = null;
      }
      playPauseButText.text = "Пуск";

      sec = 0;
      min = 0;
      stopwatchText.text = min.ToString("D2") + ":" + sec.ToString("D2");
   }

   public void saveTime()
   {
      if ( saveTimeNum == 4 )
      {
         saveTimeNum = 1;
      }
      if ( saveTimeNum == 3 )
      {
         saveTimeText3.text = stopwatchText.text;
      }
      if ( saveTimeNum == 2 )
      {
         saveTimeText2.text = stopwatchText.text;
      }
      if ( saveTimeNum == 1)
      {
         saveTimeText1.text = stopwatchText.text;
      }
      saveTimeNum++;
   }

}
