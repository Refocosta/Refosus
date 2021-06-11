using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class ShoppingViewModel : ShoppingEntity
    {
        [DisplayName("Compañia")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int IdCompany { get; set; }
        [DisplayName("Usuario Solicitante")]
        public string IdUserCreate { get; set; }

        [DisplayName("Usuario Asignado")]
        public string IdUserAssign { get; set; }

        [DisplayName("Estado")]
        public int IdState { get; set; }

        [DisplayName("Proyecto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int IdProject { get; set; }

        [DisplayName("Jefe del Projecto")]
        public string IdUserProjectBoss { get; set; }

        [DisplayName("Departamento")]
        public int IdGroupCreate { get; set; }

        [DisplayName("Departamento Asignado")]
        public int IdGroupAssigned { get; set; }



































        #region Operativas
        public int Operation { get; set; }
        public int DeleteItem { get; set; }
        public int DeleteItemTemp { get; set; }
        public int EditTempItem { get; set; }
        public int EditItem { get; set; }
        #endregion
        #region Valores de Modal
        [DisplayName("Codigo SAP")]
        public string ItemCodSap { get; set; }

        [DisplayName("Descripción")]
        public string ItemDescription { get; set; }

        [DisplayName("Categoria")]
        public int ItemIdCategory { get; set; }
        [DisplayName("Sub Categoria")]
        public int ItemIdSubCategory { get; set; }
        [DisplayName("Unidades")]
        public int ItemIdUnit { get; set; }

        [DisplayName("Medida")]
        public int ItemIdMeasure { get; set; }

        [DisplayName("Cantidad")]
        public int ItemQuantity { get; set; }
        [DisplayName("Referencia")]
        public string ItemReference { get; set; }

        [DisplayName("Marca")]
        public string ItemMark { get; set; }

        [DisplayName("Orden Interna")]
        public string ItemInternalOrder { get; set; }

        [DisplayName("Numero de Orden Interna")]
        public string ItemNumInternalOrder { get; set; }

        [DisplayName("Observacion")]
        public string ItemObservation { get; set; }

        [DisplayName("Valor Unidad")]
        public decimal ItemUnitCost { get; set; }
        [DisplayName("Valor Total")]
        public decimal ItemFullCost { get; set; }
        [DisplayName("Cantidad Entregada")]
        public int ItemQuantityDelivered { get; set; }

        [DisplayName("Proveedores")]
        public List<TP_Shopping_ItemProvedorEntity> ItemProveedores { get; set; }
        [DisplayName("Nombre")]
        public string ProoNombre { get; set; }
        [DisplayName("Telefono")]
        public string ProoTelefono { get; set; }
        [DisplayName("Cantidad")]
        public int ProoCantidad { get; set; }
        [DisplayName("Valor Unidad")]
        public int ProoValorUnidad { get; set; }
        [DisplayName("Adjunto")]
        public IEnumerable<IFormFile> File { get; set; }
        public int Pantalla { get; set; }
        #endregion
        #region Listas
        [DisplayName("Usuarios")]
        public IEnumerable<SelectListItem> Users { get; set; }

        [DisplayName("Unidades")]
        public IEnumerable<SelectListItem> ShoppingUnits { get; set; }
        [DisplayName("Medidas")]
        public IEnumerable<SelectListItem> ShoppingMeasures { get; set; }
        [DisplayName("Estados")]
        public IEnumerable<SelectListItem> ShoppingStates { get; set; }
        [DisplayName("Proyectos")]
        public IEnumerable<SelectListItem> Projects { get; set; }
        [DisplayName("Categorias")]
        public IEnumerable<SelectListItem> Categories { get; set; }
        [DisplayName("Sub Categorias")]
        public IEnumerable<SelectListItem> SubCategories { get; set; }
        [DisplayName("Departamentos")]
        public IEnumerable<SelectListItem> Groups { get; set; }

        [DisplayName("Compañias")]
        public IEnumerable<SelectListItem> Companies { get; set; }
        #endregion

    }
}
