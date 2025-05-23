public class NavigationManager
{
    // Référence au terrain sur lequel se déplace le curseur
    private Terrain terrain;
    
    // Constructeur qui initialise le NavigationManager avec un terrain donné
    public NavigationManager(Terrain terrain)
    {
        this.terrain = terrain;
    }
    
    // Méthode pour afficher le jardin et le curseur avec un message en bas
    public void AfficherAvecCurseur(string message)
    {
        Console.Clear();               // Efface l'écran console
        terrain.AfficherJardin();      // Affiche la grille du jardin (terrain)
        
        // Récupère la position actuelle de la sélection (curseur)
        var (x, y) = terrain.PositionSelection;
        
        // Positionne le curseur dans la console (x multiplié par 3 pour espacer)
        Console.SetCursorPosition(x * 3, y);
        
        // Affiche un "X" à la position du curseur
        Console.Write("  X");
        
        // Affiche le message passé en paramètre sous le jardin
        Console.WriteLine($"\n\n{message}");
        
        // Affiche les instructions de navigation
        Console.WriteLine("Flèches: Déplacer | Entrée: Valider | Échap: Annuler");
    }
    
    // Méthode pour gérer la navigation utilisateur via les touches du clavier
    public ConsoleKey Naviguer()
    {
        var key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.LeftArrow:
                terrain.DeplacerSelection(-1, 0);
                break;
            case ConsoleKey.RightArrow:
                terrain.DeplacerSelection(1, 0);
                break;
            case ConsoleKey.UpArrow:
                terrain.DeplacerSelection(0, -1);
                break;
            case ConsoleKey.DownArrow:
                terrain.DeplacerSelection(0, 1);
                break;
            case ConsoleKey.Enter:
            case ConsoleKey.Escape:
                return key; // renvoyer la touche qui stoppe la navigation
        }
        return ConsoleKey.NoName; // continuer sinon
    }

}
