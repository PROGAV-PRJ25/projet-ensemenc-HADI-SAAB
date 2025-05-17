public class NavigationManager
{
    private Terrain terrain;
    
    public NavigationManager(Terrain terrain)
    {
        this.terrain = terrain;
    }
    
    public void AfficherAvecCurseur(string message)
    {
        Console.Clear();
        terrain.AfficherJardin();
        
        // Afficher le curseur
        var (x, y) = terrain.PositionSelection;
        Console.SetCursorPosition(x * 3, y);
        Console.Write("X");
        
        Console.WriteLine($"\n\n{message}");
        Console.WriteLine("Flèches: Déplacer | Entrée: Valider | Échap: Annuler");
    }
    
    public bool Naviguer()
    {
        var key = Console.ReadKey(true);
        
        switch (key.Key)
        {
            case ConsoleKey.LeftArrow: terrain.DeplacerSelection(-1, 0); return true;
            case ConsoleKey.RightArrow: terrain.DeplacerSelection(1, 0); return true;
            case ConsoleKey.UpArrow: terrain.DeplacerSelection(0, -1); return true;
            case ConsoleKey.DownArrow: terrain.DeplacerSelection(0, 1); return true;
            case ConsoleKey.Enter: return false;
            case ConsoleKey.Escape: return false;
            default: return true;
        }
    }
}