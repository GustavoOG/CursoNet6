using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoNet6.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre del Producto es Requerido")]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "Descripcion Corta es Requerido")]
        public string DescripcionCorta { get; set; }

        [Required(ErrorMessage = "Descripcion del Producto es Requerido")]
        public string DescripcionProducto { get; set; }

        [Required(ErrorMessage = "El Precio del Producto es Requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El Precio debe de ser Mayo a cero")]
        public double Precio { get; set; }

        public string? ImagenUrl { get; set; }


        //Foreign Key

        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }

        public int TipoAplicacionID { get; set; }

        [ForeignKey("TipoAplicacionID")]
        public virtual TipoAplicacion? TipoAplicacion { get; set; }
    }
}
