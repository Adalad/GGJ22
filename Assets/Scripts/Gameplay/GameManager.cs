using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Cell[,] Grid;
    public int Columns = 110;
    public int Rows = 72;
    public int MaxAllowedTroops = 3;
    public GameplayCanvas GameCanvas;

    private Countries PlayerCountry;
    private int[][] Directions;
    private Vector2 CursorPosition = Vector2.zero;
    private Cell CurrentCell;
    private bool Editing = false;

    private void Start()
    {
        instance = this;
        Directions = new int[6][]
        {
            new int[] { 0, -1 },
            new int[] { 1, 0 },
            new int[] { 1, 1 },
            new int[] { 0, 1 },
            new int[] { -1, 1 },
            new int[] { -1, 0 }
        };
        PlayerCountry = (Countries)(PlayerPrefs.GetInt("Country", 0) + 1);
        Grid = new Cell[Columns, Rows];
        Cell[] cells = GameObject.FindObjectsOfType<Cell>();
        foreach (Cell cell in cells)
        {
            Grid[cell.XCoordinate, cell.YCoordinate] = cell;
        }

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                Cell cell = Grid[i, j];
                if ((cell.Country != Countries.NEUTRAL) && (NeighboursWithFrontiers(ref cell) > 0))
                {
                    cell.SetTroops(Random.Range(0, MaxAllowedTroops + 1));
                }
            }
        }

        GameCanvas.CPUFinished();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed && !Editing)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(CursorPosition);

            if (Physics.Raycast(ray, out hit))
            {
                Cell cell;
                if (hit.transform.TryGetComponent<Cell>(out cell) && (cell.Country == PlayerCountry))
                {
                    CurrentCell = cell;
                    GameCanvas.EditTroops(CurrentCell.Troops);
                    Editing = true;
                }
            }
        }
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        CursorPosition = context.ReadValue<Vector2>();
    }

    public void SetTroops(int troops)
    {
        Editing = false;
        if ((0 <= troops) && (troops <= MaxAllowedTroops))
        {
            CurrentCell.SetTroops(troops);
        }
    }

    public void EndTurn()
    {
        StartCoroutine(UpdateCells());
    }

    private IEnumerator UpdateCells()
    {
        int totalTroops = 0;
        // Backup current troops
        int[,] tempTroops = new int[Columns, Rows];
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (Grid[i, j] == null)
                {
                    Debug.Log(i + " " + j);
                }

                tempTroops[i, j] = Grid[i, j].Troops;
            }
        }

        int neighbours = 0;
        int newTroops = 0;
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                Cell cell = Grid[i, j];
                if ((cell.Country != PlayerCountry) && (cell.Country != Countries.NEUTRAL))
                {
                    neighbours = NeighboursWithEnemies(ref cell);
                    newTroops = NewTroopsForCell(tempTroops[i, j], neighbours);

                    if (newTroops > MaxAllowedTroops)
                    {
                        PlayerPrefs.SetInt("Result", 0);
                        SceneManager.LoadScene("Ending");
                    }
                    else
                    {
                        totalTroops += newTroops;
                        cell.SetTroops(newTroops);
                    }
                }
            }
        }

        if (totalTroops == 0)
        {
            PlayerPrefs.SetInt("Result", 1);
            SceneManager.LoadScene("Ending");
        }

        yield return new WaitForSeconds(2);

        GameCanvas.CPUFinished();
    }

    private int NeighboursWithEnemies(ref Cell cell)
    {
        int result = 0;
        int newX = 0;
        int newY = 0;
        for (int dir = 0; dir < Directions.Length; dir++)
        {
            newX = cell.XCoordinate + Directions[dir][0];
            newY = cell.YCoordinate + Directions[dir][1];
            if ((0 <= newX) && (newX < Columns) && (0 <= newY) && (newY < Rows))
            {
                if ((Grid[newX, newY].Country == PlayerCountry) && (Grid[newX, newY].Troops > 0))
                {
                    result += Grid[newX, newY].Troops;
                }
            }
        }

        return result;
    }

    private int NeighboursWithFrontiers(ref Cell cell)
    {
        int result = 0;
        int newX = 0;
        int newY = 0;
        for (int dir = 0; dir < Directions.Length; dir++)
        {
            newX = cell.XCoordinate + Directions[dir][0];
            newY = cell.YCoordinate + Directions[dir][1];
            if ((0 <= newX) && (newX < Columns) && (0 <= newY) && (newY < Rows))
            {
                if ((Grid[newX, newY].Country != cell.Country) && (Grid[newX, newY].Country != Countries.NEUTRAL))
                {
                    result++;
                }
            }
        }

        return result;
    }

    private int NewTroopsForCell(int currentTroops, int enemiesAround)
    {
        int result = currentTroops;

        if (enemiesAround > currentTroops + 1)
        {
            result++;
        }
        else if (enemiesAround < currentTroops)
        {
            result--;
        }

        if (result < 0)
        {
            result = 0;
        }

        return result;
    }
}
