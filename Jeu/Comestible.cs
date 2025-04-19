public class Comestible : Plante
{
    public Comestible(string nom, string nature, List<string> saisons, string terrainPrefere, List<double> zonesTempPreferee, double espace, double vitesse, List<Maladie> maladies, int productivite, double besoinEau, double besoinLumineux) : base(nom, nature, saisons, terrainPrefere, zonesTempPreferee, espace, vitesse, maladies, productivite, besoinEau, besoinLumineux)
    {
        
    }
    public Comestible() : base("", "", new List<string>(), "", new List<double>(), 0, 0, new List<Maladie>(), 0, 0, 0)
    {
        // Nécessaire pour la désérialisation
    }

}