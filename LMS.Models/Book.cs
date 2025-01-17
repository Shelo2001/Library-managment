﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Publisher { get; set; }  
        public required DateTime PublicationDate { get; set; }
        public required string ISBN { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
    }
}
