using Sudoku.Shared;

namespace Solution.NorvigSolvers;

public class NorvigSolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        int[,] grid = s.Cells;
        if (SolveSudoku(grid))
        {
            SudokuGrid solution = s.CloneSudoku();
            solution.Cells = grid;
            return solution;
        }
        else
        {
            throw new Exception("Aucune solution trouvée !");
        }
    }

    private bool SolveSudoku(int[,] grid)
    {
        int row, col;
        if (!FindUnassignedLocation(grid, out row, out col))
        {
            return true; // Plus aucune case vide, solution trouvée
        }

        foreach (int num in GetPossibleValues(grid, row, col))
        {
            grid[row, col] = num;

            if (SolveSudoku(grid))
            {
                return true;
            }

            grid[row, col] = 0; // Backtracking si erreur
        }

        return false;
    }

    private bool FindUnassignedLocation(int[,] grid, out int row, out int col)
    {
        for (row = 0; row < 9; row++)
        {
            for (col = 0; col < 9; col++)
            {
                if (grid[row, col] == 0)
                {
                    return true;
                }
            }
        }
        row = col = -1;
        return false;
    }

    private List<int> GetPossibleValues(int[,] grid, int row, int col)
    {
        HashSet<int> possibleValues = new HashSet<int>(Enumerable.Range(1, 9));

        for (int i = 0; i < 9; i++)
        {
            possibleValues.Remove(grid[row, i]); // Vérifie la ligne
            possibleValues.Remove(grid[i, col]); // Vérifie la colonne
        }

        int boxRow = row / 3 * 3;
        int boxCol = col / 3 * 3;
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                possibleValues.Remove(grid[boxRow + r, boxCol + c]); // Vérifie le carré 3x3
            }
        }

        return possibleValues.ToList();
    }
}
