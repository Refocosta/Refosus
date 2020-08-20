﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace Refosus.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboCeCo()
        {
            List<SelectListItem> list = _context.CeCos.Select(t =>

              new SelectListItem
              {
                  Text = t.Code + "-" + t.Name,
                  Value = $"{t.Id}"
              })
                .OrderBy(t => t.Text)
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Centro de Costos]",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboCampus()
        {
            List<SelectListItem> list = _context.Campus.Where(t => t.IsActive == true).Select(t =>

              new SelectListItem
              {
                  Text = t.Name,
                  Value = $"{t.Id}"

              })
                .OrderBy(t => t.Text)
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Selecciones una sede]",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboCompany()
        {
            List<SelectListItem> list = _context.Companies.Where(t => t.IsActive == true).Select(t =>

              new SelectListItem
              {
                  Text = t.Name,
                  Value = $"{t.Id}"
              })
                .OrderBy(t => t.Text)
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una Compañia]",
                Value = "0"
            });
            return list;
        }
        public IEnumerable<SelectListItem> GetComboMenus()
        {
            List<SelectListItem> list = _context.Menus
                .Where(t => t.Hidden == false)
                .Select(t =>

              new SelectListItem
              {
                  Text = t.Name,
                  Value = $"{t.Id}"
              })
                .OrderBy(t => t.Text)
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Menu]",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboMessageType()
        {
            List<SelectListItem> list = _context.MessagesTypes.Select(t =>

              new SelectListItem
              {
                  Text = t.Name,
                  Value = $"{t.Id}"
              })
                .OrderBy(t => t.Text)
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Tipo]",
                Value = "0"
            });
            return list;
        }
        public IEnumerable<SelectListItem> GetComboMessageState()
        {
            List<SelectListItem> list = _context.MessagesStates
                .Where(t => t.Active == true)
                .Select(t =>

              new SelectListItem
              {
                  Text = t.Name,
                  Value = $"{t.Id}"
              })
                .OrderBy(t => t.Text)
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Estado]",
                Value = "0"
            });
            return list;
        }
        public IEnumerable<SelectListItem> GetComboMessageBillState()
        {
            List<SelectListItem> list = _context.MessagesBillState
                .Where(t => t.Active == true)
                .Select(t =>
              new SelectListItem
              {
                  Text = t.Name,
                  Value = $"{t.Id}"
              })
                .OrderBy(t => t.Text)
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Estado]",
                Value = "0"
            });
            return list;
        }
        public IEnumerable<SelectListItem> GetComboUser()
        {
            List <SelectListItem> list = _context.Users
                .OrderBy(t=>t.FirstName)
                .Select(t =>
              new SelectListItem
              {
                  Text = t.FullName,
                  Value = $"{t.Id}"
              })
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Usuario]",
                Value = "0"
            });
            return list;
        }
        public IEnumerable<SelectListItem> GetDocumentType()
        {
            List<SelectListItem> list = _context.DocumentTypes
                .Select(t =>
              new SelectListItem
              {
                  Text = t.Nom+"-" + t.Name,
                  Value = $"{t.Id}"
              })

                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un Tipo de Documento]",
                Value = "0"
            });
            return list;
        }
    }
}
