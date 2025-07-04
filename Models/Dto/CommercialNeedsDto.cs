﻿using System;

namespace EcoinverGMAO_api.Models.Dto
{
    public class CommercialNeedsDto
    {
        public int Id { get; set; }
        public int ClientCode { get; set; }
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string NombreUsuario { get; set; }
        public decimal Kgs { get; set; }
        public decimal KgsPlan { get; set; }
        public decimal Pendiente { get; set; }
        // Campos para el género
        public int IdGenero { get; set; }
        public string NombreGenero { get; set; }
    }
}
