public class Plante 
{
    public string Nom { get; protected set; }
    public string Nature { get; protected set; }
    public List<string> Saisons { get; protected set; } 
    public string TerrainPrefere { get; protected set; } 
    public List<double> ZonesTempPreferee { get; protected set; }
    public double Espace { get; protected set; }
    public double VitesseCroissance { get; protected set; }
    public List<Maladie> MaladiesPossibles { get; protected set; }
    public int Productivite { get; protected set; }
    public double BesoinEau { get; protected set; }  // entre 0.0 et 1.0
    public double BesoinLumineux { get; protected set; } // entre 0.0 et 1.0
    public int Age { get; protected set; }
    public int Sante { get; protected set; }
    public bool EstMort { get; protected set; }



    public Plante(string nom, string nature, List<string> saisons, string terrainPrefere, List<double> zonesTempPreferee, double espace, double vitesse, List<Maladie> maladies, int productivite, double besoinEau, double besoinLumineux)
    {
        Nom = nom;
        Nature = nature;
        Saisons = saisons;
        TerrainPrefere = terrainPrefere;
        ZonesTempPreferee = zonesTempPreferee;
        Espace = espace;
        VitesseCroissance = vitesse;
        MaladiesPossibles = maladies;
        Productivite = productivite;
        BesoinEau = besoinEau;
        BesoinLumineux = besoinLumineux;
        Age = 0;
        Sante = 100;
        EstMort = false;
    }

    public void ConditionsPalnte(Meteo meteo, Terrain terrain)
    {
        int conditionOk = 0;
        if (meteo.Temperature >= ZonesTempPreferee.Min() && meteo.Temperature <= ZonesTempPreferee.Max())
        {
            conditionOk++;
        }
        if (terrain.Type == TerrainPrefere)
        {
            conditionOk++;
        }
        if (meteo.Pluie >= BesoinEau)
        {
            conditionOk++;
        }
        if (meteo.Soleil >= BesoinLumineux)
        {
            conditionOk++;
        }
        conditionOk = conditionOk*25;
        if (conditionOk < 50)
        {
            EstMort = true;
            Sante = 0;
            Console.WriteLine("La plante est morte ! Les conditions ne sont pas res à 50 %");
            return;
        }

        Sante = Math.Min(Sante + 10, 100);
        VitesseCroissance += conditionOk / 100.0;

        Console.WriteLine($"La plante {Nom} a poussé. Santé : {Sante}%, Croissance : {VitesseCroissance}");

    }

  

}