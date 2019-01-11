using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventos.IO.Application.ViewModels
{
    public class CategoriaViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public SelectList Estados()
        {
            return new SelectList(CategoriaViewModel.ListarCategorias(), "UF", "Nome");
        }

        public static List<CategoriaViewModel> ListarCategorias()
        {
            return new List<CategoriaViewModel>()
            {
                new CategoriaViewModel() { Id = new Guid("8998615A-DE8B-4BC1-B350-AD4940A8D9D7"), Nome = "Congresso" },
                new CategoriaViewModel() { Id = new Guid("E0E13F3E-0D20-4072-93F1-FED7BF0AA8FF"), Nome = "Meetup" },
                new CategoriaViewModel() { Id = new Guid("69724823-1AA2-4859-82CF-7515203516D7"), Nome = "Workshop" }
            };
        }
    }
}
