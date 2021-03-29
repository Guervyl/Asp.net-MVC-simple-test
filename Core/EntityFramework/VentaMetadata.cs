using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EntityFramework
{
    [MetadataType(typeof(VentaMetadata))]
    partial class Venta { }

    public class VentaMetadata
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:s}")]
        public System.DateTime fecha { get; set; }
    }
}
