public class SelectionPlante
{
    private int indexSelectionne = 0;
    private List<Plante> plantesDisponibles;

    public SelectionPlante(List<Plante> plantes)
    {
        plantesDisponibles = plantes;
    }

    public Plante AfficherEtChoisir()
    {
        ConsoleKeyInfo key;
        do
        {
            Console.Clear();
            Console.WriteLine("▼ Sélectionnez une plante avec les flèches ▼\n");

            for (int i = 0; i < plantesDisponibles.Count; i++)
            {
                if (i == indexSelectionne)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                var p = plantesDisponibles[i];
                Console.WriteLine($"{p.Nom} (Espace: {p.Espace}m², Saison: {string.Join(",", p.Saisons)})");
                Console.ResetColor();
            }

            Console.WriteLine("\nAppuyez sur [Entrée] pour valider, [Échap] pour annuler");

            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.DownArrow)
                indexSelectionne = Math.Min(indexSelectionne + 1, plantesDisponibles.Count - 1);
            else if (key.Key == ConsoleKey.UpArrow)
                indexSelectionne = Math.Max(indexSelectionne - 1, 0);

        } while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape);

        return key.Key == ConsoleKey.Enter ? plantesDisponibles[indexSelectionne] : null;
    }
}
