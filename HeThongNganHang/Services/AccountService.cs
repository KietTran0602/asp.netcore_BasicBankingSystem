using HeThongNganHang.Models;

namespace HeThongNganHang.Services;

public interface AccountService
{
    public bool Login(string username, string password);
    public bool Create(Account account);
    public Account findByUsername(string username);
    public bool Update(Account account);


}
