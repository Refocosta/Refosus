using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Refosus.Web.Data.Entities
{
    public class ShoppingEntity
    {
        [Display(Name = "Consecutivo")]
        public int Id { get; set; }

        [Display(Name = "Compañia")]
        public CompanyEntity Company { get; set; }

        [Display(Name = "Creado Por")]
        public UserEntity UserCreate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime CreateDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime CreateDateLocal => CreateDate.ToLocalTime();

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de actualización")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime UpdateDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de actualización")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss tt}", ApplyFormatInEditMode = false)]
        public DateTime UpdateDateLocal => UpdateDate.ToLocalTime();

        [Display(Name = "Asignado A")]
        public UserEntity UserAssigned { get; set; }

        [Display(Name = "Estado")]
        public ShoppingStateEntity State { get; set; }

        [Display(Name = "proyecto")]
        public ProjectEntity Project { get; set; }

        [Display(Name = "Jefe de Proyecto")]
        public UserEntity UserProjectBoss { get; set; }

        [Display(Name = "Articulos")]
        public IEnumerable<ShoppingItemsEntity> Items { get; set; }
        [Display(Name = "Articulos")]
        public IEnumerable<ShoppingTempItems> ItemsTemp { get; set; }

        [Display(Name = "Grupo Asignado")]
        public TP_GroupEntity AssignedGroup { get; set; }

        [Display(Name = "Grupo")]
        public TP_GroupEntity CreateGroup { get; set; }


        [Display(Name = "Valor Total")]
        public double TotalValue { get; set; }

        [Display(Name = "Observaciones")]
        public string observations { get; set; }

    }
}
