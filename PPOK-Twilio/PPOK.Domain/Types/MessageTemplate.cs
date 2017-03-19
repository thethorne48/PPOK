using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPOK.Domain.Service;

namespace PPOK.Domain.Types
{
    public class MessageTemplate
    {
        [PrimaryKey, Identity]
        public int Code { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public MessageTemplate()
        {

        }

        public MessageTemplate(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }
}
