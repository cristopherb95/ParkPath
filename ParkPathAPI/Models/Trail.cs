﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkPathAPI.Models
{
    public enum DifficultyType
    {
        Easy,
        Moderate,
        Difficult,
        Expert
    }

    public class Trail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }
        [ForeignKey(nameof(NationalParkId))]
        public NationalPark NationalPark { get; set; }

        public DateTime DateCreated { get; set; }
    }

}