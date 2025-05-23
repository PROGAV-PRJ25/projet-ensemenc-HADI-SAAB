public class SelectionPlante
{
    // Index de la plante actuellement sélectionnée dans la liste
    private int indexSelectionne = 0;
    
    // Liste des plantes disponibles pour la sélection
    private List<Plante> plantesDisponibles;

    // Constructeur qui initialise la liste des plantes disponibles
    public SelectionPlante(List<Plante> plantes)
    {
        plantesDisponibles = plantes;
    }

    // Méthode pour afficher la liste des plantes et permettre à l'utilisateur d'en choisir une
    public Plante AfficherEtChoisir()
    {
        ConsoleKeyInfo key;
        do
        {
            Console.Clear(); // Efface l'écran pour actualiser l'affichage
            Console.WriteLine("▼ Sélectionnez une plante avec les flèches ▼\n");

            // Parcours de toutes les plantes disponibles
            for (int i = 0; i < plantesDisponibles.Count; i++)
            {
                // Met en surbrillance la plante sélectionnée
                if (i == indexSelectionne)
                {
                    Console.BackgroundColor = ConsoleColor.White; // Fond blanc
                    Console.ForegroundColor = ConsoleColor.Black; // Texte noir
                }

                var p = plantesDisponibles[i];
                // Affiche le nom de la plante, son espace requis, et ses saisons
                Console.WriteLine($"{p.Nom} (Espace: {p.Espace}m², Saison: {string.Join(",", p.Saisons)})");
                
                // Réinitialise les couleurs pour les autres lignes
                Console.ResetColor();
            }

            Console.WriteLine("\nAppuyez sur [Entrée] pour valider, [Échap] pour annuler");

            // Lecture d'une touche utilisateur en mode silencieux
            key = Console.ReadKey(true);
            
            // Mise à jour de l'index sélectionné en fonction des flèches haut/bas
            if (key.Key == ConsoleKey.DownArrow)
                indexSelectionne = Math.Min(indexSelectionne + 1, plantesDisponibles.Count - 1);
            else if (key.Key == ConsoleKey.UpArrow)
                indexSelectionne = Math.Max(indexSelectionne - 1, 0);

        // Continue la boucle tant que l'utilisateur n'a pas appuyé sur Entrée ou Échap
        } while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape);

        // Si Entrée est pressé, retourne la plante sélectionnée, sinon retourne null (annulation)
        return key.Key == ConsoleKey.Enter ? plantesDisponibles[indexSelectionne] : null;
    }
}
