public class Fleur : Plante
{
    public Fleur(string nom, string nature, List<string> saisons, string terrainPrefere, (double Min, double Max) zonesTemp, double espace, double vitesse, List<Maladie> maladies, int productivite, double besoinEau, double besoinLumineux, double tailleMax)
        : base(nom, nature, saisons, terrainPrefere, zonesTemp, espace, vitesse, maladies, productivite, besoinEau, besoinLumineux, tailleMax)
    {
    }

    public override Plante Clone()
    {
        return new Fleur(Nom, Nature, new List<string>(Saisons), TerrainPrefere, ZonesTempPreferee,
                         Espace, VitesseCroissance, new List<Maladie>(MaladiesPossibles), Productivite, BesoinEau, BesoinLumineux, TailleMax);
    }
}
