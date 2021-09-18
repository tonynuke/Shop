using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using DataAccess.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Basket.Domain
{
    /// <summary>
    /// Basket.
    /// </summary>
    public sealed class Basket : EntityBase
    {
        private HashSet<BasketItem> _items = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="Basket"/> class.
        /// </summary>
        /// <param name="buyerId">Buyer Id.</param>
        /// <param name="creationDate">Creation date.</param>
        public Basket(Guid buyerId, DateTime creationDate)
        {
            Id = buyerId;
            CreationDate = creationDate;
        }

        /// <summary>
        /// Gets a value indicating whether basket is empty.
        /// </summary>
        [BsonIgnore]
        public bool IsEmpty => !_items.Any();

        /// <summary>
        /// Gets items.
        /// </summary>
        [BsonElement("items")]
        public IReadOnlyCollection<BasketItem> Items
        {
            get => _items;
            private set => _items = value.ToHashSet();
        }

        /// <summary>
        /// Gets creation date.
        /// </summary>
        [BsonElement("creationDate")]
        public DateTime CreationDate { get; private set; }

        /// <summary>
        /// Adds or updates the <paramref name="item"/> to the basket.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <remarks>
        /// If the <paramref name="item"/> is already presented in basket, then changes the item quantity.
        /// </remarks>
        public void AddOrUpdateItem(BasketItem item)
        {
            _items.Add(item);
        }

        /// <summary>
        /// Removes the item from the basket by <paramref name="catalogItemId"/>.
        /// </summary>
        /// <param name="catalogItemId">Catalog item identifier.</param>
        public void RemoveItem(Guid catalogItemId)
        {
            _items.RemoveWhere(item => item.Id == catalogItemId);
        }

        /// <summary>
        /// Finds an item by the item id.
        /// </summary>
        /// <param name="catalogItemId">Item id.</param>
        /// <returns>Item.</returns>
        public Maybe<BasketItem> FindItem(Guid catalogItemId)
        {
            return _items.SingleOrDefault(item => item.Id == catalogItemId);
        }

        /// <summary>
        /// Replaces basket items with <paramref name="items"/>.
        /// </summary>
        /// <param name="items">Items.</param>
        public void ReplaceBasketItems(IReadOnlyCollection<BasketItem> items)
        {
            Clear();
            foreach (var item in items)
            {
                AddOrUpdateItem(item);
            }
        }

        /// <summary>
        /// Clears basket.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }
    }
}
