public class Animaux //pour le mode urgence
{
    public List<string> NomAnimal { get; protected set; }
    public double NiveauDeRisqueAnimal {get; protected set;}
    public double niveauDeRisqueAnimalMax {get; protected set; } // entre 1 et 10
    public Animaux ( string nomAnimal, double niveauDeRisqueAnimal )
    {
        NomAnimal = nomAnimal;
        NiveauDeRisqueAnimal  = niveauDeRisqueAnimal;
        niveauDeRisqueAnimalMax= 9.9; 
    }
}