public class Maladie
{
    public string Nom { get; protected set; }
    public int NiveauRisque { get; protected set;}
    public int Duree { get; protected set; }


    public Maladie(string nom, int niveauRisque, int duree)
    {
        Nom = nom;
        NiveauRisque = niveauRisque;
        Duree = duree;
    }
   
}