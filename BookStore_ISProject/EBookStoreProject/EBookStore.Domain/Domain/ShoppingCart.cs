﻿using EBookStore.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookStore.Domain.Domain
{
    public class ShoppingCart : BaseEntity
    {
        public string? OwnerId { get; set; }
        public EBookStoreAppUser? Owner { get; set; }
        public virtual ICollection<BookInShoppingCart>? ProductInShoppingCarts { get; set; }
    }
}
