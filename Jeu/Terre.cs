public class Terre : Terrain
{
    public Terre(double surface,  List<Plante> plantes) : base("Terre", surface, plantes)
    {

    }
    public Terre() : base("Terre", 0, new List<Plante>()) { }
}