public class Recolte
{
    // Nom de la plante récoltée
    public string NomPlante { get; }
    
    // Type de produit récolté (ex : "Fruit", "Légume", "Fleur", etc.)
    public string TypeProduit { get; }
    
    // Quantité récoltée
    public int Quantite { get; }
    
    // Qualité de la récolte, valeur décimale influençant le prix
    public double Qualite { get; }
    
    // Saison durant laquelle la récolte a été faite
    public Saison SaisonRecolte { get; }
    
    // Constructeur pour initialiser tous les champs de la récolte
    public Recolte(string nom, string type, int quantite, double qualite, Saison saison)
    {
        NomPlante = nom;
        TypeProduit = type;
        Quantite = quantite;
        Qualite = qualite;
        SaisonRecolte = saison;
    }
    
    // Méthode pour obtenir le prix de base de vente d'un produit, ajusté selon la qualité
    public double GetVenteBase()
    {
        return TypeProduit switch {
            "Fruit" => 3.5,    // Prix de base pour les fruits
            "Légume" => 2.5,   // Prix de base pour les légumes
            "Fleur" => 4.0,    // Prix de base pour les fleurs
            _ => 1.5           // Prix de base pour les autres types
        } * (1 + Qualite);     // Application d'un bonus selon la qualité
    }
}
