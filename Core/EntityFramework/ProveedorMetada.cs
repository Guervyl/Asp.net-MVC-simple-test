using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EntityFramework
{
    [MetadataType(typeof(proveedorMetada))]
    partial class proveedor { }

    public class proveedorMetada
    {
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Por favor el nombre del proveedor debe ser entr 2 y 15 caracteres.")]
        [Required]
        public string nombre { get; set; }
    }
}
