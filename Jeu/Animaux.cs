public class Animaux
{
    // Nom de l'animal (protégé en écriture)
    public string Nom { get; protected set; }
    
    // Probabilité d'apparition de l'animal (protégée en écriture)
    public double ProbabiliteApparition { get; protected set; }
    
    // Dégâts que l'animal peut infliger
    public int Degats { get; set; }
    
    // Position actuelle de l'animal sur le terrain (nullable)
    public (int X, int Y)? Position { get; set; }
    
    // Générateur de nombres aléatoires
    public Random rng = new Random();

    // Liste statique des types d'animaux possibles avec leurs caractéristiques
    private static List<Animaux> typesAnimaux = new List<Animaux>
    {
        new Animaux("🐇 Lapin", 0.3, 20),
        new Animaux("🐦 Pigeon", 0.2, 10),
        new Animaux("🦫 Taupe", 0.15, 30),
        new Animaux("🐛 Chenille", 0.25, 50)
    };

    // Constructeur pour créer un animal avec son nom, sa probabilité et ses dégâts
    public Animaux(string nom, double proba, int degats)
    {
        Nom = nom;
        ProbabiliteApparition = proba;
        Degats = degats;
        Position = null; // Position initiale nulle (non placée)
    }

    // Méthode statique pour générer un animal aléatoire en fonction des probabilités
    public static Animaux GenererAnimalAleatoire()
    {
        Random rnd = new Random();
        double tirage = rnd.NextDouble(); // Tirage aléatoire entre 0 et 1
        double cumul = 0;

        // Parcours de la liste des animaux pour trouver celui correspondant au tirage
        foreach (var a in typesAnimaux)
        {
            cumul += a.ProbabiliteApparition;
            if (tirage <= cumul)
            {
                // Retourne un nouvel animal basé sur celui trouvé
                return new Animaux(a.Nom, a.ProbabiliteApparition, a.Degats);
            }
        }
        return null; // Pas d'animal trouvé (cas théorique)
    }

    // Méthode pour qu'un animal attaque une plante donnée
    public void AttaquerPlante(Plante plante)
    {
        // Si la plante est protégée, l'attaque est repoussée
        if (plante.EstProtegee)
        {
            Console.WriteLine("L’animal est repoussé ! La plante ne subit aucun dégât.");
            return;
        }

        // Si la plante possède une protection physique (filet), l'attaque échoue
        if (plante.ProtectionPhysique)
        {
            Console.WriteLine("Un filet bloque l’animal. Il repart sans attaquer.");
            return;
        }
        
        // La plante subit des dégâts
        plante.ModifierSante(-Degats);
        Console.WriteLine($"{Nom} a attaqué {plante.Nom} et lui a infligé {Degats}% de dégâts !");
    }

    // Méthode pour déplacer l'animal sur le terrain
    public void SeDeplacer(Terrain terrain)
    {
        if (Position == null) return; // Si position non définie, on ne bouge pas
        
        var (x, y) = Position.Value;
        // Calcul de la nouvelle position avec déplacement aléatoire (-1, 0 ou +1)
        int newX = x + rng.Next(-1, 2);
        int newY = y + rng.Next(-1, 2);
        
        // Limiter les nouvelles coordonnées aux dimensions du terrain (0 à 9)
        newX = Math.Clamp(newX, 0, 9);
        newY = Math.Clamp(newY, 0, 9);
        
        // Mise à jour de la position de l'animal
        Position = (newX, newY);
        
        // Si une plante est présente sur la nouvelle case et qu'elle n'est pas morte, attaquer
        var plante = terrain.GrillePlantes[(newX, newY)];
        if (plante != null && !plante.EstMort)
        {
            AttaquerPlante(plante);
        }
    }

}
