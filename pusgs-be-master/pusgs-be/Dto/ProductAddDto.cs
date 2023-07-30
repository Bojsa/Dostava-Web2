using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pusgs_be.Dto
{
    public class ProductAddDto
    {
        public string Name { get; set; }
        public List<string> Components { get; set; }
        public float Price { get; set; }
    }
}
