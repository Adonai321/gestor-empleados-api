namespace MiWebAPI.Models
{
    public class Pelicula
    {
        public long IdPelicula { get; set; }
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string Director {  get; set; }
        public string Genero { get; set; }
        public int AñoEstreno { get; set; }
        public decimal DuracionMinutos { get; set; } 
        public bool Activo { get; set; }
    }
}
