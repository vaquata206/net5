﻿namespace WebClient.Core.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Controler { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        public bool Show { get; set; }
        public string Icon { get; set; }
    }
}
