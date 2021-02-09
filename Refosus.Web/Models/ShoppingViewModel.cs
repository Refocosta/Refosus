using Microsoft.AspNetCore.Mvc.Rendering;
using Refosus.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Models
{
    public class ShoppingViewModel : ShoppingEntity
    {
        [DisplayName("Usuario Solicitante")]
        public string IdUserCreate { get; set; }

        [DisplayName("Usuario Asignado")]
        public string IdUserAssign { get; set; }

        [DisplayName("Estado")]
        public int IdState { get; set; }

        [DisplayName("Proyecto")]
        public int IdProject { get; set; }

        [DisplayName("Jefe del Projecto")]
        public string IdUserProjectBoss { get; set; }

        [DisplayName("Departamento")]
        public int IdGroupCreate { get; set; }

        [DisplayName("Departamento Asignado")]
        public int IdGroupAssigned { get; set; }




        [DisplayName("Codigo SAP")]
        public string CodSap { get; set; }


        [DisplayName("Categoria")]
        public int IdCategory { get; set; }

        [DisplayName("Sub Categoria")]
        public int IdSubCategory { get; set; }

        [DisplayName("Unidades")]
        public int IdUnit { get; set; }

        [DisplayName("Medida")]
        public int IdMeasure { get; set; }

        [DisplayName("Cantidad")]
        public int Quantity { get; set; }

        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("Referencia")]
        public string Reference { get; set; }

        [DisplayName("Marca")]
        public string Mark { get; set; }

        [DisplayName("Orden Interna")]
        public string InternalOrder { get; set; }

        [DisplayName("Numero de Orden Interna")]
        public string NumInternalOrder { get; set; }











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



        [DisplayName("Operación")]
        public int Operation { get; set; }
        [DisplayName("DeleteItem")]
        public int DeleteItem { get; set; }
    }
}
