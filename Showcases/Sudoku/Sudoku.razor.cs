using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace ShowcaseLibrary.Showcases;

partial class Sudoku
{
    int _size = 9;
    int Size
    {
        get => _size;
        set
        {
            _size = value;
            StartNewGame();
        }
    }
    bool? hard = null;
    bool ready = false;
    bool solved = false;
    List<List<int>> board = [];
    List<List<int>> solution = [];
    List<List<bool>> revealed = [];
    RenderFragment RenderCell(int row, int col) =>
    builder =>
            {
                if (revealed[row][col])
                    RenderFixedCell(builder, row, col);
                else
                    RenderEditableCell(builder, row, col);
            };
    protected override void OnInitialized() => InitializeBoard(Size);
    private void RenderFixedCell(RenderTreeBuilder builder, int row, int col)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "bg-success-subtle d-flex flex-column justify-content-center align-items-center");
        builder.AddAttribute(2, "style", "border: 1px solid black; height: 50px; width: 50px; margin: 0; padding: 0; text-align: center;");
        builder.AddContent(3, solution[row][col]);
        builder.CloseElement();
    }

    private void RenderEditableCell(RenderTreeBuilder builder, int row, int col)
    {
        builder.OpenElement(4, "input");
        builder.AddAttribute(5, "type", "number");
        builder.AddAttribute(6, "value", board[row][col]);
        builder.AddAttribute(7, "style", "border: 1px solid black; height: 50px; width: 50px; margin: 0; padding: 0; text-align: center;");
        builder.AddAttribute(8, "oninput", EventCallback.Factory.CreateBinder<int>(this, value => InputChanged(value, row, col), board[row][col]));
        builder.AddAttribute(9, "name", $"{row}_{col}");
        builder.CloseElement();
    }

    void InputChanged(int value, int row, int col)
    {
        if (value > 0 && value <= Size)
        {
            board[row][col] = value;
        }
        else if (value < 0)
        {
            board[row][col] = Size;
        }
        else if (value > Size)
        {
            board[row][col] = 0;
        }
    }

    private void InitializeBoard(int size)
    {
        solved = false;
        board = [];
        solution = [];
        revealed = [];

        for (int i = 0; i < size; i++)
        {
            board.Add(new List<int>(new int[size]));
            solution.Add(new List<int>(new int[size]));
            revealed.Add(new List<bool>(new bool[size]));
        }

    }
    void StartNewGame()
    {
        InitializeBoard(Size);
        FillBoard();
        RevealStarters();
        ready = true;
    }

    bool FillBoard()
    {
        bool IsValidPlacement(int number, int row, int col)
        {
            return !solution[row].Contains(number) &&
            !solution.Any(r => r[col] == number) &&
            !IsNumberInBox(number, row, col);
        }

        bool IsNumberInBox(int number, int startRow, int startCol)
        {
            int boxSize = Size == 16 ? 4 : Size == 9 ? 3 : 2;
            int boxRow = startRow / boxSize * boxSize;
            int boxCol = startCol / boxSize * boxSize;
            for (int r = boxRow; r < boxRow + boxSize; r++)
            {
                for (int c = boxCol; c < boxCol + boxSize; c++)
                {
                    if (solution[r][c] == number)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                if (solution[row][col] == 0)
                {
                    var numbers = Enumerable.Range(1, Size).OrderBy(n => Guid.NewGuid()).ToList();
                    foreach (var number in numbers)
                    {
                        if (IsValidPlacement(number, row, col))
                        {
                            solution[row][col] = number;
                            if (FillBoard())
                            {
                                return true;
                            }
                            solution[row][col] = 0;
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }

    void RevealStarters()
    {
        var random = new Random();
        int totalCells = Size * Size;
        int cellsToReveal = (int)(totalCells * (hard == true ? 0.3 : hard == false ? 0.5 : 0.4));
        var allCells = Enumerable.Range(0, totalCells).OrderBy(_ => random.Next()).Take(cellsToReveal).ToList();

        for (int i = 0; i < totalCells; i++)
        {
            int row = i / Size;
            int col = i % Size;
            revealed[row][col] = allCells.Contains(i);
        }
    }

    void SetDifficulty(bool? difficulty) => hard = difficulty;
    void SetSize(int size) => Size = size;
    void Solve()
    {
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
                if (solution[row][col] == board[row][col])
                    revealed[row][col] = true;
        }
        if (revealed.All(r => r.All(c => c)))
            solved = true;
    }
}
