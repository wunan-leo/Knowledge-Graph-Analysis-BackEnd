using System;
using System.Collections.Generic;

namespace Knowledge_Graph_Analysis_BackEnd.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string CommentContent { get; set; } = null!;
        public string CommentTime { get; set; } = null!;
    }
}
