using System;
using System.Collections.Generic;
using System.Text;

namespace TerraTome.Domain.Dtos
{
    public class AttributeDto
    {
        public required string Name { get; set; }
        public required Type Type { get; set; }
        public required object Value { get; set; }
    }
}
