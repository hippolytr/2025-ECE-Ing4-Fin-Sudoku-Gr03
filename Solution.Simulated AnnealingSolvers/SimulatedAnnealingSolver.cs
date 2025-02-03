using Sudoku.Shared;

namespace Solution.Simulated_AnnealingSolvers;

public class SimulatedAnnealingSolver : ISudokuSolver
{
    private const double InitialTemperature = 1000.0;
    private const double FinalTemperature = 0.1;
    private const double CoolingRate = 0.995;

    private static readonly Random rand = new Random();

    public SudokuGrid Solve(SudokuGrid s)
    {
        var currentGrid = CopyGrid(s);
        var currentScore = CalculateScore(currentGrid);
        var temperature = InitialTemperature;

        while (temperature > FinalTemperature)
        {
            var newGrid = GenerateNeighbour(currentGrid, s);
            var newScore = CalculateScore(newGrid);

            if (AcceptSolution(currentScore, newScore, temperature))
            {
                currentGrid = CopyGrid(newGrid);
                currentScore = newScore;

                if (currentScore == 0)
                    break;
            }

            temperature *= CoolingRate;
        }

        return currentGrid;
    }

    private SudokuGrid CopyGrid(SudokuGrid grid)
    {
        SudokuGrid newGrid = new SudokuGrid();
        newGrid.Cells = (int[,])grid.Cells.Clone(); // Copie correcte du tableau
        return newGrid;
    }

    private int CalculateScore(SudokuGrid grid)
    {
        int score = 0;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int value = grid.Cells[i, j];
                if (value == 0) continue;

                // Vérifier la ligne
                for (int k = 0; k < 9; k++)
                {
                    if (k != j && grid.Cells[i, k] == value)
                        score++;
                }

                // Vérifier la colonne
                for (int k = 0; k < 9; k++)
                {
                    if (k != i && grid.Cells[k, j] == value)
                        score++;
                }

                // Vérifier la sous-grille 3x3
                int subgridRow = (i / 3) * 3;
                int subgridCol = (j / 3) * 3;
                for (int m = subgridRow; m < subgridRow + 3; m++)
                {
                    for (int n = subgridCol; n < subgridCol + 3; n++)
                    {
                        if ((m != i || n != j) && grid.Cells[m, n] == value)
                            score++;
                    }
                }
            }
        }

        return score;
    }

    private SudokuGrid GenerateNeighbour(SudokuGrid grid, SudokuGrid originalGrid)
    {
        var newGrid = CopyGrid(grid);
        int row, col, newValue;

        do
        {
            row = rand.Next(0, 9);
            col = rand.Next(0, 9);
        } while (originalGrid.Cells[row, col] != 0);

        newValue = rand.Next(1, 10);
        newGrid.Cells[row, col] = newValue;

        return newGrid;
    }

    private bool AcceptSolution(int currentScore, int newScore, double temperature)
    {
        if (newScore < currentScore) return true;

        double probability = Math.Exp((currentScore - newScore) / temperature);
        return rand.NextDouble() < probability;
    }
}
