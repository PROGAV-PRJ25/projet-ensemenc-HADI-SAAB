// Inventaire.cs
using System;
using System.Collections.Generic;
using System.Linq;

public class Inventaire
{
    public List<Recolte> Recoltes { get; private set; } = new List<Recolte>();

    public void AjouterRecolte(Plante plante, Saison saison, Terrain terrain)
    {
        if (!plante.PeutRecolter()) return;

        double qualite = 0.7 + (plante.Sante / 200.0);
        
        if (plante.TerrainPrefere == terrain.Type)
            qualite *= 1.2;
        
        if (plante.Saisons.Contains(saison.ToString()))
            qualite *= 1.3;

        qualite = Math.Clamp(qualite, 0.5, 1.5);

        string typeProduit = plante is Comestible c ? c.Nature : "Plante";

        int quantite = (int)(plante.Productivite * (1 + qualite));
        
        Recoltes.Add(new Recolte(
            plante.Nom,
            typeProduit,
            quantite,
            qualite,
            saison
        ));

        plante.RecoltesRestantes--;
        if (plante.RecoltesRestantes <= 0)
            plante.EstMort = true;
    }

    public void VendreRecolte(int index, Joueur joueur)
    {
        if (index < 0 || index >= Recoltes.Count) return;

        var recolte = Recoltes[index];
        double prix = recolte.GetVenteBase() * recolte.Quantite;
        joueur.AjouterArgent(prix);
        Recoltes.RemoveAt(index);

        Console.WriteLine($"Vendu: {recolte.Quantite}x {recolte.NomPlante} " +
                         $"(Qualité: {recolte.Qualite * 100:F0}%) " +
                         $"pour {prix:F2}€");
    }

    public void Afficher(double argent)
    {
        Console.WriteLine("\n=== INVENTAIRE ===");
        Console.WriteLine($"Argent: {argent:F2}€");
        Console.WriteLine("Récoltes:");
        
        if (!Recoltes.Any())
        {
            Console.WriteLine("Aucune récolte disponible");
            return;
        }

        for (int i = 0; i < Recoltes.Count; i++)
        {
            var r = Recoltes[i];
            Console.WriteLine($"{i+1}. {r.Quantite}x {r.NomPlante} " +
                            $"(Saison: {r.SaisonRecolte}) " +
                            $"Valeur: {r.GetVenteBase()*r.Quantite*r.Qualite:F2}€");
        }
    }
}