public class Maladies
{
    public int NiveauRisque {get; set;}
   public string Nom {get; set;}
   public Maladies(int niveauRisque, string nom)
   {
    Nom = nom;
    NiveauRisque = niveauRisque;
   }
   
}