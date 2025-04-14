public class Plantes 
{
    protected string Nom;
    protected string Nature;
    protected string Saisons; 
    protected string TerrainPrefere; 
    protected string  ZonesTempPreferee;
    protected double Espace;
    protected double VitessCroissance;
    protected int Productivite; 
    protected double BesoinEau;
    protected double BesoinLuministe;


    public Plantes(string nom, string saisons, string nature, string terrainPrefere, string zonesTempPreferee,
    double espace, int productivite, double besoinEau, double besoinLuministe )
    {
        Nom = nom;
        Saisons = saisons;
        Nature = nature;
        TerrainPrefere = terrainPrefere;
        ZonesTempPreferee = zonesTempPreferee;
        Espace = espace; 
        Productivite = productivite;
        BesoinEau = besoinEau;
        BesoinLuministe = besoinLuministe; 
    }



}