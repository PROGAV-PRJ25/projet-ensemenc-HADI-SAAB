public class Animaux
{
    // Liste globale de noms d'animaux pour la génération
    private static List<string> ListeNomAnimaux = new List<string>
    {
        "escargots", "rongeurs", "oiseaux", "lagomorphes", "talpinae", "piétineurs", "Serpent"
    };

    public string NomAnimal { get; protected set; }
    public double NiveauDeRisqueAnimal { get; protected set; }
    public double NiveauDeRisqueAnimalMax { get; protected set; }

  
    public Animaux(string nomAnimal, double niveauDeRisqueAnimal)
    {
        NomAnimal = nomAnimal;
        NiveauDeRisqueAnimal = niveauDeRisqueAnimal;
        NiveauDeRisqueAnimalMax = 9.9;
    }

    public static Animaux GenererAnimaux()
    {
        Random rng = new Random();

        // Choisir un nom aléatoire dans la liste
        int index = rng.Next(ListeNomAnimaux.Count);
        string nomAleatoire = ListeNomAnimaux[index];

        // Générer un niveau de risque aléatoire entre 1 et 9.9
        double niveauRisque = Math.Round(rng.NextDouble() * 8.9 + 1, 1);

        return new Animaux(nomAleatoire, niveauRisque);
    }
}
