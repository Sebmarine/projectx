﻿using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Comments
{
    public class CommentBase
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public int? ParentId { get; set; }
        public int EntityId { get; set; }
        public LookUp EntityType { get; set; }
        public DateTime DateCreated { get; set; }
        public User CreatedBy { get; set; }
    }
}
