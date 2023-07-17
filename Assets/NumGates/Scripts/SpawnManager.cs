using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Reflection;
using DG.Tweening.Core.Easing;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefs;
    [SerializeField] private float spawnDelay = 0.25f;
    [SerializeField] private float spawnXOffset = 0.1f;
    [SerializeField] private float spawnYOffset = 0.1f;

    private GameObject parent;
    private int row = 0;
    private int col = 0;

    private const int BASE_ZOOM_LEVEL = 3;
    private int zoomLevel = 0;

    private Transform starter;
    private int counter = 0;

    private float movingTime = 0.5f;

    private GameManager gameManager;
    private GameplayManager gameplayManager;
    private CameraManager cameraManager;

    #region Initialize
    public void Initialize()
    {
        gameManager = GameManager.Instance;
        gameplayManager = gameManager.GameplayManager;
        cameraManager = gameManager.CameraManager;

        EnableAction();
    }

    public void Termiante()
    {
        DisableAction();
    }

    private void EnableAction()
    {
        gameManager.OnEnterCustom += EnterCustom;
        gameManager.OnExitCustom += ExitCustom;

        gameplayManager.OnStartGame += StartGame;
        gameplayManager.OnPlayGame += PlayGame;
        gameplayManager.OnEndGame += EndGame;
        gameplayManager.OnPauseGame += PauseGame;
        gameplayManager.OnResumeGame += ResumeGame;
        gameplayManager.OnRestartGame += RestartGame;
        gameplayManager.OnExitGame += ExitGame;
    }

    private void DisableAction()
    {
        gameManager.OnEnterCustom -= EnterCustom;
        gameManager.OnExitCustom -= ExitCustom;

        gameplayManager.OnStartGame -= StartGame;
        gameplayManager.OnPlayGame -= PlayGame;
        gameplayManager.OnEndGame -= EndGame;
        gameplayManager.OnPauseGame -= PauseGame;
        gameplayManager.OnResumeGame -= ResumeGame;
        gameplayManager.OnRestartGame -= RestartGame;
        gameplayManager.OnExitGame -= ExitGame;
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SpawnUpperRow();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnLowerRow();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnLeftColumn();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SpawnRightColumn();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(IsSymmetric())
            {
                SpawnSquareCircle();
            }
            else
            {
                SpawnRectCircle();
            }
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(row == 1 && col == 1)
            {
                SpawnFixRowAndColumn(3, 3);
            }
            else
            {
                if(row < col)
                {
                    row++;
                }
                else
                {
                    col++;
                }
                SpawnFixRowAndColumn(row, col);
            }
        }
    }

    #region Public Getting
    public int GetButtonAmount()
    {
        return row * col;
    }

    public int GetRow()
    {
        return row;
    }

    public int GetColumn()
    {
        return col;
    }

    public int GetMaxActivateAmount()
    {
        return Mathf.FloorToInt((row * col) * 0.7f);
    }

    public int GetMaxWave()
    {
        return Mathf.FloorToInt((row * col) / 2);
    }

    public int GetMaxLevel()
    {
        return GetMaxWave() * row;
    }
    #endregion

    #region Public Method
    public void SpawnNextLevel()
    {
        InitChildrenButtons(parent.transform);

        if(row == 1 && col == 1) { SpawnSquareCircle(); }
        else
        {
            if(row + 1 < col)
            {
                if (row % 2 == 0) { SpawnUpperRow(); }
                else { SpawnLowerRow(); }
            }
            else
            {
                if(col % 2 == 0) { SpawnLeftColumn(); }
                else { SpawnRightColumn(); }
            }
        }
    }

    public void ActivateButtons(List<int> indexes)
    {
        foreach (int index in indexes)
        {
            GameButton button = parent.transform.GetChild(index).GetComponent<GameButton>();
            button.ActivateButton();
        }
    }

    public void DeactivateButtons()
    {
        DeactivateChildrenButtons(parent.transform);
    }
    #endregion

    #region Helper Getting
    private string GetButtonName()
    {
        string name = $"Button_{counter}";
        counter++;
        return name;
    }

    private int GetZoomLevel()
    {
        return (row > BASE_ZOOM_LEVEL) ? row / BASE_ZOOM_LEVEL : 0;
    }

    private float GetZoomTime()
    {
        return (GetZoomLevel() > zoomLevel) ? cameraManager.ZoomSpeed : 0f;
    }

    private Vector3 GetStarterPosition()
    {
        return starter.transform.position;
    }

    private Vector3 GetParentPosition()
    {
        return parent.transform.position;
    }

    private Vector3 GetGridPosition(int row, int col)
    {
        Vector3 position = Vector3.zero;

        position.x = GetStarterPosition().x + ((1 * row) + (row * spawnXOffset));
        position.y = GetStarterPosition().y - ((1 * col) + (col * spawnYOffset));

        return position;
    }

    private Vector3 GetUpperRowPosition(int index)
    {
        Vector3 position = Vector3.zero;

        position.x = GetStarterPosition().x + (index + (index * spawnXOffset));
        position.y = GetStarterPosition().y + (1 + (1 * spawnYOffset));

        return position;
    }

    private Vector3 GetLowerRowPosition(int index)
    {
        Vector3 position = Vector3.zero;

        position.x = GetStarterPosition().x + (index + (index * spawnXOffset));
        position.y = GetStarterPosition().y - (row - 1 + ((row - 1) * spawnYOffset));

        return position;
    }

    private Vector3 GetLeftColumnPosition(int index)
    {
        Vector3 position = Vector3.zero;

        position.x = GetStarterPosition().x - (1 + (1 * spawnXOffset));
        position.y = GetStarterPosition().y - (index + (index * spawnYOffset));

        return position;
    }

    private Vector3 GetRightColumnPosition(int index)
    {
        Vector3 position = Vector3.zero;

        position.x = GetStarterPosition().x + (col - 1 + ((col - 1) * spawnXOffset));
        position.y = GetStarterPosition().y - (index + (index * spawnYOffset));

        return position;
    }

    private Vector3 GetSquareCirclePosition(int index)
    {
        Vector3 position = Vector3.zero;

        int divider = row - 1;
        int tmpIndex = index % divider;

        switch(index / divider)
        {
            case 0:
                {
                    position.x = GetStarterPosition().x + (tmpIndex - 1 + ((tmpIndex - 1) * spawnXOffset));
                    position.y = GetStarterPosition().y + (1 + (1 * spawnYOffset));
                    break; 
                }
            case 1:
                {
                    position.x = GetStarterPosition().x + (divider - 1 + ((divider - 1) * spawnXOffset));
                    position.y = GetStarterPosition().y - (tmpIndex - 1 + ((tmpIndex - 1) * spawnYOffset));
                    break;
                }
            case 2:
                {
                    position.x = GetStarterPosition().x + (divider - 1 - tmpIndex + ((divider - 1 - tmpIndex) * spawnXOffset));
                    position.y = GetStarterPosition().y - (divider - 1 + ((divider - 1) * spawnYOffset));
                    break;
                }
            case 3:
                {
                    position.x = GetStarterPosition().x - (1 + (1 * spawnXOffset));
                    position.y = GetStarterPosition().y - (divider - 1 - tmpIndex + ((divider - 1 - tmpIndex) * spawnYOffset));
                    break;
                }
        }

        return position;
    }

    private Vector3 GetRectCirclePosition(int index)
    {
        Vector3 position = Vector3.zero;

        int divider = (row - 1) + (col - 1);
        int tmpIndex = (index % divider < col - 1) ? index % divider : (index % divider) - col;

        // First section
        if(index / divider == 0)
        {
            if(index % divider < col - 1)
            {
                position.x = GetStarterPosition().x + (tmpIndex - 1 + ((tmpIndex - 1) * spawnXOffset));
                position.y = GetStarterPosition().y + (1 + (1 * spawnYOffset));
            }
            else
            {
                position.x = GetStarterPosition().x + (col - 2 + ((col - 2) * spawnXOffset));
                position.y = GetStarterPosition().y - (tmpIndex + (tmpIndex * spawnYOffset));
            }
        }
        // Second section
        else
        {
            if (index % divider < col - 1)
            {
                int actualX = col - 2;
                position.x = GetStarterPosition().x + (actualX - tmpIndex + ((actualX - tmpIndex) * spawnXOffset));
                position.y = GetStarterPosition().y - (row - 2 + ((row - 2) * spawnYOffset));
            }
            else
            {
                int actualY = row - 3;
                position.x = GetStarterPosition().x - (1 + (1 * spawnXOffset));
                position.y = GetStarterPosition().y - (actualY - tmpIndex + ((actualY - tmpIndex) * spawnYOffset));
            }
        }

        return position;
    }

    private float GetXMoveTarget()
    {
        return (1f + (1f * spawnXOffset)) / 2; // TODO: Change 1f to variable
    }

    private float GetYMoveTarget()
    {
        return (1f + (1f * spawnYOffset)) / 2; // TODO: Change 1f to variable
    }

    private float GetActalSpawnDelay(int amount)
    {
        return spawnDelay / amount;
    }
    #endregion

    #region Boolean
    private bool IsSymmetric()
    {
        return row == col;
    }
    #endregion

    #region Center Spawn
    private void SpawnCenter()
    {
        row = 1; col = 1;

        StartCoroutine(OnZoom(GetZoomLevel(), () =>
        {
            StartCoroutine(OnSpawnCenter(() =>
            {
                GameManager.Instance.LevelManager.InitLevel();
            }));
        }));
        
    }

    private IEnumerator OnSpawnCenter(Action callback)
    {
        GameObject button = Instantiate(buttonPrefs, parent.transform);
        button.name = GetButtonName();
        button.GetComponent<GameButton>().InitButton();

        starter = button.transform;

        yield return new WaitForSecondsRealtime(spawnDelay);

        DeactivateChildrenButtons(parent.transform);

        callback?.Invoke();
    }
    #endregion

    #region Circle Spawn
    private void SpawnSquareCircle()
    {
        row += 2;
        col += 2;

        StartCoroutine(OnZoom(GetZoomLevel(), () =>
        {
            StartCoroutine(OnSpawnSquareCircle(() =>
            {
                GameManager.Instance.LevelManager.GenerateWave();
            }));
        }));
    }

    private IEnumerator OnSpawnSquareCircle(Action callback)
    {
        Transform tmpStarter = starter;
        int lastedRow = row - 2;
        
        int amount = (row * row) - (lastedRow * lastedRow);

        for (int i = 0; i < amount; i++)
        {
            GameObject button = Instantiate(buttonPrefs, parent.transform);
            button.name = GetButtonName();
            button.transform.position = GetSquareCirclePosition(i);
            button.GetComponent<GameButton>().InitButton();
            if (i == 0) tmpStarter = button.transform;
            yield return new WaitForSecondsRealtime(spawnDelay);
        }

        starter = tmpStarter;

        DeactivateChildrenButtons(parent.transform);

        callback?.Invoke();
    }

    private void SpawnRectCircle()
    {
        row += 2;
        col += 2;

        StartCoroutine(OnZoom(GetZoomLevel(), () =>
        {
            StartCoroutine(OnSpawnRectCircle(() =>
            {
                GameManager.Instance.LevelManager.GenerateWave();
            }));
        }));
        
    }

    private IEnumerator OnSpawnRectCircle(Action callback)
    {
        Transform tmpStarter = starter;

        int amount = ((row - 3) + (col - 3)) * 2;

        for (int i = 0; i < amount; i++)
        {
            GameObject button = Instantiate(buttonPrefs, parent.transform);
            button.name = GetButtonName();
            button.transform.position = GetRectCirclePosition(i);
            button.GetComponent<GameButton>().InitButton();
            if (i == 0) tmpStarter = button.transform;
            yield return new WaitForSecondsRealtime(spawnDelay);
        }

        starter = tmpStarter;

        DeactivateChildrenButtons(parent.transform);

        callback?.Invoke();
    }
    #endregion

    #region Row Spawner
    private void SpawnUpperRow()
    {
        row++;

        StartCoroutine(OnZoom(GetZoomLevel(), () =>
        {
            Vector3 moveTarget = new Vector3(0f, -GetYMoveTarget(), 0f) + GetParentPosition();

            StartCoroutine(OnMovingParent(moveTarget, () =>
            {
                StartCoroutine(OnSpawnUpperRow(() =>
                {
                    GameManager.Instance.LevelManager.GenerateWave();
                }));
            }));
        }));
    }

    private IEnumerator OnSpawnUpperRow(Action callback)
    {
        Transform tmpStarter = starter;

        for (int i = 0; i < col; i++)
        {
            GameObject button = Instantiate(buttonPrefs, parent.transform);
            button.name = GetButtonName();
            button.transform.position = GetUpperRowPosition(i);
            button.GetComponent<GameButton>().InitButton();
            if (i == 0) tmpStarter = button.transform;
            yield return new WaitForSecondsRealtime(spawnDelay);
        }

        starter = tmpStarter;

        DeactivateChildrenButtons(parent.transform);

        callback?.Invoke();
    }

    private void SpawnLowerRow()
    {
        row++;

        StartCoroutine(OnZoom(GetZoomLevel(), () =>
        {
            Vector3 moveTarget = new Vector3(0f, GetYMoveTarget(), 0f) + GetParentPosition();

            StartCoroutine(OnMovingParent(moveTarget, () =>
            {
                StartCoroutine(OnSpawnLowerRow(() =>
                {
                    GameManager.Instance.LevelManager.GenerateWave();
                }));
            }));
        }));
    }

    private IEnumerator OnSpawnLowerRow(Action callback)
    {
        for (int i = 0; i < col; i++)
        {
            GameObject button = Instantiate(buttonPrefs, parent.transform);
            button.name = GetButtonName();
            button.transform.position = GetLowerRowPosition(i);
            button.GetComponent<GameButton>().InitButton();
            yield return new WaitForSecondsRealtime(spawnDelay);
        }

        DeactivateChildrenButtons(parent.transform);

        callback?.Invoke();
    }
    #endregion

    #region Column Spawner
    private void SpawnRightColumn()
    {
        col++;

        StartCoroutine(OnZoom(GetZoomLevel(), () =>
        {
            Vector3 moveTarget = new Vector3(-GetXMoveTarget(), 0f, 0f) + GetParentPosition();

            StartCoroutine(OnMovingParent(moveTarget, () =>
            {
                StartCoroutine(OnSpawnRightColumn(() =>
                {
                    GameManager.Instance.LevelManager.GenerateWave();
                }));
            }));
        }));
    }

    private IEnumerator OnSpawnRightColumn(Action callback)
    {
        for (int i = 0; i < row; i++)
        {
            GameObject button = Instantiate(buttonPrefs, parent.transform);
            button.name = GetButtonName();
            button.transform.position = GetRightColumnPosition(i);
            button.GetComponent<GameButton>().InitButton();
            yield return new WaitForSecondsRealtime(spawnDelay);
        }

        DeactivateChildrenButtons(parent.transform);

        callback?.Invoke();
    }

    private void SpawnLeftColumn()
    {
        col++;

        StartCoroutine(OnZoom(GetZoomLevel(), () =>
        {
            Vector3 moveTarget = new Vector3(GetXMoveTarget(), 0f, 0f) + GetParentPosition();

            StartCoroutine(OnMovingParent(moveTarget, () =>
            {
                StartCoroutine(OnSpawnLeftColumn(() =>
                {
                    GameManager.Instance.LevelManager.GenerateWave();
                }));
            }));
        }));
    }

    private IEnumerator OnSpawnLeftColumn(Action callback)
    {
        Transform tmpStarter = starter;

        for (int i = 0; i < row; i++)
        {
            GameObject button = Instantiate(buttonPrefs, parent.transform);
            button.name = GetButtonName();
            button.transform.position = GetLeftColumnPosition(i);
            button.GetComponent<GameButton>().InitButton();
            if (i == 0) tmpStarter = button.transform;
            yield return new WaitForSecondsRealtime(spawnDelay);
        }

        starter = tmpStarter;

        DeactivateChildrenButtons(parent.transform);

        callback?.Invoke();
    }
    #endregion

    #region Fix Spawner
    private void SpawnFixRowAndColumn(int row, int col)
    {
        this.row = row;
        this.col = col;

        StartCoroutine(OnZoom(GetZoomLevel(), () =>
        {
            StartCoroutine(OnSpawnFixRowAndColumn(row, col, () =>
            {
                GameManager.Instance.LevelManager.GenerateWave();
            }));
        }));
    }

    private IEnumerator OnSpawnFixRowAndColumn(int row, int col, Action callback)
    {
        DestoryAllChildren(parent.transform);

        yield return new WaitForSecondsRealtime(spawnDelay);

        float xPos = ((1 * (col - 1)) + ((col - 1) * spawnXOffset)) / 2;
        float yPos = ((1 * (row - 1)) + ((row - 1) * spawnYOffset)) / 2;

        parent.transform.position = new Vector3(-xPos, yPos, 0f);

        for(int x = 0; x < row; x++)
        {
            for(int y = 0; y < col; y++)
            {
                GameObject button = Instantiate(buttonPrefs, parent.transform);
                if (x == 0 && y == 0) starter = button.transform;
                button.name = GetButtonName();
                button.transform.position = GetGridPosition(y, x);
                button.GetComponent<GameButton>().InitButton();
            }
        }

        yield return new WaitForSecondsRealtime(spawnDelay);

        DeactivateChildrenButtons(parent.transform);

        callback?.Invoke();
    }
    #endregion

    #region Custom Spawner
    private void SpawnCustomRowAndColumn(int row, int col)
    {
        this.row = row;
        this.col = col;

        StartCoroutine(OnZoom(GetZoomLevel(), () =>
        {
            StartCoroutine(OnSpawnCustomRowAndColumn(row, col));
        }));
    }

    private IEnumerator OnSpawnCustomRowAndColumn(int row, int col)
    {
        DestoryAllChildren(parent.transform);

        yield return new WaitForSecondsRealtime(spawnDelay);

        float xPos = ((1 * (col - 1)) + ((col - 1) * spawnXOffset)) / 2;
        float yPos = ((1 * (row - 1)) + ((row - 1) * spawnYOffset)) / 2;

        parent.transform.position = new Vector3(-xPos, yPos, 0f);

        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                GameObject button = Instantiate(buttonPrefs, parent.transform);
                if (x == 0 && y == 0) starter = button.transform;
                button.name = GetButtonName();
                button.transform.position = GetGridPosition(y, x);
            }
        }

        yield return new WaitForSecondsRealtime(spawnDelay);
    }
    #endregion

    #region Helper
    private IEnumerator OnMovingParent(Vector3 target, Action callback)
    {
        parent.transform.DOMove(target, movingTime);

        yield return new WaitForSecondsRealtime(movingTime);

        callback?.Invoke();
    }

    private IEnumerator OnZoom(int level, Action callback)
    {
        cameraManager.Zoom(level);

        yield return new WaitForSecondsRealtime(GetZoomTime());

        zoomLevel = level;

        callback?.Invoke();
    }

    private void InitChildrenButtons(Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameButton button = child.GetComponent<GameButton>();
            button.InitButton();
        }
    }

    private void DeactivateChildrenButtons(Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameButton button = child.GetComponent<GameButton>();
            button.DeactivateButton();
        }
    }

    private void DestoryAllChildren(Transform parent)
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion

    #region Gameplay Action
    private void StartGame()
    {
        if (parent == null)
        {
            parent = new GameObject("SpawnerParent");
        }

        //SpawnCenter();
        //SpawnFixRowAndColumn(1, 1);

        RelaxModeData data = SaveManager.LoadRelaxModeData();

        if(data.row > 1 && data.col > 1)
        {
            SpawnFixRowAndColumn(data.row, data.col);
        }
        else
        {
            SpawnCenter();
        }
    }

    private void PlayGame()
    {
        // 'Speed Mode' : update timer text
    }

    private void EndGame()
    {
        // 'Speed Mode' : show result
    }

    private void PauseGame()
    {
        // 'Speed Mode' : stop timer
    }

    private void ResumeGame()
    {
        // 'Spped Mode' : countdown and continue timer
    }

    private void RestartGame()
    {
        // 'Speed Mode' : 
        // 'Relax / Free' : hide restart button
    }

    private void ExitGame()
    {
        // 'Relax' : Save lasted level
        // 'Speed' : Save lasted level and timer
        StartCoroutine(OnZoom(0, () =>
        {
            DestoryAllChildren(parent.transform);
        }));
    }
    #endregion

    #region Game Action
    private void EnterCustom()
    {
        if (parent == null)
        {
            parent = new GameObject("SpawnerParent");
        }

        SpawnCustomRowAndColumn(gameManager.GetMaxRow(), gameManager.GetMaxColumn());
    }

    private void ExitCustom()
    {
        StartCoroutine(OnZoom(GetZoomLevel(), () =>
        {
            DestoryAllChildren(parent.transform);
        }));
    }
    #endregion
}
