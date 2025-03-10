using Aspire_POS.Models;

namespace Aspire_POS.Services
{
    public class CashRegisterService
    {
        public async Task<StaffMainModel> GetCashRegisterAsync()
        {
            return new StaffMainModel();
        }
    }
}
