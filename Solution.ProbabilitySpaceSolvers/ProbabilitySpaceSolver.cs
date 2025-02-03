using Sudoku.Shared;
using System;

namespace Solution.ProbabilitySpaceSolvers
{
    public class ProbabilitySpaceSolver : ISudokuSolver
    {
        private const int GridSize = 9; // Taille de la grille 9x9
        private const int NumValues = 9; // Valeurs possibles (1 à 9)

        public SudokuGrid Solve(SudokuGrid sudoku)
        {
            // Représenter les variables aléatoires pour chaque cellule de la grille
            var variables = new double[GridSize, GridSize, NumValues];

            // Initialiser les probabilités
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    if (sudoku.Cells[row, col] != 0)
                    {
                        // Probabilité de la valeur de la cellule déjà remplie = 1 pour la valeur correspondante
                        variables[row, col, sudoku.Cells[row, col] - 1] = 1.0;
                    }
                    else
                    {
                        // Distribution uniforme pour les cases vides
                        for (int value = 0; value < NumValues; value++)
                        {
                            variables[row, col, value] = 1.0 / NumValues;
                        }
                    }
                }
            }

            // Appliquer les contraintes : lignes, colonnes, régions 3x3
            ApplyConstraints(variables);

            // Inférence : itération de propagation de messages
            IterateInference(variables);

            // Convertir les variables en solution de Sudoku
            return PostprocessOutput(variables);
        }

        // Appliquer les contraintes sur les lignes, colonnes et régions
        private void ApplyConstraints(double[,,] variables)
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    // Empêcher les duplications sur la même ligne
                    for (int i = 0; i < GridSize; i++)
                    {
                        if (i != col)
                        {
                            for (int value = 0; value < NumValues; value++)
                            {
                                variables[row, col, value] *= (1 - variables[row, i, value]);
                            }
                        }
                    }

                    // Empêcher les duplications sur la même colonne
                    for (int i = 0; i < GridSize; i++)
                    {
                        if (i != row)
                        {
                            for (int value = 0; value < NumValues; value++)
                            {
                                variables[row, col, value] *= (1 - variables[i, col, value]);
                            }
                        }
                    }

                    // Empêcher les duplications dans la même région 3x3
                    int startRow = (row / 3) * 3;
                    int startCol = (col / 3) * 3;
                    for (int i = startRow; i < startRow + 3; i++)
                    {
                        for (int j = startCol; j < startCol + 3; j++)
                        {
                            if (i != row || j != col)
                            {
                                for (int value = 0; value < NumValues; value++)
                                {
                                    variables[row, col, value] *= (1 - variables[i, j, value]);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Effectuer l'inférence itérative
        private void IterateInference(double[,,] variables)
        {
            for (int iteration = 0; iteration < 100; iteration++) // Limité à 100 itérations
            {
                for (int row = 0; row < GridSize; row++)
                {
                    for (int col = 0; col < GridSize; col++)
                    {
                        double sum = 0.0;
                        for (int value = 0; value < NumValues; value++)
                        {
                            sum += variables[row, col, value];
                        }

                        // Normaliser les probabilités pour chaque cellule
                        for (int value = 0; value < NumValues; value++)
                        {
                            variables[row, col, value] /= sum;
                        }
                    }
                }
            }
        }

        // Convertir les probabilités en une solution de Sudoku
        private SudokuGrid PostprocessOutput(double[,,] variables)
        {
            var solvedSudoku = new SudokuGrid();

            // Choisir la valeur ayant la probabilité la plus élevée pour chaque cellule
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    int bestValue = -1;
                    double bestProb = -1;

                    for (int value = 0; value < NumValues; value++)
                    {
                        if (variables[row, col, value] > bestProb)
                        {
                            bestProb = variables[row, col, value];
                            bestValue = value + 1;
                        }
                    }

                    solvedSudoku.Cells[row, col] = bestValue;
                }
            }

            return solvedSudoku;
        }
    }
}
