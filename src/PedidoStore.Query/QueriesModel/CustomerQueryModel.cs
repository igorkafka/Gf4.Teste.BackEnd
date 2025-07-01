


using PedidoStore.Query.Abstractions;

namespace PedidoStore.Query.QueriesModel
{
    public class CustomerQueryModel : IQueryModel<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public CustomerQueryModel()
        {

        }
        public CustomerQueryModel(Guid id, string name, string email, string phone)
        {
            Id = id;
            Name = name;
            Email = email;  
            Phone = phone;
        }
    }
}
