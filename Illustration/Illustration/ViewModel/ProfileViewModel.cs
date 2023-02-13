﻿using Illustration.Models;

namespace Illustration.ViewModel
{
    public class ProfileViewModel
    {
        public Portrait Portrait { get; set; }
        public List<Portrait> Portraits { get; set; } = new List<Portrait>();
    }
}