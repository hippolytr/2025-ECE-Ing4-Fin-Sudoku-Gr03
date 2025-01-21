from tensorflow.keras.models import load_model
import numpy as np

# Charger le modèle
modele = load_model("Sudoku/Modèles/Sudoku/Modèles/Final.h5.h5")

# caractéristiques du modèle sudoku_model_colab_1M_V2.h5 : val_accuracy: 0.8211 - val_loss: 0.4156 IL Y A DES FAUTES
# caractéristiques du modèle sudoku_model_colab_1M_Martial_V1.h5 : val_accuracy: 0.8145 - val_loss: 0.4139 IL Y A DES FAUTES
# caractéristiques du modèle A100_sudoku_model_V1.h5 : val_accuracy: 0.6381 - val_loss: 1.1762 IL Y A DES FAUTES
# caractéristiques du modèle A100_sudoku_model_9M_V2.h5 : val_accuracy: 0.7444 - val_loss: 0.7722 IL Y A DES FAUTES
# caractéristiques du modèle AA_sudoku_model_final.h5 : val_accuracy: 0.8297 - val_loss: 0.3796 IL Y A DES FAUTES

# caractéristiques du modèle sudoku_model_FINAL_PUNAISE.h5 : 
# 1M : val_accuracy: 0.6434 - val_loss: 0.9379
# 1M+9M : 

# caractéristiques du modèle Final.h5 : params: 11,360,985 (43.34 MB)
# 1M : val_accuracy: val_accuracy: 0.6659 - val_loss: 0.9293
# 1M+9M : val_accuracy: 0.6971 - val_loss: 0.8200

grille_facile = [
    [0, 0, 9, 0, 0, 2, 0, 0, 5],
    [5, 3, 8, 0, 6, 4, 0, 0, 9],
    [1, 6, 2, 0, 0, 0, 0, 3, 0],
    [0, 0, 3, 0, 2, 7, 0, 0, 0],
    [0, 5, 4, 6, 0, 0, 1, 0, 0],
    [0, 0, 7, 0, 1, 5, 3, 4, 0],
    [3, 0, 0, 8, 0, 1, 9, 0, 6],
    [7, 0, 0, 3, 0, 0, 8, 5, 0],
    [0, 9, 1, 0, 0, 0, 4, 7, 0],
]

grille_difficile = [
    [5, 6, 0, 7, 4, 2, 9, 0, 1],
    [0, 4, 0, 0, 0, 0, 0, 7, 8],
    [0, 0, 0, 3, 6, 0, 2, 0, 0],
    [0, 0, 4, 0, 5, 0, 1, 0, 6],
    [0, 0, 0, 2, 0, 0, 4, 9, 0],
    [0, 0, 9, 0, 0, 0, 5, 0, 0],
    [9, 0, 6, 0, 0, 0, 0, 0, 4],
    [8, 0, 0, 9, 0, 0, 7, 0, 0],
    [0, 0, 3, 0, 0, 5, 0, 0, 0],
]

grille_EXTREME = [
    [0, 0, 0, 0, 0, 6, 0, 0, 5],
    [0, 0, 9, 0, 5, 8, 0, 0, 0],
    [0, 0, 1, 0, 0, 0, 9, 2, 0],
    [0, 0, 0, 0, 0, 0, 2, 0, 0],
    [7, 4, 0, 0, 0, 0, 3, 0, 0],
    [0, 3, 0, 0, 0, 0, 0, 0, 7],
    [9, 0, 0, 0, 1, 3, 0, 7, 0],
    [2, 5, 0, 0, 9, 0, 8, 0, 0],
    [0, 0, 0, 0, 0, 0, 0, 4, 0],
]

grille_facile = np.array(grille_facile).reshape(1, 9, 9, 1)
#13 fautes pour la grille facile

# Faire la prédiction
prediction = modele.predict(grille_facile)

# Convertir les probabilités en valeurs (0 à 8) et ajouter 1 pour obtenir les chiffres (1 à 9)
solution = np.argmax(prediction, axis=-1).reshape(9, 9) + 1

print("Grille résolue :")
print(solution)