public class Terrain
{
    public double Surface {get; set;}
   public List<string> Plantes {get; set; }
   public string Type {get; set;}
   public Terrain(double surface, List<string> plantes, string type)
   {
    Surface = surface;
    Plantes = plantes; 
    Type = type;
   }
   
}