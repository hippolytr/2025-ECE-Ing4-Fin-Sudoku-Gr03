using Sudoku.Shared;
using System;
using System.Linq;

namespace Solution.NeuralNetworkSolvers
{
    public class NeuralNetworkSolver : ISudokuSolver
    {
        private ConvolutionLayer convLayer;
        private PoolingLayer poolLayer;
        private FullyConnectedLayer fcLayer;

        public NeuralNetworkSolver()
        {
            // Initialisation des différentes couches du réseau
            convLayer = new ConvolutionLayer(32, 3, 3, 1); // Par exemple, 32 filtres 3x3
            poolLayer = new PoolingLayer(2, 2); // Pooling 2x2
            fcLayer = new FullyConnectedLayer(128); // 128 neurones dans la couche fully connected
        }

        public SudokuGrid Solve(SudokuGrid sudoku)
        {
            // Prétraiter les données (convertir le Sudoku en format adapté au réseau)
            var inputData = PreprocessData(sudoku);

            // Propagation avant
            var convOutput = convLayer.Forward(inputData);
            var poolOutput = poolLayer.Forward(convOutput);
            var fcOutput = fcLayer.Forward(poolOutput);

            // Résultats du réseau
            var solvedSudoku = PostprocessOutput(fcOutput);

            return solvedSudoku;
        }

        // Prétraiter les données du Sudoku pour le réseau
        private double[] PreprocessData(SudokuGrid sudoku)
        {
            double[] inputData = new double[81];

            // Remplir le tableau inputData en fonction de la grille
            for (int i = 0; i < 81; i++)
            {
                int value = sudoku.Cells[i / 9, i % 9];

                // Cas de la case vide
                inputData[i] = (value == 0) ? 0.0 : value;
            }

            return inputData;
        }

        // Post-traiter la sortie du réseau pour récupérer la grille résolue
        private SudokuGrid PostprocessOutput(double[] output)
        {
            // Vérification de la taille de la sortie
            if (output == null || output.Length > 81)
                throw new InvalidOperationException($"Le tableau de sortie doit avoir exactement 81 éléments, mais la taille actuelle est {output?.Length ?? 0}.");

            var solvedSudoku = new SudokuGrid();

            // Remplir la grille de Sudoku avec les valeurs du tableau de sortie
            for (int i = 0; i < 81; i++)
            {
                int value = (int)Math.Round(output[i]);

                // Vérifier la validité de la valeur
                if (value < 1 || value > 9)
                    throw new InvalidOperationException($"Valeur invalide pour un Sudoku : {value} à l'indice {i}.");

                solvedSudoku.Cells[i / 9, i % 9] = value;
            }

            return solvedSudoku;
        }
    }

    // Exemple d'une couche convolutionnelle simplifiée
    public class ConvolutionLayer
    {
        private int numFilters;
        private int filterWidth;
        private int filterHeight;
        private int depth;

        public ConvolutionLayer(int numFilters, int filterWidth, int filterHeight, int depth)
        {
            this.numFilters = numFilters;
            this.filterWidth = filterWidth;
            this.filterHeight = filterHeight;
            this.depth = depth;
        }

        public double[] Forward(double[] input)
        {
            return input.Select(i => i * 0.5).ToArray(); // Placeholder pour la propagation avant
        }
    }

    // Exemple d'une couche de pooling simplifiée
    public class PoolingLayer
    {
        private int poolSize;
        private int stride;

        public PoolingLayer(int poolSize, int stride)
        {
            this.poolSize = poolSize;
            this.stride = stride;
        }

        public double[] Forward(double[] input)
        {
            return input.Where((x, i) => i % 2 == 0).ToArray(); // Placeholder pour la propagation avant
        }
    }

    // Exemple d'une couche fully connected simplifiée
    public class FullyConnectedLayer
    {
        private int numNeurons;

        public FullyConnectedLayer(int numNeurons)
        {
            this.numNeurons = numNeurons;
        }

        public double[] Forward(double[] input)
        {
            return input.Select(i => i * 2.0).ToArray(); // Placeholder pour la propagation avant
        }
    }
}
