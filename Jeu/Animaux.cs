public class Animaux
{
    public string Nom { get; protected set; }
    public double ProbabiliteApparition { get; protected set; }
    public int Degats { get; protected set; }
    public (int X, int Y)? Position { get; set; }

    private static List<Animaux> typesAnimaux = new List<Animaux>
    {
        new Animaux("🐇 Lapin", 0.3, 20),
        new Animaux("🐦 Pigeon", 0.2, 10),
        new Animaux("🦫 Taupe", 0.15, 30),
        new Animaux("🐛 Chenille", 0.25, 50)
    };

    public Animaux(string nom, double proba, int degats)
    {
        Nom = nom;
        ProbabiliteApparition = proba;
        Degats = degats;
        Position = null;
    }

    public static Animaux GenererAnimalAleatoire()
    {
        Random rnd = new Random();
        double tirage = rnd.NextDouble();
        double cumul = 0;

        foreach (var a in typesAnimaux)
        {
            cumul += a.ProbabiliteApparition;
            if (tirage <= cumul)
            {
                return new Animaux(a.Nom, a.ProbabiliteApparition, a.Degats);
            }
        }
        return null;
    }

    public void AttaquerPlante(Plante plante)
    {
        if (plante.EstProtegee)
        {
            Console.WriteLine("L’animal est repoussé ! La plante ne subit aucun dégât.");
            return;
        }

        if (plante.ProtectionPhysique)
        {
            Console.WriteLine("Un filet bloque l’animal. Il repart sans attaquer.");
            return;
        }
        plante.ModifierSnate(-Degats);
        Console.WriteLine($"{Nom} a attaqué {plante.Nom} et lui a infligé {Degats}% de dégâts !");
    }
}
