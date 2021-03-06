﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HappyStation.Core.Entities
{
    public class News : DatabaseEntity
    {
        public News()
        {
            Type = NewsType.News;
        }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Title { get; set; }

        public string Image { get; set; }

        [DefaultValue(NewsType.News)]
        public NewsType Type { get; set; }

        [Required]
        public string Description { get; set; }

        public string Alias { get; set; }

        public string Keywords { get; set; }
    }
}
