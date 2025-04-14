public class Maladies
{
    public double Surface {get; set;}
   public List<string> Plantes {get; set; }
   public string Type {get; set;}
   public Maladies(double surface, List<string> plantes, string type)
   {
    Surface = surface;
    Plantes = plantes; 
    Type = type;
   }
   
}