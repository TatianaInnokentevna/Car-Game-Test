using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GhostRaceManager : MonoBehaviour
{
    public enum RaceState
    {
        PlayerRace,
        GhostRace,
        Finished
    }

    public GameObject playerCar; // Машина игрока
    public GameObject ghostCarPrefab; // Префаб машины-призрака
    
    public CanvasButtons reStartbtn; // Кнопка для перезапуска
    public CanvasButtons Startbtn;
    public float recordInterval = 0.1f; // Интервал записи точек

    public Text raceStatusText; // Ссылка на компонент Text для отображения текущего круга
    private bool isFirstRace = true; // Переменная для отслеживания текущего заезда

    private RouteData routeData = new RouteData();
    private float timeSinceLastRecord = 0f;
    private bool isRecording = false;
    private GameObject ghostCar; // Экземпляр машины-призрака
    private RaceState currentState = RaceState.PlayerRace; // Текущее состояние гонки

    private Vector3 initialPlayerPosition;
    private Quaternion initialPlayerRotation;

    void Start()
    {
        // Сохраняем стартовую позицию машины игрока
        initialPlayerPosition = playerCar.transform.position;
        initialPlayerRotation = playerCar.transform.rotation;

        // Устанавливаем текст первого круга
        UpdateRaceStatus(isFirstRace);

        StartPlayerRace();
    }

    void Update()
    {
        if (isRecording)
        {
            timeSinceLastRecord += Time.deltaTime;
            if (timeSinceLastRecord >= recordInterval)
            {
                RecordPoint();
                timeSinceLastRecord = 0f;
            }
        }
    }

    private void StartPlayerRace()
    {
        currentState = RaceState.PlayerRace;
        isRecording = true;
        RecordPoint();
        timeSinceLastRecord = 0f;
        Debug.Log("Начинается заезд игрока.");
    }

    private void StartGhostRace()
    {
        Debug.Log("Начинается заезд с призраком.");
        currentState = RaceState.GhostRace;
        isRecording = false;

        // Проверяем наличие записанных точек маршрута
        if (routeData.points.Count > 0)
        {
            // Первая точка маршрута
            var startPoint = routeData.points[0];

            // Создаём машину-призрака на первой точке маршрута
            ghostCar = Instantiate(ghostCarPrefab, startPoint.position, startPoint.rotation);
            ghostCar.GetComponent<Rigidbody>().isKinematic = true;

            // Запускаем проигрывание маршрута
            StartCoroutine(PlayGhostRoute());
        }
        else
        {
            Debug.LogWarning("Маршрут для машины-призрака пуст! Убедитесь, что запись маршрута работает.");
        }

        // Возвращаем машину игрока на стартовую позицию
        playerCar.transform.position = initialPlayerPosition;
        playerCar.transform.rotation = initialPlayerRotation;

        // Обновляем статус
        isFirstRace = !isFirstRace;
        UpdateRaceStatus(isFirstRace);
    }

    private void FinishRace()
    {
        Debug.Log("Гонка завершена.");
        currentState = RaceState.Finished;
        isRecording = false;

        if (ghostCar != null)
        {
            Destroy(ghostCar); // Уничтожаем призрака
        }

        // Показываем кнопку перезапуска
        reStartbtn.gameObject.SetActive(true);
        Startbtn.gameObject.SetActive(true);
    }

    private void RecordPoint()
    {
        if (isRecording)
        {
            routeData.points.Add(new RoutePoint(playerCar.transform.position, playerCar.transform.rotation));
        }
    }

    private IEnumerator PlayGhostRoute()
    {
        foreach (var point in routeData.points)
        {
            if (ghostCar != null)
            {
                ghostCar.transform.position = point.position;
                ghostCar.transform.rotation = point.rotation;
                yield return new WaitForSeconds(recordInterval);
            }
        }

        Debug.Log("Призрак завершил маршрут.");
        FinishRace();
    }

    public void OnPlayerFinish()
    {
        Debug.Log("Игрок завершил заезд!");
        if (currentState == RaceState.PlayerRace)
        {
            StartGhostRace();
        }
        else if (currentState == RaceState.GhostRace)
        {
            FinishRace();
        }
    }

    private void UpdateRaceStatus(bool firstRace)
    {
        if (raceStatusText != null)
        {
            raceStatusText.text = firstRace ? "1 st ROUND" : "2 nd ROUND";
        }
        else
        {
            Debug.LogWarning("Текст для отображения статуса не привязан!");
        }
    }
}
