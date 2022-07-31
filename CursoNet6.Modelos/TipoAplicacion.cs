using System.ComponentModel.DataAnnotations;

namespace CursoNet6.Modelos
{
    public class TipoAplicacion
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "El Nombre del Tipo de Aplicacion es Obligatorio.")]
        public string Nombre { get; set; }
    }
}
