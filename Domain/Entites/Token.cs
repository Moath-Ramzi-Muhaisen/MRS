using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entites
{
    public class Token
    {
        public Guid Id { get; set; }
        public string TokenStr { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
