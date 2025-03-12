namespace Aspire_POS.Models
{
    public class CashRegisterMainModel
    {
        public UserModel CurrentUser { get; set; }
        public List<ProductModel> CashRegisters { get; set; }
    }
}
