public class Maladie
{
    public string Nom { get; protected set; }
    public int NiveauRisque { get; protected set;}
    public int Duree { get; protected set; }
    public double ProbabiliteApparition { get; protected set; } // entre 0.0 et 1.0

    public Maladie() { }

    public Maladie(string nom, int niveauRisque, int duree, double probApparition)
    {
        Nom = nom;
        NiveauRisque = niveauRisque;
        Duree = duree;
        ProbabiliteApparition = probApparition;
    }
   
}