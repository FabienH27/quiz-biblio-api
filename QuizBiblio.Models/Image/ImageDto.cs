using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBiblio.Models.Image;

public class ImageDto
{
    public required string OriginalUrl { get; set; }

    public string? ResizedUrl { get; set; }

}
