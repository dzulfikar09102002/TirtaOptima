using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class CustomerTypeService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<CustomerType> GetCustomerTypes() => [.._context.CustomerTypes.Where(x => x.DeletedAt == null)];
        public CustomerType? GetCustomerType(string id) => _context.CustomerTypes.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        public void Store(CustomerType input, long userid)
        {
            _context.CustomerTypes.Add(new CustomerType
            {
                Id = input.Id,
                Deskripsi = input.Deskripsi,
                MinPakai = input.MinPakai,
                Tarif1 = input.Tarif1,
                Tarif2 = input.Tarif2,
                Denda = input.Denda,
                Retribusi = input.Retribusi,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = userid,
                UpdatedBy = userid,
            });
            _context.SaveChanges();
        }
        public void Update(CustomerType input, long userid)
        {
            CustomerType? customerType = GetCustomerType(input.Id);
            if (customerType != null)
            {
                customerType.Id = input.Id;
                customerType.Deskripsi = input.Deskripsi;
                customerType.MinPakai = input.MinPakai;
                customerType.Tarif1 = input.Tarif1;
                customerType.Tarif2 = input.Tarif2;
                customerType.Denda = input.Denda;
                customerType.Retribusi = input.Retribusi;
                customerType.UpdatedAt = DateTime.Now;
                customerType.UpdatedBy = userid;
            }
            _context.SaveChanges();
        }
        public void Delete(string id, long userid)
        {
            CustomerType? customerType = GetCustomerType(id);
            if (customerType != null)
            {
                customerType.DeletedAt = DateTime.Now;
                customerType.DeletedBy = userid;
                _context.SaveChanges();
            }
        }
    
    }
}
