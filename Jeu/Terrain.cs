public abstract class Terrain
{
    public string Type { get; protected set; }
    public double Surface { get; set; }
    public List<Plante>? Plantes {get; protected set; }

   public Terrain(string type, double surface,  List<Plante> plantes)
   {
        Type = type;
        Surface = surface;
        Plantes = plantes; 
   }

   public void AjouterPlante(Plante plante)
   {
        Plantes.Add(plante);
   }
   
}