public class Recolte
{
    public string NomPlante { get; }
    public string TypeProduit { get; }
    public int Quantite { get; }
    public double Qualite { get; }
    public Saison SaisonRecolte { get; }
    
    public Recolte(string nom, string type, int quantite, double qualite, Saison saison)
    {
        NomPlante = nom;
        TypeProduit = type;
        Quantite = quantite;
        Qualite = qualite;
        SaisonRecolte = saison;
    }
    
    public double GetVenteBase()
    {
        return TypeProduit switch {
            "Fruit" => 3.5,    // Augmentation des prix
            "Légume" => 2.5,
            "Fleur" => 4.0,
            _ => 1.5
        } * (1 + Qualite); // Bonus de qualité
    }
}