﻿
namespace PedidoStore.Core.SharedKernel
{
    public class BaseEntity : IEntity<Guid>
    {
        private readonly List<BaseEvent> _domainEvents = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class.
        /// </summary>
        protected BaseEntity() => Id = Guid.NewGuid();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        protected BaseEntity(Guid id) => Id = id;

        /// <summary>
        /// Gets the domain events associated with this entity.
        /// </summary>
        public IEnumerable<BaseEvent> DomainEvents =>
            _domainEvents.AsReadOnly();

        /// <summary>
        /// Gets the unique identifier of this entity.
        /// </summary>
        public Guid Id { get; private init; }

        /// <summary>
        /// Adds a domain event to the entity.
        /// </summary>
        /// <param name="domainEvent">The domain event to add.</param>
        protected void AddDomainEvent(BaseEvent domainEvent) =>
            _domainEvents.Add(domainEvent);

        /// <summary>
        /// Clears all the domain events associated with this entity.
        /// </summary>
        public void ClearDomainEvents() =>
            _domainEvents.Clear();
    }
}
