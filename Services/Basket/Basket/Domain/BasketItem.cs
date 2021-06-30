using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using MongoDB.Bson.Serialization.Attributes;

namespace Basket.Domain
{
    /// <summary>
    /// Basket item.
    /// </summary>
    public class BasketItem : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasketItem"/> class.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="quantity">Quantity.</param>
        private BasketItem(Guid id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }

        /// <summary>
        /// Gets identifier.
        /// </summary>
        [BsonId]
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets quantity.
        /// </summary>
        [BsonElement("quantity")]
        public int Quantity { get; private set; }

        /// <summary>
        /// Creates basket item.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="quantity">Quantity.</param>
        /// <returns>Basket item.</returns>
        public static Result<BasketItem> Create(Guid id, int quantity)
        {
            if (quantity <= 0)
            {
                return Result.Failure<BasketItem>("Quantity should be greater than 0");
            }

            return new BasketItem(id, quantity);
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return Quantity;
        }
    }
}